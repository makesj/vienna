using OpenTK.Graphics.OpenGL;
using Vienna.Extensions;
using Vienna.Rendering;

namespace Vienna.Sprites
{
    public class SpriteBuffer : BatchBuffer
    {
        private const int MaxSprites = 2000;
        private const int VertexPerSprite = 4;
        private readonly int[] _indexTemplate = {0, 1, 2, 2, 1, 3};

        public SpriteBuffer(Shader shader, TextureAtlas atlas) : base(
            MaxSprites, VertexPerSprite, RenderPass.Sprite, Batch.Sprite, shader, atlas)
        {
        }

        public override void Initialize()
        {
            var temp = new Vertex[BufferSize];
            Vbohandle = GlHelper.CreateBuffer(temp, BufferTarget.ArrayBuffer, BufferUsageHint.StreamDraw);

            var indices = BuildIndices(BufferSize, _indexTemplate, VertexPerObject);
            Ibohandle = GlHelper.CreateBuffer(indices, BufferTarget.ElementArrayBuffer, BufferUsageHint.StreamDraw);

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
            if (!instance.Changed) return;

            var v = instance.Vertices;
            var n = instance.Vertices;
            var t = Atlas.GetFrame(instance.Frame);

            var mat = instance.GetTransform();

            var vertices = new Vertex[v.Length];

            for (var i = 0; i < v.Length; i++)
            {
                var tv = v[i].Transform(ref mat);
                var nv = n[i].Transform(ref mat);
                vertices[i] = new Vertex(tv.X, tv.Y, nv.X, nv.Y, t[i].X, t[i].Y);
            }
            
            BufferData(instance, vertices);
        }

        public override void Render(double time, Camera camera)
        {
            if(Instances.Count == 0) return;

            Shader.SetUniformMatrix4("projection_matrix", ref camera.ProjectionMatrix);
            Shader.SetUniformMatrix4("view_matrix", ref camera.ViewMatrix);

            GL.DrawElements(BeginMode.Triangles, BufferSize, DrawElementsType.UnsignedInt, 0);
        }

        public static SpriteBuffer CreateTestObject()
        {
            var tex = Textures.Instance.Items[Data.Images.TerrainDebug];
            var atlas = new TextureAtlas(4, 4, tex, 64);

            var shader = Shaders.Instance.Items[Data.Shaders.TileName];

            var buffer = new SpriteBuffer(shader, atlas);
            buffer.Initialize();

            return buffer;
        }
    
    }
}