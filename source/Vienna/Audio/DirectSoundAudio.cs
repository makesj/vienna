using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Audio
{
    public class DirectSoundAudio : Audio
    {
        public override IAudioBuffer InitAudioBuffer(Resources.Resource handle)
        {
            throw new NotImplementedException();
        }

        public override bool Initialize(object hWnd)
        {
            if (Initialized) return true;
            Initialized = false;

            AllSamples.Clear();

            Initialized = true;
            return true;
        }

        public override void ReleaseAudioBuffer(IAudioBuffer audioBuffer)
        {
            throw new NotImplementedException();
        }

        public new void Shutdown()
        {
            if (Initialized)
            {
                base.Shutdown();
                Initialized = false;
            }
        }
    }
}
