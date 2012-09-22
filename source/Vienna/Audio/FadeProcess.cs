using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Audio
{
    public class FadeProcess : Vienna.Processes.Process
    {
        protected SoundProcess Sound { get; set; }
        protected int TotalFadeTime { get; set; }
        protected int EndVolume { get; set; }
        protected long ElapsedTime { get; set; }
        protected int StartVolume { get; set; }

        public FadeProcess(SoundProcess sound, int fadeTime, int endVolume)
        {
            Sound = sound;
            StartVolume = sound.GetVolume();
            TotalFadeTime = fadeTime;
            EndVolume = endVolume;
            ElapsedTime = 0;
            OnUpdate(ElapsedTime);
        }

        public override void OnUpdate(long delta)
        {
            ElapsedTime += delta;

            if (Sound.IsDead)
            {
                Logger.Debug("Sound is Dead");
                Succeed();
            }

            var cooef = ElapsedTime / (float)TotalFadeTime;
            if (cooef > 1f)
            {
                cooef = 1f;
            }
            else if (cooef < 0F)
            {
                cooef = 0;
            }

            int newVolume = StartVolume + (int)((EndVolume - StartVolume) * cooef);
            Logger.Debug("New Volume:{0} ElapsedTime:{1}",newVolume,ElapsedTime);
            if (ElapsedTime >= TotalFadeTime)
            {
                Logger.Debug("Succedded Elapsed:{0} TotalFade:{1}", ElapsedTime, TotalFadeTime); 
                newVolume = EndVolume;
                Succeed();
            }
            Sound.SetVolume(newVolume);
        }
    }
}
