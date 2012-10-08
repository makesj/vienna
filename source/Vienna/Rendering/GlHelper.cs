using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace Vienna.Rendering
{
    public class GlHelper
    {
        public static int CreateBuffer(float[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            var buffer = GenBuffer(target);
            var size = data.Length*sizeof (float);
            GL.BufferData(target, new IntPtr(size), data, hint);
            return buffer;
        }

        public static int CreateBuffer(Vector2[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            var buffer = GenBuffer(target);
            var size = data.Length * Vector2.SizeInBytes;
            GL.BufferData(target, new IntPtr(size), data, hint);
            return buffer;
        }

        public static int CreateBuffer(int[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            var buffer = GenBuffer(target);
            var size = data.Length * sizeof(int);
            GL.BufferData(target, new IntPtr(size), data, hint);
            return buffer;
        }

        public static int CreateBuffer(byte[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            var buffer = GenBuffer(target);
            var size = data.Length * sizeof(int);
            GL.BufferData(target, new IntPtr(size), data, hint);
            return buffer;
        }

        public static int CreateBuffer(Vertex[] data, BufferTarget target = BufferTarget.ArrayBuffer, BufferUsageHint hint = BufferUsageHint.StaticDraw)
        {
            var buffer = GenBuffer(target);
            var size = data.Length * Vertex.SizeInBytes;
            GL.BufferData(target, new IntPtr(size), data, hint);
            return buffer;
        }

        public static void ReleaseBuffers()
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.TextureBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
        }


        public static void DeleteBuffer(int buffer)
        {
            GL.DeleteBuffer(buffer);
        }

        public static void DeleteVertexArray(int vbo)
        {
            GL.DeleteVertexArray(vbo);
        }

        private static int GenBuffer(BufferTarget target)
        {
            var bufferId = GL.GenBuffer();
            GL.BindBuffer(target, bufferId);
            return bufferId;
        }

        public static int CreateVertexArray(VertexAttribute[] attributes)
        {
            var vertexArrayId = GL.GenVertexArray();
            GL.BindVertexArray(vertexArrayId);

            for (var i = 0; i < attributes.Length; i++)
            {
                var attr = attributes[i];
                GL.EnableVertexAttribArray(i);
                GL.BindBuffer(attr.Target, attr.VertexBuffer);
                GL.VertexAttribPointer(i, attr.Size, attr.PointerType, attr.Normalized, attr.Stride, attr.Offset);
                GL.BindAttribLocation(attr.ShaderHandle, i, attr.ShaderAttribName);
            }

            GL.BindVertexArray(0);

            return vertexArrayId;
        }

        public static void DrawArrays(BeginMode mode, int vbahandle, int count, int bufferSize)
        {
            GL.BindVertexArray(vbahandle);
            GL.DrawArrays(mode, 0, bufferSize);
        }

        public static void DrawElements(BeginMode mode, int vbahandle, int count)
        {
            
        }
    }
}
