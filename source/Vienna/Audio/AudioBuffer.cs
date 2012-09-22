using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Resources;

namespace Vienna.Audio
{
    public abstract class AudioBuffer : IAudioBuffer
    {
        protected bool _isLooping;
        protected bool _isPaused;
        protected int _volume;
        protected Resource _Resource;

        protected AudioBuffer(Resource resource)
        {
            _Resource = resource;
            _isPaused = false;
            _isLooping = false;
            _volume = 0;
        }

        public abstract object Get();        

        public Resources.Resource GetResource()
        {
            return _Resource;
        }

        public abstract bool OnRestore();

        public abstract bool Play(int volume, bool looping);

        public abstract bool Pause();

        public abstract bool Stop();

        public abstract bool Resume();

        public abstract bool TogglePause();

        public abstract bool IsPlaying();
        

        public bool IsLooping()
        {
            return _isLooping;
        }

        public abstract void SetVolume(int volume);

        public abstract void SetPosition(long newPosition);

        public int GetVolume()
        {
            return _volume;
        }

        public abstract float GetProgress();
    }
}
