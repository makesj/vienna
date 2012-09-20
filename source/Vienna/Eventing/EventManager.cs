﻿using System;
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

        private List<IEventData> EventQueue = new List<IEventData>();
        private Dictionary<long, List<MulticastDelegate>> EventDelegateMap { get; set; }

        public bool AddListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData
        {
            if (!EventDelegateMap.ContainsKey(eventType))
            {
                EventDelegateMap.Add(eventType, new List<MulticastDelegate>());
            }

            if (EventDelegateMap[eventType].Contains(eventHandler)) return false;

            EventDelegateMap[eventType].Add(eventHandler);
            return true;
        }

        public bool RemoveListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData
        {
            var success = false;

            if (EventDelegateMap.ContainsKey(eventType))
            {
                var eventListeners = EventDelegateMap[eventType];

                for (var i = 0; i < eventListeners.Count; i++)
                {
                    var item = eventListeners[i];

                    if (item != eventHandler) continue;

                    eventListeners.RemoveAt(i);
                    success = true;
                    break;
                }
            }

            return success;
        }

        public bool TriggerEvent(IEventData eventData)
        {
            var success = false;

            // TODO: Process the active queue.

            if (EventDelegateMap.ContainsKey(eventData.EventType))
            {
                foreach (var eventDelegate in EventDelegateMap[eventData.EventType])
                {
                    eventDelegate.DynamicInvoke(eventData);
                    success = true;
                }
            }

            return success;
        }

        public bool QueueEvent(IEventData eventData)
        {
            var success = false;

            if (EventDelegateMap.ContainsKey(eventData.EventType))
            {
                // TODO: Implement active queuing.

                EventQueue.Add(eventData);
                success = true;
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
            // TODO: Implement
            throw new NotImplementedException();
        }
    }
}