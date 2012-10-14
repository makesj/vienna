namespace Vienna.Rendering
{
    public class SpriteBatchItem
    {
        public float Depth { get; set; }
        public Vertex[] Vertices { get; set; }

        public SpriteBatchItem()
        {
            Vertices = new Vertex[4];
            Vertices[0] = new Vertex();
            Vertices[1] = new Vertex();
            Vertices[2] = new Vertex();
            Vertices[3] = new Vertex();
        }
    }
}