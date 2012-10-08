using System;

namespace Vienna.Messaging
{
    public static class Events
    {
        public static Action<float, float> CameraMove;
        public static Action<float> CameraRotate;
        public static Action<float> CameraZoom;

        public static Action ExitGame;
    }
}
