using System;
using System.Collections.Generic;
using Vienna.Audio;
using Vienna.Resources;

public abstract class Audio : IAudio
{
    public Audio()
    {
        AllPaused = false;
        Initialied = false;
    }

    ~Audio()
    {
        Shutdown();
    }


    static bool HasSoundCard()
    {
        return true;
    }

    protected IList<IAudioBuffer> AudioBufferList;
    protected bool AllPaused;
    protected bool Initialied;

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
        foreach (var audioBuffer in AudioBufferList)
        {
            audioBuffer.Stop();
        }
        AllPaused = false;
    }

    public void PauseAllSounds()
    {
        foreach (var audioBuffer in AudioBufferList)
        {
            audioBuffer.Pause();
        }
        AllPaused = true;
    }

    public void ResumeAllSounds()
    {
        foreach (var audioBuffer in AudioBufferList)
        {
            audioBuffer.Resume();
        }
        AllPaused = false;
    }

    public void Shutdown()
    {
        for (int i = 0, j = AudioBufferList.Count; i < j; i++)
        {
            var buffer = AudioBufferList[i];
            buffer.Stop();
            AudioBufferList.RemoveAt(0);
        }

    }
    #endregion IAudio
}