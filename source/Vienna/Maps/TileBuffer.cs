using OpenTK.Graphics.OpenGL;
using Vienna.Rendering;

namespace Vienna.Maps
{
    public class TileBuffer : BatchBuffer
    {
        private const int MaxTiles = 2000;
        private const int VertexPerTile = 4;
        private readonly int[] _indexTemplate = { 0, 1, 2, 2, 1, 3 };

        public TileBuffer(Shader shader, TextureAtlas atlas) :
            base(MaxTiles, VertexPerTile, RenderPass.Map, Batch.Tile, shader, atlas)
        {
        }

        public override void Initialize()
        {
            var temp = new Vertex[BufferSize];
            Vbohandle = GlHelper.CreateBuffer(temp, BufferTarget.ArrayBuffer, BufferUsageHint.StaticDraw);

            var indices = BuildIndices(BufferSize, _indexTemplate, VertexPerObject);
            Ibohandle = GlHelper.CreateBuffer(indices, BufferTarget.ElementArrayBuffer, BufferUsageHint.StaticDraw);

            GlHelper.ReleaseBuffers();

            var attributes = new VertexAttribute[3];

            var in_position = new VertexAttribute();
            in_position.VertexBuffer = Vbohandle;
            in_position.Target = BufferTarget.ArrayBuffer;
            in_position.Size = 2;
            in_position.PointerType = VertexAttribPointerType.Float;
            in_position.Normalized = false;
            in_position.Stride = Vertex.SizeInBytes;
            in_position.Offset = Vertex.PositionOffset;
            in_position.ShaderHandle = Shader.Handle;
            in_position.ShaderAttribName = "in_position";
            attributes[0] = in_position;

            var in_normal = new VertexAttribute();
            in_normal.VertexBuffer = Vbohandle;
            in_normal.Target = BufferTarget.ArrayBuffer;
            in_normal.Size = 2;
            in_normal.PointerType = VertexAttribPointerType.Float;
            in_normal.Stride = Vertex.SizeInBytes;
            in_normal.Offset = Vertex.NormalOffset;
            in_normal.Normalized = false;
            in_normal.ShaderHandle = Shader.Handle;
            in_normal.ShaderAttribName = "in_normal";
            attributes[1] = in_normal;

            var in_texcoord = new VertexAttribute();
            in_texcoord.VertexBuffer = Vbohandle;
            in_texcoord.Target = BufferTarget.ArrayBuffer;
            in_texcoord.Size = 2;
            in_texcoord.PointerType = VertexAttribPointerType.Float;
            in_texcoord.Stride = Vertex.SizeInBytes;
            in_texcoord.Offset = Vertex.TextureOffset;
            in_texcoord.Normalized = false;
            in_texcoord.ShaderHandle = Shader.Handle;
            in_texcoord.ShaderAttribName = "in_texcoord";
            attributes[2] = in_texcoord;
            

            Vbahandle = GlHelper.CreateVertexArray(attributes);
        }

        public override void Process(BatchBufferInstance instance, Camera camera)
        {

        }

        public override void Render(double time, Camera camera)
        {
            Shader.SetUniformMatrix4("projection_matrix", ref camera.ProjectionMatrix);
            Shader.SetUniformMatrix4("view_matrix", ref camera.ViewMatrix);

            //GL.DrawArrays(BeginMode.TriangleStrip, 0, MaxTiles); 
        }

        public static TileBuffer CreateTestObject()
        {
            var tex = Textures.Instance.Items[Data.Images.CartoonTiles];
            var atlas = new TextureAtlas(12, 10, tex, 48);

            var shader = Shaders.Instance.Items[Data.Shaders.TileName];

            var buffer = new TileBuffer(shader, atlas);
            buffer.Initialize();

            return buffer;
        }
    }
}
