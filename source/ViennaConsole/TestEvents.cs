
using Vienna;
using Vienna.Eventing;
using Vienna.Eventing.Events;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 2)]
    public class TestEvents
    {
        private GameEventHandler<EventData_New_Actor> TriggerNewActorHandler;
        private GameEventHandler<EventData_New_Actor> QueueNewActorHandler;

        public void Execute()
        {
            var newActorEvent = new EventData_New_Actor(1, 1);

            // Testing immediate trigger of event.
            TriggerNewActorHandler = HandleTriggerNewActor;
            EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);

            Logger.Debug("One event should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);

            EventManager.Instance.RemoveListener(EventData_New_Actor.Type, TriggerNewActorHandler);
            TriggerNewActorHandler = null;

            // Testing queuing of an event
            QueueNewActorHandler = HandleQueueNewActor;
            EventManager.Instance.AddListener(EventData_New_Actor.Type, QueueNewActorHandler);

            for (var i = 0; i < 5; i++)
            {
                EventManager.Instance.QueueEvent(newActorEvent);
            }

            Logger.Debug("5 events should fire.");
            EventManager.Instance.Update(1000);

            EventManager.Instance.RemoveListener(EventData_New_Actor.Type, QueueNewActorHandler);
            QueueNewActorHandler = null;

            // Testing no listeners
            Logger.Debug("No events should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);

            // Try to register multiple listeners to same method. Should only register one.
            TriggerNewActorHandler = HandleTriggerNewActor;
            EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);
            EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);

            Logger.Debug("One event should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);
        }

        public void HandleTriggerNewActor(EventData_New_Actor eventData)
        {
            Logger.Debug("Trigger Handler firing. New actor with id {0} created in game view id {1}", eventData.ActorId, eventData.GameViewId);
        }

        public void HandleQueueNewActor(EventData_New_Actor eventData)
        {
            Logger.Debug("Queue Handler firing. New actor with id {0} created in game view id {1}", eventData.ActorId, eventData.GameViewId);
        }
    }
}
