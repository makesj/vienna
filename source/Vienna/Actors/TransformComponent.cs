using OpenTK;
using Vienna.Core;

namespace Vienna.Actors
{
    public class TransformComponent : ITransformComponent
    {
        public const int ComponentId = 1000;
        public int Id { get { return ComponentId; } }
        public Actor Parent { get; protected set; }

        public Vector3 Position { get; protected set; }
        public float ScaleFactor { get; protected set; }
        public float Rotation { get; protected set; }
        public bool Changed { get; set; }

        private Matrix4 _transform;

        public void Initialize(Actor parent)
        {
            Parent = parent;
            ScaleFactor = 1.0f;
            Changed = true;
        }

        public void Update(double time)
        {
        }

        public void Destroy()
        {
        }

        public void Move(float x, float y)
        {
            Position += new Vector3(x, y, 0);
            Changed = true;
        }

        public void Scale(float scaleFactor)
        {
            ScaleFactor = scaleFactor;
            Changed = true;
        }

        public void Rotate(float rotation)
        {
            Rotation = rotation;
            Changed = true;
        }

        public Matrix4 GetTransform()
        {
            if (!Changed) return _transform;
            _transform = MatrixHelpers.CalculateMatrix(_transform, Position, ScaleFactor, Rotation);
            Changed = false;
            return _transform;
        }

        public void SetTransform(Matrix4 worldTransform)
        {
            _transform = worldTransform;
            Changed = false;
        }
    }
}