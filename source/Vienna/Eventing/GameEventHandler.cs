namespace Vienna.Eventing
{
    public delegate void GameEventHandler<in T>(T message) where T : IEventData;
}
