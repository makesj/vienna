using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;
using Vienna.Physics;

namespace Vienna.Physics
{

    public class PhysicsComponent : Vienna.Actors.IComponent
    {
        const float DEFAULT_MAX_VELOCITY = 7.5f;
        const float DEFAULT_MAX_ANGULAR_VELOCITY = 1.2f;

        public const string ComponentId = "physics";
        public string Id { get { return ComponentId; } }
        public Vienna.Actors.Actor Parent { get; set; }

        protected float acceleration { get; set; }
        protected float angularAcceleration { get; set; }
        protected float maxVelocity { get; set; }
        protected float maxAngularVelocity { get; set; }

        protected string shape { get; set; }
        protected string density { get; set; }
        protected string material { get; set; }

        protected Vector rigidBodyLocation { get; set; }
        protected Vector rigidBodyOrientation { get; set; }
        protected Vector rigidBodyScale { get; set; }        


        int Actors.IComponent.Id
        {
            get { return 0x26DCFDD4; }
        }

        public PhysicsComponent()
        {
            rigidBodyLocation = new Vector(0f, 0f, 0f);
            rigidBodyOrientation = new Vector(0f, 0f, 0f);
            rigidBodyScale = new Vector(1f, 1f, 1f);

            acceleration = 0;
            angularAcceleration = 0;
            maxVelocity = DEFAULT_MAX_VELOCITY;
            maxAngularVelocity = DEFAULT_MAX_ANGULAR_VELOCITY;
            shape = "box";
        }

        public void _setUp(string shape = null, string density = null, string material = null)
        {
            if (shape != null) this.shape = shape;
            if (density != null) this.density = density;
            if (material != null) this.material = material;
        }

        ~PhysicsComponent()
        {
            GlobalPhysics.Instance.RemoveActor(Parent.Id);
        }

        public void Initialize(Actors.Actor owner)
        {
            Parent = owner;


            PostInit();
        }
       
        public void PostInit()
        {
            if (Parent != null)
            {
                switch (this.shape.ToLower())
                {
                    case "sphere":
                        GlobalPhysics.Instance.AddSphere(rigidBodyScale.X, Parent, density, material);
                        break;
                    case "box":
                        GlobalPhysics.Instance.AddBox(rigidBodyScale, Parent, density, material);
                        break;
                    default:
                        throw new NotImplementedException(string.Format("Do not know how to create:{0}", this.shape)); 
                }
            }
        }

        public void Update(double delta)
        {
            var transformComponent = Parent.GetComponent<Vienna.Actors.TransformComponent>(Vienna.Actors.TransformComponent.ComponentId);
            if (transformComponent == null)
                throw new InvalidOperationException("Can't Rotate Actor without Transform Component");

            var transform = transformComponent.GetTransform();

            if (acceleration != 0)
            {
                var accelerationToApplyThisFrame = acceleration / 1000f * (float)delta;
                var velocity = GlobalPhysics.Instance.GetVelocity(Parent.Id);
                var velocityScalar = velocity.Length;                
                //var direction = transform.GetDirection();

                //GlobalPhysics.Instance.ApplyForce(direction, accelerationToApplyThisFrame, Owner.Id);
            }

            if (angularAcceleration != 0)
            {
                //var angularAccelerationToApplyThisFrame = angularAcceleration / 1000f * (float)delta;
                //GlobalPhysics.Instance.ApplyTorque(transform.GetUp(), angularAccelerationToApplyThisFrame, Owner.Id);
            }

        }

        public void Destroy()
        {
            
        }

        public void Changed()
        {
            
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        //Physics
        public void ApplyForce(Vector direction, float forceNewtons)
        {
            GlobalPhysics.Instance.ApplyForce(direction, forceNewtons, Parent.Id);
        }
        
        public void ApplyTorque(Vector direction, float forceNewtons)
        {
            GlobalPhysics.Instance.ApplyTorque(direction, forceNewtons, Parent.Id);
        }
        
        public bool KinematicMove(Matrix transform)
        {
            return GlobalPhysics.Instance.KinematicMove(transform, Parent.Id);
        }

        //Accelerations
        public void ApplyAcceleration(float acceleration)
        {
            this.acceleration = acceleration;
        }
        
        public void RemoveAcceleration()
        {
            acceleration = 0;
        }
        
        public void ApplyAngularAcceleration(float acceleration)
        {
            angularAcceleration = acceleration;
        }
        
        public void RemoveAngularAcceleration()
        {
            angularAcceleration = 0;
        }
	    
        public Vector GetVelocity()
        {
            return GlobalPhysics.Instance.GetVelocity(Parent.Id);
        }
        
        public void SetVelocity(Vector velocity)
        {
            GlobalPhysics.Instance.SetVelocity(Parent.Id, velocity);
        }

        public void RotateY(float angleRadians)
        {
            var transformComponent = Parent.GetComponent<Vienna.Actors.TransformComponent>(Vienna.Actors.TransformComponent.ComponentId);
            if (transformComponent == null) 
                throw new InvalidOperationException("Can't Rotate Actor without Transform Component");

            var transform = transformComponent.GetTransform();
            var position = new Vector(transform.M31, transform.M32, transform.M33);
        }

        public void SetPosition(float x, float y, float z)
        {
            throw new NotImplementedException();
        }
        
        public void Stop()
        {
            GlobalPhysics.Instance.StopActor(Parent.Id);
        }


        protected void CreateShape()
        {

        }        
    }
}
