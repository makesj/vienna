using System;
using System.Runtime.InteropServices;
using OpenTK;

namespace Vienna.Rendering
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vertex
    {
        public static readonly Vertex Zero;

        public Vector2 Position;
        public Vector2 Texture;

        public Vertex(float x, float y, float tx, float ty)
        {
            Position = new Vector2(x, y);
            Texture = new Vector2(tx, ty);
        }

        public static int SizeInBytes
        {
            get { return 16; } //2 * Vector2.SizeInBytes;
        }

        public static int PositionOffset
        {
            get { return 0; }
        }

        public static int TextureOffset
        {
            get { return 8; }
        }

        public override string ToString()
        {
            return string.Format("v{0} t{1}", Position, Texture);
        }
    }
}