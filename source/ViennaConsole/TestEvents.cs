
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
		private GameEventHandler<EventData_New_Actor> QueueNewActorRenderHandler;
		
        public void Execute()
        {
			TestTrigger();
			TestQueuing();
			TestQueueWithMultipleDelegates();
			TestNoListeners();
			TestRegisterIdenticalListeners();
			TestAbortEvent();
        }

        public void HandleTriggerNewActor(EventData_New_Actor eventData)
        {
            Logger.Debug("Trigger Handler firing. New actor with id {0} created in game view id {1}", 
			             eventData.ActorId, eventData.GameViewId);
        }

        public void HandleQueueNewActor(EventData_New_Actor eventData)
        {
            Logger.Debug("Queue Handler firing. New actor with id {0} created in game view id {1}", 
			             eventData.ActorId, eventData.GameViewId);
        }
		
		public void HandleQueueRenderNewActor(EventData_New_Actor eventData)
		{
			Logger.Debug("Queue Handler firing. Rendering actoror with id {0} in game view id {1}",
			             eventData.ActorId, eventData.GameViewId);
		}
		
		#region Tests
		
		private void TestTrigger()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
			TriggerNewActorHandler = HandleTriggerNewActor;
		    EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);
	     	Logger.Debug("One event should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);
			EventManager.Instance.RemoveListener(EventData_New_Actor.Type, TriggerNewActorHandler);
            TriggerNewActorHandler = null;
		}
		
		private void TestQueuing()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
			
		    // Testing queuing of an event
            QueueNewActorHandler = HandleQueueNewActor;
            EventManager.Instance.AddListener(EventData_New_Actor.Type, QueueNewActorHandler);

            for (var i = 0; i < 5; i++)
            {
                EventManager.Instance.QueueEvent(newActorEvent);
            }
			
			// Simulate multiple calls to the update to test each queue.
            Logger.Debug("5 events should fire.");
            EventManager.Instance.Update(1000);
			
			Logger.Debug("1 event should fire.");
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.Update(1000);
			
			Logger.Debug("No events should fire.");
			EventManager.Instance.Update(1000);
			
			EventManager.Instance.RemoveListener(EventData_New_Actor.Type, QueueNewActorHandler);
            QueueNewActorHandler = null;
		}
		
		private void TestQueueWithMultipleDelegates()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
			
			QueueNewActorHandler = HandleQueueNewActor;
			QueueNewActorRenderHandler = HandleQueueRenderNewActor;
			
			EventManager.Instance.AddListener(EventData_New_Actor.Type, QueueNewActorHandler);	
			EventManager.Instance.AddListener(EventData_New_Actor.Type, QueueNewActorRenderHandler);
			
			Logger.Debug("Two methods should fire");
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.Update(1000);
			
			EventManager.Instance.RemoveListener(EventData_New_Actor.Type, QueueNewActorHandler);
            QueueNewActorHandler = null;
			
			EventManager.Instance.RemoveListener(EventData_New_Actor.Type, QueueNewActorRenderHandler);
			QueueNewActorRenderHandler = null;
		}
		
		private void TestNoListeners()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
            Logger.Debug("No events should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);
		}
		
		private void TestRegisterIdenticalListeners()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
            TriggerNewActorHandler = HandleTriggerNewActor;
            EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);
            EventManager.Instance.AddListener(EventData_New_Actor.Type, TriggerNewActorHandler);

            Logger.Debug("One event should fire.");
            EventManager.Instance.TriggerEvent(newActorEvent);
			
			EventManager.Instance.RemoveListener(EventData_New_Actor.Type, TriggerNewActorHandler);
            TriggerNewActorHandler = null;
		}
		
		private void TestAbortEvent()
		{
			var newActorEvent = new EventData_New_Actor(1, 1);
			
			QueueNewActorHandler = HandleQueueNewActor;
			EventManager.Instance.AddListener(EventData_New_Actor.Type, QueueNewActorHandler);
			
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.AbortEvent(newActorEvent.EventType, false);
			
			Logger.Debug("One event should fire");
			EventManager.Instance.Update(1000);
			
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.QueueEvent(newActorEvent);
			EventManager.Instance.AbortEvent(newActorEvent.EventType, true);
			
			Logger.Debug("No events should fire");
			EventManager.Instance.Update(1000);
			
			EventManager.Instance.RemoveListener(newActorEvent.EventType, QueueNewActorHandler);
			QueueNewActorHandler = null;
		}
		
		#endregion
	}

}