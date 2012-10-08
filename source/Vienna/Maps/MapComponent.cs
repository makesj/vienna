using OpenTK;
using Vienna.Actors;
using Vienna.Rendering;

namespace Vienna.Maps
{
    public class MapComponent : IRenderingComponent
    {
        public const int ComponentId = 4000;
        public int Id { get { return ComponentId; } }
        public Actor Parent { get; protected set; }

        public Batch Target { get; private set; }
        public BatchBuffer Buffer { get; set; }
        public int Frame { get; private set; }
        public bool Changed { get; set; }
        public Vector2[] Vertices { get; private set; }
        public Vector2[] Normals { get; private set; }

        public Map WorldMap { get; private set; }

        public void Initialize(Actor parent)
        {
            Changed = true;
            Target = Batch.Tile;
            Parent = parent;
            WorldMap = new Map(8,8,128);
            WorldMap.Initialize(120);
        }

        public void Update(double time)
        {
            
        }

        public void Destroy()
        {
            if (Buffer != null) Buffer.Release(Parent);
        }
    }
}
