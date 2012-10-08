using OpenTK;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public interface IRenderingComponent : IComponent
    {
        BatchBuffer Buffer { get; set; }
        Batch Target { get; }

        Vector2[] Vertices { get; }
        Vector2[] Normals { get; }
        int Frame { get; }
        bool Changed { get; set; }
    }
}