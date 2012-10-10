using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletSharp;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;

using Vienna.Eventing;
using Vienna.Actors;
using Vienna.Eventing.Events;
using Vienna.Rendering;


namespace Vienna.Physics
{
       
    public class BulletGamePhysics : IGamePhysics
    {
        protected DynamicsWorld dynamicsWorld { get; set; }
        protected BroadphaseInterface broadphase { get; set; }
        protected CollisionDispatcher dispatcher { get; set; }
        protected ConstraintSolver solver { get; set; }
        protected DefaultCollisionConfiguration collisionConfiguration { get; set; }
        protected DebugDraw debugDrawer { get; set; }

        protected Dictionary<string, float> DensityTable { get; set; }
        protected Dictionary<string, MaterialData> MaterialTable { get; set; }
        protected Dictionary<long, RigidBody> actorIdRigidBody { get; set; }
        protected Dictionary<RigidBody, long> rigidBodyToActorId{ get; set; }

        protected IList<Tuple<RigidBody, RigidBody>> previousTickCollisionPairs { get; set; }

        public BulletGamePhysics()
        {
            DensityTable = new Dictionary<string, float>();
            MaterialTable = new Dictionary<string, MaterialData>();
            actorIdRigidBody = new Dictionary<long, RigidBody>();
            previousTickCollisionPairs = new List<Tuple<RigidBody, RigidBody>>();
            rigidBodyToActorId = new Dictionary<RigidBody, long>();
        }
        
        protected void AddShape(Actors.Actor actor, CollisionShape shape, float mass, string physicsMaterial)
        {
            var material = LookupMaterialTable(physicsMaterial);
            var localInteria = new Vector(0f, 0f, 0f);
            if (mass > 0f)
            {
                shape.CalculateLocalInertia(mass,out localInteria);
            }

            var transform = Matrix.Identity;
            var transformComponent = actor.GetComponent<TransformComponent>(TransformComponent.ComponentId);
            if (transformComponent == null)
            {
                throw new InvalidOperationException("Physics can't work on an actor that doesn't have a transform Component");
            }

            transform = transformComponent.GetTransform();
            var motion = new ActorMotionState(transform);
            var rbInfo = new BulletSharp.RigidBodyConstructionInfo(mass, motion, shape, localInteria)
            {
                Restitution = material.Restitution,
                Friction = material.Friction
            };

            var body = new RigidBody(rbInfo);
            dynamicsWorld.AddRigidBody(body);
            actorIdRigidBody.Add(actor.Id, body);
            rigidBodyToActorId.Add(body, actor.Id);
        }

        protected void RemoveCollisionObject(CollisionObject removeObject)
        {
            dynamicsWorld.RemoveCollisionObject(removeObject);

            for(var i = 0; i < previousTickCollisionPairs.Count; i++)
            {
                var item = previousTickCollisionPairs[i];
                if (removeObject == item.Item1 || removeObject == item.Item2 )
                {
                    SendCollisionPairRemoveEvent(item.Item1, item.Item2);
                    previousTickCollisionPairs.RemoveAt(i);
                }
            }
            
            
            RigidBody body = removeObject as RigidBody;
            if (body != null && body.MotionState != null)
            {
                body.MotionState.Dispose();
                
            }

            if (body != null && body.CollisionShape != null)
            {
                body.CollisionShape.Dispose();

            }
           
            body.UserObject = null;

            
            removeObject.Dispose();
            body.Dispose();
        }

        static void BulletInternalTickCallback(DynamicsWorld world, float timeStep)
        {
            if (world == null)
                throw new InvalidOperationException("World was null");

            if (world.WorldUserInfo == null)
                throw new InvalidOperationException("User info was null");

            var bulletPhysics = world.WorldUserInfo as BulletGamePhysics;
            if (bulletPhysics == null)
                throw new InvalidCastException(string.Format("Physics World is somehow not BulletGamePhysics type. Type:{0}",world.WorldUserInfo.GetType()));

            IList<Tuple<RigidBody, RigidBody>> currentTickCollisionPairs = new List<Tuple<RigidBody,RigidBody>>();
            var dispatcher = world.Dispatcher;
            for (var i = 0; i < dispatcher.NumManifolds; i++)
            {
                var manifold = dispatcher.GetManifoldByIndexInternal(i);
                if (manifold == null)
                    throw new InvalidOperationException("Manifold was null");
                var body0 = manifold.Body0 as RigidBody;
                var body1 = manifold.Body1 as RigidBody;

                bool swapped = body0.GetHashCode() > body1.GetHashCode();
                var sortedBodyA = swapped ? body1 : body0;
                var sortedBodyB = swapped ? body0 : body1;
                var pair = Tuple.Create<RigidBody,RigidBody>(sortedBodyA, sortedBodyB);
                currentTickCollisionPairs.Add(pair);
                if (!bulletPhysics.previousTickCollisionPairs.Contains(pair))
                {
                    bulletPhysics.SendCollisionPairAddEvent(manifold, body0, body1);
                }
            }

            foreach (var pair in bulletPhysics.previousTickCollisionPairs.Except(currentTickCollisionPairs))
            {
                bulletPhysics.SendCollisionPairRemoveEvent(pair.Item1, pair.Item2);
            }
            bulletPhysics.previousTickCollisionPairs = currentTickCollisionPairs;
        }

