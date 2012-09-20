using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Audio
{
    class DirectSoundAudioBuffer : IAudioBuffer
    {
        void IAudioBuffer.Get()
        {
            throw new NotImplementedException();
        }

        Resources.Resource IAudioBuffer.GetResource()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.OnRestore()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.Play(int volume, bool looping)
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.Pause()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.Stop()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.Resume()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.TogglePause()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.IsPlaying()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.IsLooping()
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.SetVolume(int volume)
        {
            throw new NotImplementedException();
        }

        bool IAudioBuffer.SetPosition(ulong newPosition)
        {
            throw new NotImplementedException();
        }

        int IAudioBuffer.GetVolume()
        {
            throw new NotImplementedException();
        }

        float IAudioBuffer.GetProgress()
        {
            throw new NotImplementedException();
        }
    }
}
