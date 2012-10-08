namespace Vienna.Actors
{
    public interface IComponent
    {
        int Id { get; }
        Actor Parent { get; }
        void Initialize(Actor parent);
        void Update(double time);
        void Destroy();
    }
}