        protected void LoadXml()
        {
            var doc = new System.Xml.XmlDocument();
            doc.Load("Assets\\physics.xml");            
            var materials = doc.GetElementsByTagName("PhysicsMaterials")[0];
            for (var i = 0; i < materials.ChildNodes.Count; i++)
            {
                float restitution = 0f;
                float friction = 0f;
                var node = materials.ChildNodes[i];
                if (node.NodeType == System.Xml.XmlNodeType.Comment) continue;
                restitution = (float)Convert.ToDouble(node.Attributes["restitution"].Value);
                friction = (float)Convert.ToDouble(node.Attributes["friction"].Value);
                MaterialTable.Add(node.Name, new MaterialData(restitution,friction));
            }
            var density = doc.GetElementsByTagName("DensityTable")[0];
            for (var i = 0; i < density.ChildNodes.Count; i++)
            {
                var node = density.ChildNodes[i];
                if (node.NodeType == System.Xml.XmlNodeType.Comment) continue;
                DensityTable.Add(node.Name, (float)Convert.ToDouble(node.FirstChild.Value));
            }
        }

        protected float LookupSpecificGravity(string density)
        {
            float value = 0f;
            DensityTable.TryGetValue(density, out value);
            return value;
        }

        protected MaterialData LookupMaterialTable(string material)
        {
            MaterialData value = null;
            MaterialTable.TryGetValue(material, out value);
            return value;
        }

        protected RigidBody FindBulletRigidBody(long actorId)
        {
            RigidBody body = null;
            actorIdRigidBody.TryGetValue(actorId, out body);
            return body;
        }

        protected long FindActorID(RigidBody body)
        {
            long actorid = 0;
            rigidBodyToActorId.TryGetValue(body, out actorid);
            return actorid;
        }

        protected Actor GetActor(long id)
        {
            return ActorList.Actors[(int)id]; 
        }

        protected void SendCollisionPairAddEvent(PersistentManifold maifold, RigidBody body0, RigidBody body1)
        {
            if (body0.UserObject != null || body1.UserObject != null)
            {
                RigidBody triggerBody = null,
                    otherBody = null;
                if (body0.UserObject != null)
                {
                    triggerBody = body0;
                    otherBody = body1;
                }
                else
                {
                    triggerBody = body1;
                    otherBody = body0;
                }

                var triggerId = (int)triggerBody.UserObject;
                var triggerEvent = new EventData_PhysicsTrigger_Enter(triggerId, FindActorID(otherBody));
                EventManager.Instance.QueueEvent(triggerEvent);
            }
            else
            {
                var actorId0 = FindActorID(body0);
                var actorId1 = FindActorID(body1);
                if (actorId0 == 0 || actorId1 == 0)
                {
                    System.Diagnostics.Trace.WriteLine("Collided with a non Actor");
                }

                var collisionPoints = new List<Vector>();
                var sumNormalForce = new Vector();
                var sumFrictionForce = new Vector();

                for (var i = 0; i < maifold.NumContacts; i++)
                {
                    var point = maifold.GetContactPoint(i);
                    collisionPoints.Add(point.PositionWorldOnB);
                    sumNormalForce += point.CombinedRestitution * point.NormalWorldOnB;
                    sumFrictionForce += point.CombinedFriction * point.LateralFrictionDir1;
                }

                var collisionEvent = new EventData_PhysicsCollision(actorId0, actorId1, sumNormalForce, sumFrictionForce, collisionPoints);
                EventManager.Instance.QueueEvent(collisionEvent);
            }
        }

        protected void SendCollisionPairRemoveEvent(RigidBody body0, RigidBody body1)
        {
            if (body0.UserObject != null || body1.UserObject != null)
            {
                RigidBody triggerBody = null,
                    otherBody = null;
                if (body0.UserObject != null)
                {
                    triggerBody = body0;
                    otherBody = body1;
                }
                else
                {
                    triggerBody = body1;
                    otherBody = body0;
                }

                var triggerId = (int)triggerBody.UserObject;
                var triggerEvent = new EventData_PhysicsTrigger_Leave(triggerId, FindActorID(otherBody));
                EventManager.Instance.QueueEvent(triggerEvent);
            }
            else
            {
                var actorId0 = FindActorID(body0);
                var actorId1 = FindActorID(body1);
                if (actorId0 == 0 || actorId1 == 0)
                {
                    System.Diagnostics.Trace.WriteLine("Collided with a non Actor");
                }

                var collisionEvent = new EventData_PhysicsSeparation(actorId0, actorId1);
                EventManager.Instance.QueueEvent(collisionEvent);
            }
        }

