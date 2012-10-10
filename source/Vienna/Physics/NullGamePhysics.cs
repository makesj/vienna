using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;

namespace Vienna.Physics
{
    public class NullGamePhysics : IGamePhysics
    {
        public bool Initialize()
        {
            return true;
        }

        public void SyncVisibleScene()
        {
            
        }

        public void OnUpdate(double deltaSeconds)
        {
            
        }

        public void AddSphere(float radius, Actors.Actor actor, string densityStr, string physicsMaterial)
        {
            
        }

        public void AddBox(Vector dimensions, Actors.Actor gameActor, string densityStr, string physicsMaterial)
        {
           
        }

        public void AddPointCloud(IList<Vector> verts, int numPoints, Actors.Actor gameActor, string densityStr, string physicsMaterial)
        {
            
        }

        public void RemoveActor(long id)
        {
            
        }

        public void RenderDiagnostics()
        {
           
        }

        public void CreateTrigger(Actors.Actor pGameActor, Vector pos, float dim)
        {
            
        }

        public void ApplyForce(Vector dir, float newtons, long aid)
        {
           
        }

        public void ApplyTorque(Vector dir, float newtons, long aid)
        {
            
        }

        public bool KinematicMove(Matrix mat, long aid)
        {
            return true;
        }

        public void RotateY(long actorId, float angleRadians, float time)
        {
            
        }

        public float GetOrientationY(long actorId)
        {
            return 0.0f;
        }

        public void StopActor(long actorId)
        {
           
        }

        public Vector GetVelocity(long actorId)
        {
            return new Vector();
        }

        public void SetVelocity(long actorId, Vector vel)
        {
            
        }

        public Vector GetAngularVelocity(long actorId)
        {
            return new Vector();
        }

        public void SetAngularVelocity(long actorId, Vector vel)
        {
           
        }

        public void Translate(long actorId, Vector vec)
        {
            
        }

        public void SetTransform(long id, Matrix mat)
        {
           
        }

        public Matrix GetTransform(long id)
        {
            return Matrix.Identity;
        }

        public void Dispose()
        {
            
        }
    }
}
