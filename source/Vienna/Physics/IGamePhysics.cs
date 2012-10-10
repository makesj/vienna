using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector = OpenTK.Vector3;
using Matrix = OpenTK.Matrix4;

namespace Vienna.Physics
{
    public interface IGamePhysics : IDisposable
    {
	    // Initialiazation and Maintenance of the Physics World
	    bool Initialize();
	    void SyncVisibleScene();
	    void OnUpdate( double deltaSeconds ); 

	    // Initialization of Physics Objects
	    void AddSphere(float radius, Actors.Actor actor,string densityStr, string physicsMaterial);
	    void AddBox(Vector dimensions, Actors.Actor gameActor, string densityStr, string physicsMaterial);
        void AddPointCloud(IList<Vector> verts, int numPoints, Actors.Actor gameActor, string densityStr, string physicsMaterial);
	    void RemoveActor(long id);

	    // Debugging
	    void RenderDiagnostics();

	    // Physics world modifiers
	    void CreateTrigger(Actors.Actor pGameActor, Vector pos, float dim);
        void ApplyForce(Vector dir, float newtons, long aid);
        void ApplyTorque(Vector dir, float newtons, long aid);
        bool KinematicMove(Matrix mat, long aid);
	
	    // Physics actor states
        void RotateY(long actorId, float angleRadians, float time);
        float GetOrientationY(long actorId);
        void StopActor(long actorId);
        Vector GetVelocity(long actorId);
        void SetVelocity(long actorId, Vector vel);
        Vector GetAngularVelocity(long actorId);
        void SetAngularVelocity(long actorId, Vector vel);
        void Translate(long actorId, Vector vec);

        void SetTransform(long id, Matrix mat);
        Matrix GetTransform(long id);	    
    }
}
