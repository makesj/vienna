using OpenTK;
using OpenTK.Graphics.OpenGL;
using Vienna.Messaging;

namespace Vienna.Rendering
{
    public class Camera
    {
        public Matrix4 ProjectionMatrix;
        public Matrix4 ViewMatrix;

        public Vector3 Position { get; protected set; }
        public float ZoomFactor { get; protected set; }
        public float Rotation { get; protected set; }

        public Camera()
        {
            ZoomFactor = 1.0f;
            Rotation = 0.0f;
            Events.CameraMove += Move;
            Events.CameraZoom += Zoom;
            Events.CameraRotate += Rotate;
        }

        public void Move(float x, float y)
        {
            Position = new Vector3(x, y, 0);
        }

        public void Zoom(float zoomFactor)
        {
            ZoomFactor = zoomFactor;
        }

        public void Rotate(float rotation)
        {
            Rotation = rotation;
        }

        public void SetViewport(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
            ProjectionMatrix = Matrix4.CreateOrthographicOffCenter(0, width, height, 0, 1, -1);
            ViewMatrix = Matrix4.CreateTranslation(width * 0.5f, height * 0.5f, 0);
        }

        public void Transform()
        {
            var camera = Matrix4.CreateTranslation(Position);
            var zoom = Matrix4.Scale(ZoomFactor, ZoomFactor, 1);
            var rotation = Matrix4.CreateRotationZ(Rotation);
            ViewMatrix = camera * rotation * zoom * ViewMatrix;
        }
    }
}
