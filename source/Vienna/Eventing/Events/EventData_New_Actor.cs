using System;

namespace Vienna.Eventing.Events
{
    public class EventData_New_Actor : IEventData
    {
        public const long Type = 0x6B4C1561;
        public long EventType { get { return Type; } }
        public string Name { get { return "EventData_New_Actor"; } }
        public float Timestamp { get; protected set; }

        public long ActorId { get; protected set; }
        public long GameViewId { get; protected set; }

        public EventData_New_Actor(long actorId, long gameViewId)
        {
            Timestamp = DateTime.Now.Ticks;
            ActorId = actorId;
            GameViewId = gameViewId;
        }
    }
}
