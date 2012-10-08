using OpenTK;

namespace Vienna.Rendering
{
    public static class Vector2Extensions
    {
        public static Vector2 Transform(this Vector2 vector, ref Matrix4 matrix)
        {
            return new Vector2((vector.X * matrix.M11) + (vector.Y * matrix.M21) + matrix.M41,
                                 (vector.X * matrix.M12) + (vector.Y * matrix.M22) + matrix.M42);
        }
    }
}
