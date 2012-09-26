using System;
namespace Vienna.Audio
{
    public static class GlobalAudio
    {
        private static IAudio _instance;
        public static IAudio Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("An Instance of IAudio has not been registered");
                }
                return _instance;
            }
        }
        public static IAudio Register(IAudio audio)
        {
            _instance = audio;
            return Instance;
        }

        public static void UnRegister()
        {
            _instance.Shutdown();
            _instance = null;
        }

    }
}
