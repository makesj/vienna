namespace Vienna.Eventing
{
    public interface IEventData
    {
        long EventType { get; }
        string Name { get; }
        float Timestamp { get; }
    }
}
