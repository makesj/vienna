using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Eventing.Events
{
    public class EventData_PhysicsCollision :IEventData
    {
        public const long Type = 0xD3E7240E;
        public long actorId0 { get; private set; }
        public long actorId1 { get; private set; }
        public OpenTK.Vector3 sumNormalForce { get; private set; }
        public OpenTK.Vector3 sumFrictionForce { get; private set; }
        public List<OpenTK.Vector3> collisionPoints { get; private set; }

        public EventData_PhysicsCollision(long actorId0, long actorId1, OpenTK.Vector3 sumNormalForce, OpenTK.Vector3 sumFrictionForce, List<OpenTK.Vector3> collisionPoints)
        {            
            this.actorId0 = actorId0;
            this.actorId1 = actorId1;
            this.sumNormalForce = sumNormalForce;
            this.sumFrictionForce = sumFrictionForce;
            this.collisionPoints = collisionPoints;
        }
        public long EventType
        {
            get { return Type; }
        }

        public string Name { get { return "EventData_PhysicsCollision"; } }

        public float Timestamp { get; protected set; }
    }
}
