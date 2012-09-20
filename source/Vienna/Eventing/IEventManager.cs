namespace Vienna.Eventing
{
    public interface IEventManager
    {
        bool AddListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData;
        bool RemoveListener<T>(long eventType, GameEventHandler<T> eventHandler) where T : class, IEventData;
        bool TriggerEvent(IEventData eventData);
        bool QueueEvent(IEventData eventData);
        bool Update(int maxMilliseconds);
    }
}
