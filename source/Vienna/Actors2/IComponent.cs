
namespace Vienna.Actors2
{
    public interface IComponent
    {
        Actor Owner { get; set; }
        string Id { get; }
        void Init();
        void PostInit();
        void Update(int delta);
        void Destroy();
        void Changed();
        string Serialize();
    }
}