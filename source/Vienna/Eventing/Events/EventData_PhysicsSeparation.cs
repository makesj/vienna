using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Eventing.Events
{
    public class EventData_PhysicsSeparation :IEventData
    {
        public const long Type = 0x3C2CE779;
        public long actorId0 { get; private set; }
        public long actorId1 { get; private set; }

        public EventData_PhysicsSeparation(long actorId0, long actorId1)
        {            
            this.actorId0 = actorId0;
            this.actorId1 = actorId1;
        }
        public long EventType
        {
            get { return Type; }
        }

        public string Name { get { return "EventData_PhysicsSeparation"; } }

        public float Timestamp { get; protected set; }
    }
}
