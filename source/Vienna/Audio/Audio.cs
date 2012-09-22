﻿using System;
using System.Collections.Generic;
using Vienna.Audio;
using Vienna.Resources;

namespace Vienna.Audio
{
    public abstract class Audio : IAudio
    {        
        protected bool _shuttingDown = false;

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
            return GlobalAudio.Instance != null;
        }

        protected IList<IAudioBuffer> AllSamples;
        protected bool AllPaused;
        protected bool Initialized;

        bool IsPaused()
        {
            return AllPaused;
        }

        #region IAudio

        public abstract bool Active();

        public abstract IAudioBuffer InitAudioBuffer(Resource handle);

        public abstract void ReleaseAudioBuffer(IAudioBuffer audioBuffer);

        public abstract bool Initialize();

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
            if (!_shuttingDown)
            {
                _shuttingDown = true;                
                for (int i = 0, j = AllSamples.Count; i < j; i++)
                {
                    var buffer = AllSamples[0];
                    buffer.Stop();
                    AllSamples.RemoveAt(0);
                }
            }
            

        }
        #endregion IAudio
    }

    public static class GlobalAudio
    {
        private static IAudio _instance;
        public static IAudio Instance {get {return _instance;}}
        public static IAudio Register(IAudio audio)
        {
            _instance = audio;
            return Instance;
        }
        
    }
}