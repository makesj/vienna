using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using Vienna.Resources;

namespace Vienna.Audio
{
    
    public class NAudioBuffer : WaveStream, IAudioBuffer
    {
        // General Sample Settings (Info)
        bool _loop;
        long _pausePosition = -1;
        bool _pauseLoop;

        // Sample WaveStream Settings
        WaveOffsetStream offsetStream;
        WaveChannel32 channelSteam;
        bool muted;
        float _volume;
        Resource _resource;
        bool _isPaused;

        public NAudioBuffer(Resource resource)
        {
            _resource = resource;
            var filename = _resource.Data.FileName;
            WaveFileReader reader = new WaveFileReader(filename);
            offsetStream = new WaveOffsetStream(reader);
            channelSteam = new WaveChannel32(offsetStream);
            muted = false;
            _volume = 1.0f;
        }

        public override int BlockAlign
        {
            get
            {
                return channelSteam.BlockAlign;
            }
        }

        public override WaveFormat WaveFormat
        {
            get { return channelSteam.WaveFormat; }
        }

        public override long Length
        {
            get { return channelSteam.Length; }
        }

        public override long Position
        {
            get
            {
                return channelSteam.Position;
            }
            set
            {
                channelSteam.Position = value;                
            }
        }

        public bool Mute
        {
            get
            {
                return muted;
            }
            set
            {
                muted = value;
                if (muted)
                {
                    channelSteam.Volume = 0.0f;
                }
                else
                {
                    // reset the volume                
                    Volume = Volume;
                }
            }
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            // Check if the stream has been set to loop
            if (_loop)
            {
                // Looping code taken from NAudio Demo
                int read = 0;
                while (read < count)
                {
                    int required = count - read;
                    int readThisTime = channelSteam.Read(buffer, offset + read, required);
                    if (readThisTime < required)
                    {
                        channelSteam.Position = 0;
                    }

                    if (channelSteam.Position >= channelSteam.Length)
                    {
                        channelSteam.Position = 0;
                    }
                    read += readThisTime;
                }
                return read;
            }
            else
            {
                // Normal read code, sample has not been set to loop
                return channelSteam.Read(buffer, offset, count);
            }
        }

        public override bool HasData(int count)
        {
            return channelSteam.HasData(count);
        }

        public float Volume
        {
            get
            {
                return _volume;
            }
            set
            {
                _volume = value;
                if (!Mute)
                {
                    channelSteam.Volume = _volume;
                }
            }
        }

        public TimeSpan PreDelay
        {
            get { return offsetStream.StartTime; }
            set { offsetStream.StartTime = value; }
        }

        public TimeSpan Offset
        {
            get { return offsetStream.SourceOffset; }
            set { offsetStream.SourceOffset = value; }
        }

        protected override void Dispose(bool disposing)
        {
            if (channelSteam != null)
            {
                channelSteam.Dispose();
            }

            base.Dispose(disposing);
        }

        void Pause()
        {
            // Store the current stream settings
            _pausePosition = Position;
            _pauseLoop = _loop;

            // Ensure the sample is temporairly not looped and set the position to the end of the stream
            _loop = false;
            Position = Length;

            // Set the loop status back, so that any further modifications of the loop status are observed
            _loop = _pauseLoop;
        }

        void Resume()
        {
            // Ensure that the sample had actuall been paused and that we are not just jumping to a random position
            if (_pausePosition >= 0)
            {
                // Set the position of the stream back to where it was paused
                Position = _pausePosition;

                // Set the pause position to negative so that we know the sample is not currently paused
                _pausePosition = -1;
            }
        }

        void SetPan(float pan)
        {
            channelSteam.Pan = pan;
        }

        #region IAudioBuffer
        object IAudioBuffer.Get()
        {
            return channelSteam;
        }

        Resource IAudioBuffer.GetResource()
        {
            return _resource;
        }

        bool IAudioBuffer.OnRestore()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.Play(int newVolume, bool looping)
        {            
            if (!GlobalAudio.Instance.Active()) return false;

            var self = (this as IAudioBuffer);            
            _isPaused = false;
            _volume = newVolume;
            _loop = looping;
            Position = 0;
            self.SetVolume(newVolume);


            return true;
        }

        bool IAudioBuffer.Pause()
        {
            if (!GlobalAudio.Instance.Active()) return false;
            Pause();
            _isPaused = true;
            return true;
        }

        bool IAudioBuffer.Stop()
        {
            if (!GlobalAudio.Instance.Active()) return false;

            if (Position < Length)
            {
                Position = Length;
            }
            
            _isPaused = true;

            return true;
        }

        bool IAudioBuffer.Resume()
        {
            if (!GlobalAudio.Instance.Active()) return false;

            Resume();
            _isPaused = false;
            return true;
        }

        bool IAudioBuffer.TogglePause()
        {
            if (!GlobalAudio.Instance.Active()) return false;

            var self = (this as IAudioBuffer);

            if (self.IsPlaying())
            {
                return self.Pause();
            }
            else
            {
                return self.Resume();
            }             
        }

        bool IAudioBuffer.IsPlaying()
        {
            if (!GlobalAudio.Instance.Active()) return false;
            return !_isPaused 
                && (Position <= Length && !_loop)
                || (!_isPaused && _loop);
        }

        bool IAudioBuffer.IsLooping()
        {
            return _loop;
        }

        void IAudioBuffer.SetVolume(int volume)
        {
            Volume = volume / 100f;
        }

        void IAudioBuffer.SetPosition(long newPosition)
        {
            Position = newPosition;
        }

        int IAudioBuffer.GetVolume()
        {
            return (int)Volume;
        }

        float IAudioBuffer.GetProgress()
        {
            return Position / Length;
        }

        #endregion IAudioBuffer
    }
}
