using OpenTK;

namespace Vienna.Actors
{
    public interface ITransformComponent : IComponent
    {
        Vector2 Position { get; }
        float ScaleFactor { get; }
        float Rotation { get; }
        bool Changed { get; set; }
        Matrix4 GetTransform();
        void SetPosition(float x, float y);
        void Move(float x, float y);
    }
}