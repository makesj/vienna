using Vienna.Audio;
using Vienna.Resources;

namespace Vienna.Audio
{

    public interface IAudio
    {
        bool Active();
        
        IAudioBuffer InitAudioBuffer(Resource handle);
        void ReleaseAudioBuffer(IAudioBuffer audioBuffer);

        void StopAllSounds();
        void PauseAllSounds();
        void ResumeAllSounds();

        bool Initialize();

        void Shutdown();
    }
}