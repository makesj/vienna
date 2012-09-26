using System;
using OpenTK.Audio;
using Vienna.Resources;
namespace Vienna.Audio
{
    public class OpenAlAudioBuffer : AudioBuffer
    {

        public Int32 BufferId { get; protected set; }
        public Int32 SourceId { get; protected set; }

        protected float _length { get; set; }
        public OpenAlAudioBuffer(Int32 bufferId, Int32 sourceId, float length, Resource resource)
            : base(resource)
        {
            BufferId = bufferId;
            SourceId = sourceId;
            _length = length;
            Logger.Debug("OpenAlAudioBuffer Bid:{0}, Sid:{1}", BufferId, SourceId);
        }

        public override object Get()
        {
            //the way OpenAl is there is only buffers and whatnot so just return this
            return this;
        }

        public override bool OnRestore()
        {
            throw new NotImplementedException();
        }

        public override bool Play(int volume, bool looping)
        {
            if (!GlobalAudio.Instance.Active()) return false;

            _isLooping = looping;
            AL.Source(SourceId, ALSourceb.Looping, looping);
            SetVolume(volume);
            AL.SourcePlay(SourceId);           

            return true;
        }

        public override bool Pause()
        {
            if (!GlobalAudio.Instance.Active()) return false;
            AL.SourcePause(SourceId);
            return true;
        }

        public override bool Stop()
        {
            if (!GlobalAudio.Instance.Active()) return false;

            AL.SourceStop(SourceId);

            return false;
        }

        public override bool Resume()
        {
            if (!GlobalAudio.Instance.Active()) return false;
            AL.SourcePlay(SourceId);
            return false;
        }

        public override bool TogglePause()
        {                        
            if (IsPlaying())
            {
                return Pause();
            }
            else
            {
                return Resume();
            }
        }

        public override bool IsPlaying()
        {
            var source = AL.GetSourceState(SourceId);
            return source == ALSourceState.Playing;
        }

        public override void SetVolume(int volume)
        {
            float newVolume = volume / 100f;
            Logger.Debug("Setting volume to :{0}", newVolume);
            AL.Source(SourceId, ALSourcef.Gain, newVolume);
        }

        public override void SetPosition(long newPosition)
        {
            Logger.Debug("SetPosition to :{0}", newPosition);
            AL.Source(SourceId, ALSourcei.ByteOffset, (int)newPosition);
        }

        public override float GetProgress()
        {
            int value = 0;
            OpenTK.Audio.AL.GetSource(SourceId, ALGetSourcei.ByteOffset, out value);
            var progress = value / _length;            
            return progress;
        }
    }
}
