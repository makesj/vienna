using System.Xml.Linq;
using OpenTK;
using Vienna.Extensions;

namespace Vienna.Actors
{
    public class TransformComponent : ITransformComponent
    {
        public const int ComponentId = 1000;
        public int Id { get { return ComponentId; } }
        public Actor Parent { get; protected set; }

        public Vector2 Position { get; protected set; }
        public float ScaleFactor { get; protected set; }
        public float Rotation { get; protected set; }
        public bool Changed { get; set; }

        private Matrix4 _transform;

        public void Resolve(XElement el)
        {
            Position = el.GetElement("Position").AttributeAsVector2("x", "y");
            ScaleFactor = el.GetElement("ScaleFactor").AttributeAsFloat("value");
            Rotation = el.GetElement("Rotation").AttributeAsFloat("value");
        }

        public void Initialize(Actor parent)
        {
            Parent = parent;
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
            Position += new Vector2(x, y);
            Changed = true;
        }

        public void SetPosition(float x, float y)
        {
            Position = new Vector2(x, y);
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
            var position = Matrix4.CreateTranslation(new Vector3(Position));
            var scale = Matrix4.Scale(ScaleFactor, ScaleFactor, 1);
            var rotation = Matrix4.CreateRotationZ(Rotation);
            _transform = rotation * scale * position;
            Changed = false;
            return _transform;
        }
    }
}