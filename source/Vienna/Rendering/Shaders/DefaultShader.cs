using OpenTK.Graphics.OpenGL;

namespace Vienna.Rendering.Shaders
{
    public class DefaultShader : Shader
    {
        public DefaultShader() : 
            base("Vertex", Data.Shaders.TileVert, Data.Shaders.TileFrag)
        {
        }

        public override VertexAttribute[] GetAttributes(int bufferHandle)
        {
            var attributes = new VertexAttribute[2];

            var in_position = new VertexAttribute();
            in_position.VertexBuffer = bufferHandle;
            in_position.Size = 2;
            in_position.PointerType = VertexAttribPointerType.Float;
            in_position.Normalized = false;
            in_position.Stride = Vertex.SizeInBytes;
            in_position.Offset = Vertex.PositionOffset;
            in_position.ShaderHandle = Handle;
            in_position.ShaderAttribName = "in_position";
            attributes[0] = in_position;

            var in_texcoord = new VertexAttribute();
            in_texcoord.VertexBuffer = bufferHandle;
            in_texcoord.Size = 2;
            in_texcoord.PointerType = VertexAttribPointerType.Float;
            in_texcoord.Stride = Vertex.SizeInBytes;
            in_texcoord.Offset = Vertex.TextureOffset;
            in_texcoord.Normalized = false;
            in_texcoord.ShaderHandle = Handle;
            in_texcoord.ShaderAttribName = "in_texcoord";
            attributes[1] = in_texcoord;

            return attributes;
        }
    }
}
