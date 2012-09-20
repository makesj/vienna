using System;
using System.Collections.Generic;

namespace Vienna.Eventing
{
    public class EventManager : IEventManager
    {
        // TODO: 1. Make event queue a list of lists of events
        // TODO: 2. Implement Update method

        #region Singleton Instance

        private class Nested
        {
            internal static readonly EventManager EventManager = new EventManager();
        }

        public static EventManager Instance
        {
            get { return Nested.EventManager; }
        }

        #endregion

        public EventManager()
        {
            EventDelegateMap = new Dictionary<long, List<MulticastDelegate>>();
        }

        private const int INFINITE = -1;

        private List<IEventData> EventQueue = new List<IEventData>();
        private Dictionary<long, List<MulticastDelegate>> EventDelegateMap { get; set; }

        public bool AddListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData
        {
            Logger.Debug("Events: Trying to add delegate function for event type: {0}", eventType);

            if (!EventDelegateMap.ContainsKey(eventType))
            {
                EventDelegateMap.Add(eventType, new List<MulticastDelegate>());
            }

            if (EventDelegateMap[eventType].Contains(eventHandler))
            {
                Logger.Warn("Attempting to double-register a delegate");
                return false;
            }
                

            EventDelegateMap[eventType].Add(eventHandler);
            Logger.Debug("Events: Successfully added delegate for event type: {0}", eventType);

            return true;
        }

        public bool RemoveListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData
        {
            var success = false;

            Logger.Debug("Events: Attempting to remove delegate function from event type: {0}", eventType);

            if (EventDelegateMap.ContainsKey(eventType))
            {
                var eventListeners = EventDelegateMap[eventType];

                for (var i = 0; i < eventListeners.Count; i++)
                {
                    var item = eventListeners[i];

                    if (item != eventHandler) continue;

                    eventListeners.RemoveAt(i);
                    if (eventListeners.Count == 0) EventDelegateMap.Remove(eventType);
                    Logger.Debug("Events: Successfully removed delegate function from event type: {0}", eventType);

                    success = true;
                    break;
                }
            }

            return success;
        }

        public bool TriggerEvent(IEventData eventData)
        {
            var processed = false;

            Logger.Debug("Events: Attempting to trigger event {0}", eventData.Name);

            // TODO: Process the active queue.

            if (EventDelegateMap.ContainsKey(eventData.EventType))
            {
                foreach (var eventDelegate in EventDelegateMap[eventData.EventType])
                {
                    Logger.Debug("Events: Sending Event {0} to delegate.", eventData.Name);
                    eventDelegate.DynamicInvoke(eventData);
                    processed = true;
                }
            }

            return processed;
        }

        public bool QueueEvent(IEventData eventData)
        {
            var success = false;

            Logger.Debug("Events: Attempting to queue event: {0}", eventData.Name);

            if (EventDelegateMap.ContainsKey(eventData.EventType))
            {
                // TODO: Implement active queuing.
                EventQueue.Add(eventData);
                Logger.Debug("Events: Successfully queued event: {0}", eventData.Name);
                success = true;
            }
            else
            {
                Logger.Debug("Events: Skipping event since there are no delegates registered to receive it: {0}", eventData.Name);
            }

            return success;
        }

        public bool AbortEvent(long eventType, bool allOfType)
        {
            // TODO: Implement active queue aborting.

            var success = false;

            if (EventDelegateMap[eventType].Count > 0)
            {
                while (EventDelegateMap[eventType].Count > 0)
                {
                    EventDelegateMap[eventType].RemoveAt(0);
                    success = true;
                    if (!allOfType) break;
                }
            }

            return success;
        }

        public bool Update(int maxMilliseconds)
        {
            var success = false;

            var currentMilliseconds = GetTicks();
            var maxMs = (maxMilliseconds == INFINITE) 
                ? INFINITE : currentMilliseconds + maxMilliseconds;

            // TODO: Process the active queue.
            Logger.Debug("Event Loop: Processing event queue; {0} events to process.", EventQueue.Count);

            while (EventQueue.Count > 0)
            {
                Logger.Debug("Event Loop: Processing Event {0}", EventQueue[0].Name);
                if (EventDelegateMap.ContainsKey(EventQueue[0].EventType))
                {
                    Logger.Debug("Event Loop: Found {0} delegates", EventDelegateMap[EventQueue[0].EventType].Count);
                    foreach (var eventDelegate in EventDelegateMap[EventQueue[0].EventType])
                    {
                        Logger.Debug("Event Loop: Sending event {0} to delegate", EventQueue[0].Name);
                        eventDelegate.DynamicInvoke(EventQueue[0]);
                        EventQueue.RemoveAt(0);
                        success = true;
                    }
                }

                currentMilliseconds = GetTicks();

                if (maxMilliseconds != INFINITE && currentMilliseconds >= maxMs)
                {
                    Logger.Debug("Event Loop: Aborting event processing; time ran out");
                    break;
                }
            }

            // TODO: Check if the queue is empty.  If not empty then there
            // wasn't enough time to process the entire queue. We need to
            // push events that weren't complete to the next queue so they can
            // be processed in the next update.

            // TODO: Empty the current queue.
            // TODO: Set the queue to the next queue.

            return success;
        }

        // TODO: Move this to a helper.
        public long GetTicks()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
