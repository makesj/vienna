using System;
using OpenTK.Audio;
namespace Vienna.Audio
{
    public class OpenAlAudio : Audio
    {
        protected AudioContext _audioContext {get;set;}

        ~OpenAlAudio()
        {
            if (_audioContext != null)
            {
                _audioContext.Dispose();
            }
        }

        public override bool Active()
        {
            return _audioContext != null;
        }

        public override IAudioBuffer InitAudioBuffer(Resources.Resource handle)
        {
            var bufferId = AL.GenBuffer();
            var sourceId = AL.GenSource();

            
            var reader = new AudioReader(handle.Stream);
            //Read
            var soundData = reader.ReadToEnd();
            AL.BufferData(bufferId, soundData);
            var error = AL.GetError();
            if (error != ALError.NoError)
            {
                Logger.Error("Could not read Audio, {0}", error);
                return null;
            }
            
            var audioBuffer = new OpenAlAudioBuffer(bufferId,sourceId,soundData.Data.Length, handle);
            
            //Prepare
            AL.Source(sourceId, ALSourcei.Buffer, (int)bufferId);
            
            //Add
            AllSamples.Add(audioBuffer);
            return audioBuffer;
        }

        public override void ReleaseAudioBuffer(IAudioBuffer audioBuffer)
        {
            var openAlBuffer = audioBuffer as OpenAlAudioBuffer;
            AL.DeleteSource(openAlBuffer.SourceId);
            AL.DeleteBuffer(openAlBuffer.BufferId);                        
            AllSamples.Remove(audioBuffer);
        }

        public override bool Initialize()
        {
            try
            {
                _audioContext = new AudioContext();
                Initialized = true;
            }
            catch (Exception e)
            {
                Logger.Error("Could not initialize Audio Context", e);
                return false;
            }           

            return true;
        }
        
    }
}
