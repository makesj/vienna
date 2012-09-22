using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Resources;

namespace Vienna.Audio
{
    public interface IAudioBuffer
    {
        object Get();
        Resource GetResource();
        bool OnRestore();

        bool Play(int volume, bool looping);
        bool Pause();
        bool Stop();
        bool Resume();

        bool TogglePause();
        bool IsPlaying();
        bool IsLooping();
        void SetVolume(int volume);
        void SetPosition(long newPosition);
        int GetVolume();
        float GetProgress();
    }     
}
