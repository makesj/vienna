using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Eventing.Events
{
    public class EventData_Play_Sound : IEventData
    {
        public const long Type = 0x005B20B5;
        public long EventType
        {
            get { return Type; }
        }

        public string Name { get { return "EventData_Play_Sound"; } }

        public float Timestamp { get; protected set; }

        public string SoundResource {get; protected set;}

        public EventData_Play_Sound(string soundResource)
        {
            Timestamp = DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
            SoundResource = soundResource;
        }
    }
}
