using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Eventing.Events
{
    public class EventData_PhysicsTrigger_Leave : IEventData
    {
        public const long Type = 0xCBF54522;
        public int triggerId { get; private set; }
        public long actorId { get; private set; }

        public EventData_PhysicsTrigger_Leave(int triggerId, long actorId)
        {            
            this.triggerId = triggerId;
            this.actorId = actorId;
        }
        public long EventType
        {
            get { return Type; }
        }

        public string Name { get { return "EventData_PhysicsTrigger_Leave"; } }

        public float Timestamp { get; protected set; }
    }
}
