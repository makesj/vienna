using System;
using System.Collections.Generic;
using System.Linq;

namespace Vienna.Eventing
{
    public class EventManager : IEventManager
    {

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
			EventQueue = new List<List<IEventData>>();
			
			for (var i = 0; i < MAX_QUEUES; i++)
			{
				EventQueue.Add(new List<IEventData>());
			}
			
			activeQueue = 0;
        }

        private const int INFINITE = -1;
		
		private int activeQueue;
		private const int MAX_QUEUES = 2;

        private List<List<IEventData>> EventQueue;
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
				EventQueue[activeQueue].Add(eventData);
                Logger.Debug("Events: Successfully queued event: {0}", eventData.Name);
                success = true;
            }
            else
            {
                Logger.Debug("Events: Skipping event since there are no delegates registered to receive it: {0}", eventData.Name);
            }

            return success;
        }
		
		// TODO: Refactor to not use LINQ. Could be slow.
        public bool AbortEvent(long eventType, bool allOfType)
        {
        	var success = false;
			
			if (EventDelegateMap.ContainsKey(eventType))
			{
				var eventQueue = EventQueue[activeQueue];
				
				if (allOfType) 
				{
					eventQueue.RemoveAll(x => x.EventType == eventType);
					success = true;
				}
				else
				{
					var eventToDelete = eventQueue.FirstOrDefault(x => x.EventType == eventType);
					if (eventToDelete != null) eventQueue.Remove(eventToDelete); success = true;
				}
			}

            return success;
        }
		
		// TODO: Break this up into helper methods.
        public bool Update(int maxMilliseconds)
        {
            var currentMilliseconds = GetTicks();
            var maxMs = (maxMilliseconds == INFINITE) 
                ? INFINITE : currentMilliseconds + maxMilliseconds;
			
			// TODO: Process the realtime queue
			
			var queueToProcess = activeQueue;
			activeQueue = (activeQueue + 1) % MAX_QUEUES;
			EventQueue[activeQueue].Clear();
			
            Logger.Debug("Event Loop: Processing event queue {0}; {1} events to process.", queueToProcess, 
			             EventQueue[queueToProcess].Count);
			
			var currentQueue = EventQueue[queueToProcess];
            while (EventQueue[queueToProcess].Count > 0)
            {
				var currentEvent = currentQueue[0];
                Logger.Debug("Event Loop: Processing Event {0}", currentEvent.Name);
                if (EventDelegateMap.ContainsKey(currentEvent.EventType))
                {
                    Logger.Debug("Event Loop: Found {0} delegates", EventDelegateMap[currentEvent.EventType].Count);
                    foreach (var eventDelegate in EventDelegateMap[currentEvent.EventType])
                    {
                        Logger.Debug("Event Loop: Sending event {0} to delegate", currentEvent.Name);
                        eventDelegate.DynamicInvoke(currentEvent);
                    }
					
					currentQueue.RemoveAt(0);
                }

                currentMilliseconds = GetTicks();

                if (maxMilliseconds != INFINITE && currentMilliseconds >= maxMs)
                {
                    Logger.Debug("Event Loop: Aborting event processing; time ran out");
                    break;
                }
            }
			
			// If the queue isn't empty, move remaining events to next queue.
			var queueFlushed = EventQueue[queueToProcess].Count == 0;
			if (!queueFlushed)
			{
				for (var i = EventQueue[queueToProcess].Count; i > 0; i--)
				{
					// Does a copy, maybe not the best way to do it.
					EventQueue[activeQueue].Insert(0, EventQueue[queueToProcess][i]);
					EventQueue[queueToProcess].RemoveAt(i);
				}
			}

            return queueFlushed;
        }

        // TODO: Move this to a helper.
        public long GetTicks()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
