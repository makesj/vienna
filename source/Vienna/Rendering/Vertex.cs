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
        public Vector2 Normal;
        public Vector2 Texture;

        public Vertex(float x, float y, float nx, float ny, float tx, float ty)
        {
            Position = new Vector2(x, y);
            Normal = new Vector2(nx, ny);
            Texture = new Vector2(tx, ty);
        }

        public static int SizeInBytes
        {
            get { return 24; } //3 * Vector2.SizeInBytes; }
        }

        public static int PositionOffset
        {
            get { return 0; }
        }

        public static int NormalOffset
        {
            get { return Vector2.SizeInBytes; }
        }

        public static int TextureOffset
        {
            get { return Vector2.SizeInBytes * 2; }
        }

        public override string ToString()
        {
            return string.Format("v{0} n{1} t{2}", Position, Normal, Texture);
        }
    }
}