using System;
using System.Xml.Linq;
using OpenTK;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public class SpriteComponent : IRenderingComponent
    {
        public const int ComponentId = 2000;
        public int Id { get { return ComponentId; } }
        public Actor Parent { get; protected set; }

        public int Frame { get; private set; }
        public bool Changed { get; set; }

        public int Depth { get; set; }

        public Vector2[] Vertices { get; private set; }
        public Vector2[] Normals { get; private set; }

        private double _timer;
        private Random _rand;

        public void Resolve(XElement el)
        {
        }

        public void Initialize(Actor parent)
        {
            Changed = true;
            Parent = parent;
            Vertices = Data.Quad2D.Position;
            _rand = new Random(parent.Id);
        }

        public void Update(double time)
        {
            _timer += time;

            if (_timer > 1)
            {
                Frame = _rand.Next(0, 15);
                _timer = 0;
                Changed = true;
            }
        }

        public void Destroy()
        {
        }
    }
}