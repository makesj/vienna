using System;
using System.Collections.Generic;
using Vienna.Audio;
using Vienna.Resources;

namespace Vienna.Audio
{
    public abstract class Audio : IAudio
    {
        public Audio()
        {
            AllPaused = false;
            Initialized = false;
            AllSamples = new List<IAudioBuffer>();
        }

        ~Audio()
        {
            Shutdown();
        }


        static bool HasSoundCard()
        {
            return true;
        }

        protected IList<IAudioBuffer> AllSamples;
        protected bool AllPaused;
        protected bool Initialized;

        bool IsPaused()
        {
            return AllPaused;
        }

        #region IAudio
        public abstract IAudioBuffer InitAudioBuffer(Resource handle);

        public abstract void ReleaseAudioBuffer(IAudioBuffer audioBuffer);

        public abstract bool Initialize(object hWnd);

        public void StopAllSounds()
        {
            foreach (var audioBuffer in AllSamples)
            {
                audioBuffer.Stop();
            }
            AllPaused = false;
        }

        public void PauseAllSounds()
        {
            foreach (var audioBuffer in AllSamples)
            {
                audioBuffer.Pause();
            }
            AllPaused = true;
        }

        public void ResumeAllSounds()
        {
            foreach (var audioBuffer in AllSamples)
            {
                audioBuffer.Resume();
            }
            AllPaused = false;
        }

        public void Shutdown()
        {
            for (int i = 0, j = AllSamples.Count; i < j; i++)
            {
                var buffer = AllSamples[i];
                buffer.Stop();
                AllSamples.RemoveAt(0);
            }

        }
        #endregion IAudio
    }
}