        public bool Initialize()
        {
            LoadXml();            
            collisionConfiguration = new DefaultCollisionConfiguration();

            dispatcher = new CollisionDispatcher(collisionConfiguration);
            broadphase = new DbvtBroadphase();
            solver = new SequentialImpulseConstraintSolver();
            dynamicsWorld = new DiscreteDynamicsWorld(dispatcher, broadphase, solver, collisionConfiguration);
            debugDrawer = new BullettDebugDrawer();

            dynamicsWorld.DebugDrawer = debugDrawer;
            dynamicsWorld.SetInternalTickCallback(BulletInternalTickCallback);
            dynamicsWorld.WorldUserInfo = this;
            dynamicsWorld.Gravity = new Vector(0, 10, 0);
            return true;
        }

        public void SyncVisibleScene()
        {
            foreach (var item in actorIdRigidBody)
            {
                var id = item.Key;
                var actorMotionState = item.Value.MotionState as ActorMotionState;
                if (actorMotionState == null)
                    throw new InvalidCastException("RigidBody's motionstate is not ActorMotionState");

                var actor = GetActor(id);
                if (actor != null)
                {
                    var transformComponent = actor.GetComponent<TransformComponent>(TransformComponent.ComponentId);
                    var worldTransform = actorMotionState.WorldTransform;
                    if (transformComponent != null && transformComponent.GetTransform() != worldTransform)
                    {
                        transformComponent.SetTransform(worldTransform);
                        var moveActor = new EventData_Move_Actor(id, actorMotionState.WorldTransform);
                        EventManager.Instance.QueueEvent(moveActor);
                    }
                    
                }
            }
        }

        public void OnUpdate(double deltaSeconds)
        {
            dynamicsWorld.StepSimulation((float)deltaSeconds, 4);
        }

        public void AddSphere(float radius, Actors.Actor actor, string density, string physicsMaterial)
        {
            var collisionShape = new BulletSharp.SphereShape(radius);
            var gravity = LookupSpecificGravity(density);
            var volume = (4f / 3f) * Math.PI * radius * radius * radius;
            var mass = volume * gravity;
            AddShape(actor, collisionShape, (float)mass, physicsMaterial);
        }

        public void AddPointCloud(IList<Vector> verts, int numPoints, Actors.Actor gameActor, string densityStr, string physicsMaterial)
        {
            var shape = new ConvexHullShape();
            for (var i = 0; i < numPoints; i++)
            {
                var vert = verts[i];                
                shape.AddPoint(verts[i]);
            }
            var min = new Vector(0,0,0);
            var max = new Vector(0, 0, 0);
            shape.GetAabb(Matrix.Identity, out min, out max);
            var aabbExtents = max - min;
            float specificGravity = LookupSpecificGravity(densityStr);
            float volume = aabbExtents.X * aabbExtents.Y * aabbExtents.Z;
            float mass = volume * specificGravity;

            AddShape(gameActor, shape, mass, physicsMaterial);
        }

        public void AddBox(Vector dimensions, Actors.Actor actor, string densityStr, string physicsMaterial)
        {
            var box = new BoxShape(dimensions);
            float specificGravity = LookupSpecificGravity(densityStr);
	        float volume = dimensions.X * dimensions.Y * dimensions.Z;
	        float mass = volume * specificGravity;
            AddShape(actor, box, mass, physicsMaterial);
        }
        
        public void RemoveActor(long id)
        {
            var body = FindBulletRigidBody(id);
            if (body == null) return;
            RemoveCollisionObject(body);
            actorIdRigidBody.Remove(id);
            rigidBodyToActorId.Remove(body);
        }

        public void RenderDiagnostics()
        {
            dynamicsWorld.DebugDrawWorld();
        }

        public void CreateTrigger(Actors.Actor pGameActor, Vector pos, float dim)
        {
            var boxshape = new BoxShape(new Vector(dim, dim, dim));
            const float Mass = 0f;
            var triggerTransform = Matrix.Identity;
            triggerTransform.M31 = pos.X;
            triggerTransform.M32 = pos.Y;
            triggerTransform.M34 = pos.Z;
            triggerTransform.M34 = 0;

            var motionState = new ActorMotionState(triggerTransform);
            var rbInfo = new RigidBodyConstructionInfo(Mass, motionState, boxshape, new Vector(0, 0, 0));
            var body = new RigidBody(rbInfo);
            body.CollisionFlags = body.CollisionFlags | CollisionFlags.NoContactResponse;
            dynamicsWorld.AddRigidBody(body);
            actorIdRigidBody.Add(pGameActor.Id, body);
            rigidBodyToActorId.Add(body, pGameActor.Id);            
        }

