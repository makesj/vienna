using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Eventing.Events
{
    public class EventData_Move_Actor : IEventData
    {
        public const long Type = 0xBB4D3894;
        public long EventType
        {
            get { return Type; }
        }

        public string Name { get { return "EventData_Move_Actor"; } }

        public float Timestamp { get; protected set; }

        public long actorId { get; private set; }
        public OpenTK.Matrix4 matrix4 { get; private set; }

        public EventData_Move_Actor(long actorId, OpenTK.Matrix4 matrix4)
        {            
            this.actorId = actorId;
            this.matrix4 = matrix4;
        }
    }
}
