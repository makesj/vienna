using System;
using Vienna.Resources;

namespace Vienna.Audio
{
    
    public class SoundProcess : Vienna.Processes.Process
    {
        protected Resource _soundResource { get; set; }
        protected IAudioBuffer _audioBuffer { get; set; }
        protected int _volume { get; set; }
        protected bool _isLooping { get; set; }
        public SoundType AudioType { get; protected set; }

        public SoundProcess(Resource resource, SoundType type, int volume, bool looping)
        {
            _soundResource = resource;
            _volume = volume;
            _isLooping = looping;
            AudioType = type;
            InitializeVolume();            
        }

        ~SoundProcess()
        {
            if (IsPlaying())
            {
                Stop();
            }

            if (_audioBuffer != null)
            {
                GlobalAudio.Instance.ReleaseAudioBuffer(_audioBuffer);
            }
        }

        protected SoundProcess()
        {
            
        }

        protected void InitializeVolume()
        {
            //TODO: add in initialize Volume code to tie in with GUI
        }

        public int GetLengthMilli()
        {
           //TODO add in length when when have a SoundResource
            throw new NotImplementedException();
        }

        public override void OnInit()
        {
            base.OnInit();

            if (_soundResource == null)
                return;

            var buffer = GlobalAudio.Instance.InitAudioBuffer(_soundResource);
            if (buffer == null)
            {
                Fail();
                return;
            }
            _audioBuffer = buffer;
            Play(_volume, _isLooping);
        }

        public override void OnUpdate(long delta)
        {
            if (!IsPlaying())
            {
                Logger.Debug("SoundProcess onUpdate succedding");
                Succeed();
            }            
        }

        public bool IsPlaying()
        {
            if (_soundResource == null || _audioBuffer == null)
            {
                return false;
            }

            return _audioBuffer.IsPlaying();
        }

        public void SetVolume(int volume)
        {            
            if (_audioBuffer == null)
            {
                return;
            }

            if (volume < 0 || volume > 100)
            {
                throw new InvalidOperationException(string.Format("Volume must between 0 and 100 was:{0}", volume));
            }
            Logger.Debug("Setting volume:{0}", volume);
            _audioBuffer.SetVolume(volume);
        }

        public int GetVolume()
        {
            if (_audioBuffer == null)
            {
                return 0;
            }

            _volume = _audioBuffer.GetVolume();
            return _volume;
        }

        public void TogglePause()
        {
            if (_audioBuffer != null)
            {
                _audioBuffer.TogglePause();
            }
        }


        public void Play(int volume, bool looping)
        {
            if (volume < 0 || volume > 100)
            {
                throw new InvalidOperationException(string.Format("Volume must between 0 and 100 was:{0}", volume));
            }

            if (_audioBuffer == null)
            {
                return;
            }

            _audioBuffer.Play(volume, looping);
        }

        public void Stop()
        {
            if (_audioBuffer != null)
            {
                Logger.Debug("SoundProcess Stop");
                _audioBuffer.Stop();
            }
        }


        public float GetProgress()
        {
            if (_audioBuffer == null)
            {
                return 0;
            }

            return _audioBuffer.GetProgress();
        }


        public bool IsSoundValid()
        {
            return _soundResource != null;
        }
        

        public bool IsLooping()
        {
            return _audioBuffer != null && _audioBuffer.IsLooping(); 
        }       
    }
}
