using OpenTK.Graphics.OpenGL;
using Vienna.Rendering;

namespace Vienna.Sprites
{
    public class SpriteBuffer : BatchBuffer
    {
        private const int Size = 10000;

        public SpriteBuffer(Shader shader, TextureAtlas atlas) : 
            base(Size, RenderPass.Sprite, Batch.Sprite, shader, atlas)
        {
        }

        public override void Initialize()
        {
            var temp = new Vertex[BufferSize];
            Vbohandle = GlHelper.CreateBuffer(temp, BufferTarget.ArrayBuffer, BufferUsageHint.DynamicDraw);

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

        public override void Process(BatchBufferInstance instance)
        {
            if(!instance.Changed) return;

            var v = instance.Vertices;
            var n = instance.Vertices;
            var t = Atlas.GetFrame(instance.Frame);

            var vertices = new Vertex[4];

            vertices[0] = new Vertex(v[0].X, v[0].Y, n[0].X, n[0].Y, t[0].X, t[0].Y);
            vertices[1] = new Vertex(v[1].X, v[1].Y, n[1].X, n[1].Y, t[1].X, t[1].Y);
            vertices[2] = new Vertex(v[2].X, v[2].Y, n[2].X, n[2].Y, t[2].X, t[2].Y);
            vertices[3] = new Vertex(v[3].X, v[3].Y, n[3].X, n[3].Y, t[3].X, t[3].Y);

            BufferData(instance, vertices);
        }

        public override void Render(double time, Camera camera)
        {
            foreach (var instance in Instances.Values)
            {
                if(!instance.CanTransform) continue;

                var model = instance.GetTransform();

                Shader.SetUniformMatrix4("model_matrix", ref model);
                Shader.SetUniformMatrix4("projection_matrix", ref camera.ProjectionMatrix);
                Shader.SetUniformMatrix4("view_matrix", ref camera.ViewMatrix);

                GL.DrawArrays(BeginMode.TriangleStrip, instance.Offset, instance.Length);    
            }          
        }

        public static SpriteBuffer CreateTestObject()
        {
            var tex = Textures.Instance.Items[Data.Images.TerrainDebug];
            var atlas = new TextureAtlas(4, 4, tex, 64);

            var shader = Shaders.Instance.Items[Data.Shaders.SpriteName];

            var buffer = new SpriteBuffer(shader, atlas);
            buffer.Initialize();

            return buffer;
        }
    }
}