        public void ApplyForce(Vector dir, float newtons, long aid)
        {
            var body = FindBulletRigidBody(aid);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");
            var force = new Vector(dir.X * newtons, dir.Y * newtons, dir.Z * newtons);
            body.ApplyCentralForce(force);
        }

        public void ApplyTorque(Vector dir, float newtons, long aid)
        {
            var body = FindBulletRigidBody(aid);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");
            var torque = new Vector(dir.X * newtons, dir.Y * newtons, dir.Z * newtons);
            body.ApplyCentralImpulse(torque);
        }

        public bool KinematicMove(Matrix mat, long aid)
        {
            var body = FindBulletRigidBody(aid);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            body.ActivationState = ActivationState.DisableDeactivation;
            body.WorldTransform = mat;
            return true;
        }

        public void RotateY(long actorId, float angleRadians, float time)
        {
            throw new NotImplementedException();
            RigidBody body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            var angleTransform = Matrix.Identity;
            Matrix.CreateRotationY(angleRadians, out angleTransform);            

            //angleTransform.getBasis().setEulerYPR(0, angleRadians, 0); // rotation about body Y-axis

            body.CenterOfMassTransform = body.CenterOfMassTransform * angleTransform;
        }

        public float GetOrientationY(long actorId)
        {
            throw new NotImplementedException();
            /*
            btRigidBody * pRigidBody = FindBulletRigidBody(actorId);
	GCC_ASSERT(pRigidBody);
	
	const btTransform& actorTransform = pRigidBody->getCenterOfMassTransform();
	btMatrix3x3 actorRotationMat(actorTransform.getBasis());  // should be just the rotation information

	btVector3 startingVec(0,0,1);
	btVector3 endingVec = actorRotationMat * startingVec; // transform the vector

	endingVec.setY(0);  // we only care about rotation on the XZ plane

	float const endingVecLength = endingVec.length();
	if (endingVecLength < 0.001)
	{
		// gimbal lock (orientation is straight up or down)
		return 0;
	}

	else
	{
		btVector3 cross = startingVec.cross(endingVec);
		float sign = cross.getY() > 0 ? 1.0f : -1.0f;
		return (acosf(startingVec.dot(endingVec) / endingVecLength) * sign);
	}

	return FLT_MAX;  // fail...
            */
        }

        public void StopActor(long actorId)
        {
            SetVelocity(actorId, new Vector(0, 0, 0));
        }

        public void SetVelocity(long actorId, Vector vel)
        {
            var body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            body.LinearVelocity = vel;
        }

        public Vector GetVelocity(long actorId)
        {
            var body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            return body.LinearVelocity;
        }
       
        public Vector GetAngularVelocity(long actorId)
        {
            var body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            return body.AngularVelocity;
        }

        public void SetAngularVelocity(long actorId, Vector vel)
        {
            var body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");

            body.AngularVelocity = vel;
        }

        public void Translate(long actorId, Vector vec)
        {
            var body = FindBulletRigidBody(actorId);
            if (body == null)
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");
            body.Translate(vec);
        }

        public void SetTransform(long id, Matrix mat)
        {
            KinematicMove(mat, id);
        }

        public Matrix GetTransform(long id)
        {
            var body = FindBulletRigidBody(id);
            if (body == null) 
                throw new InvalidOperationException("Can't get transform for actor that doesn't exist in Physics System");
            return body.CenterOfMassTransform;
        }

        public bool IsDisposed { get; private set; }
               
        public void Dispose()
        {
            if (IsDisposed) return;
            //remove/dispose constraints
     
            for (var i = dynamicsWorld.NumConstraints - 1; i >= 0; i--)
            {
                TypedConstraint constraint = dynamicsWorld.GetConstraint(i);
                dynamicsWorld.RemoveConstraint(constraint);
                constraint.Dispose(); ;
            }

            //remove the rigidbodies from the dynamics world and delete them
            for (var i = dynamicsWorld.NumCollisionObjects - 1; i >= 0; i--)
            {
                CollisionObject obj = dynamicsWorld.CollisionObjectArray[i];
                RemoveCollisionObject(obj);
            }



            dynamicsWorld.Dispose();
            broadphase.Dispose();
            if (dispatcher != null)
            {
                dispatcher.Dispose();
            }
            collisionConfiguration.Dispose();

            rigidBodyToActorId.Clear();
            actorIdRigidBody.Clear();
            IsDisposed = true;
        }
    }
}
