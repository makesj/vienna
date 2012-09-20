using Vienna.Audio;
using Vienna.Resources;
public interface IAudio
{
    IAudioBuffer InitAudioBuffer(Resource handle);
    void ReleaseAudioBuffer(IAudioBuffer audioBuffer);

    void StopAllSounds();
    void PauseAllSounds();
    void ResumeAllSounds();

    bool Initialize(object hWnd);

    void Shutdown();
}