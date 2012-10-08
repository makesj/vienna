using System;

namespace Vienna.Eventing
{
    public static class TestEvents
    {
        public static Action<float, float> CameraMove;
        public static Action<float> CameraRotate;
        public static Action<float> CameraZoom;

        public static Action DestroyActor;

        public static Action ExitGame;
    }
}
