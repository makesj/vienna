using OpenTK.Input;
using Vienna.Eventing;

namespace Vienna.Input
{
    public class InputManager
    {
        public void HandleInput()
        {
            var keyboard = Keyboard.GetState();

            const int cspeed = 20;
            const float zspeed = 0.03f;
            const float rspeed = 0.03f;

            var cx = 0;
            var cy = 0;
            var zoom = 1.0f;
            var rotation = 0.0f;

            if (keyboard[Key.Up])
                cy += cspeed;

            if (keyboard[Key.Down])
                cy -= cspeed;

            if (keyboard[Key.Right])
                cx -= cspeed;

            if (keyboard[Key.Left])
                cx += cspeed;

            if (keyboard[Key.S])
                zoom -= zspeed;

            if (keyboard[Key.W])
                zoom += zspeed;

            if (keyboard[Key.D])
                rotation += rspeed;

            if (keyboard[Key.A])
                rotation -= rspeed;

            if (TestEvents.CameraMove != null) TestEvents.CameraMove(cx, cy);
            if (TestEvents.CameraRotate != null) TestEvents.CameraRotate(rotation);
            if (TestEvents.CameraZoom != null) TestEvents.CameraZoom(zoom);


            if (keyboard[Key.Escape])
            {
                if (TestEvents.ExitGame != null) TestEvents.ExitGame();
            }
        }
    }
}
