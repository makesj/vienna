using OpenTK.Graphics.OpenGL;

namespace Vienna.Rendering
{
    public struct VertexAttribute
    {
        public int VertexBuffer;
        public int ShaderHandle;
        public int Size;
        public VertexAttribPointerType PointerType;
        public BufferTarget Target;
        public bool Normalized;
        public int Stride;
        public int Offset;
        public string ShaderAttribName;
    }
}