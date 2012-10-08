namespace Vienna.Rendering
{
    public class Mesh2D
    {
        public Vertex[] Vertices;
        public int[] Indices;

        public Mesh2D(Vertex[] vertices, int[] indices)
        {
            Vertices = vertices;
            Indices = indices;
        }

        public int[] Offset(int offset)
        {
            var indices = new int[Indices.Length];
            for (var i = 0; i < Indices.Length; i++)
            {
                indices[i] = Indices[i] + offset;
            }
            return indices;
        }
    }
}
