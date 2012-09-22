using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NAudio.Wave;
using System.IO;
using Vienna.Resources;

namespace Vienna.Audio
{
    public class NAudioSound : Audio
    {
        protected IWavePlayer WaveOutDevice;
        protected WaveMixerStream32 Mixer;

        public NAudioSound(IWavePlayer wavePlayer)
            : base()
        {
            WaveOutDevice = wavePlayer;
        }
        
        ~NAudioSound()
        {
            Shutdown();
        }

        public override bool Active()
        {
            return WaveOutDevice != null;
        }
        
        public override IAudioBuffer InitAudioBuffer(Resources.Resource handle)
        {
            var filename = handle.Data.FileName;
            var extension = filename.Substring(filename.LastIndexOf('.'));

            NAudioBuffer buffer;
            switch (extension)
            {
                case ".mp3":
                    buffer = SetMp3Stream(handle);
                    break;
                case ".wav":
                    buffer = SetWaveStream(handle);
                    break;
                default:
                    throw new InvalidOperationException(string.Format("Invalid File Format:{0}", filename));
            }

            AllSamples.Add(buffer);            
            Mixer.AddInputStream(buffer);
            (buffer as IAudioBuffer).Stop();
            return buffer;
        }

        public override bool Initialize()
        {
            if (Initialized) return true;
            Initialized = false;

            AllSamples.Clear();
            
            Mixer = new WaveMixerStream32();
            Mixer.AutoStop = false;
            WaveOutDevice.Init(Mixer);
            WaveOutDevice.Play();
            Initialized = true;
            return true;
        }

        public override void ReleaseAudioBuffer(IAudioBuffer audioBuffer)
        {
            Mixer.RemoveInputStream(audioBuffer.Get() as WaveChannel32);
            AllSamples.Remove(audioBuffer);           
        }

        public new void Shutdown()
        {
            if (Initialized)
            {
                WaveOutDevice.Stop();
                base.Shutdown();
                Initialized = false;
                Mixer.Dispose();
                WaveOutDevice.Dispose();
                WaveOutDevice = null;
                Mixer = null;
            }
        }

        protected NAudioBuffer SetWaveStream(Resource fileName)
        {
            return new NAudioBuffer(fileName);
        }

        protected NAudioBuffer SetMp3Stream(Resource fileName)
        {
            throw new NotImplementedException();
        }
    }
}
