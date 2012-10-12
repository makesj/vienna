using System.Xml.Linq;

namespace Vienna.Actors
{
    public interface IComponent
    {
        int Id { get; }
        Actor Parent { get; }
        void Resolve(XElement el);
        void Initialize(Actor parent);
        void Update(double time);
        void Destroy();
    }
}