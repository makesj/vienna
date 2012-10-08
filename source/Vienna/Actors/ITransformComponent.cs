using OpenTK;

namespace Vienna.Actors
{
    public interface ITransformComponent : IComponent
    {
        Vector3 Position { get; }
        float ScaleFactor { get; }
        float Rotation { get; }
        bool Changed { get; set; }
        Matrix4 GetTransform();
    }
}