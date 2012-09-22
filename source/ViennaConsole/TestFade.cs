using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Audio;
using Vienna.Processes;
using Vienna.Resources;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 7)]
    public class TestFade
    {
        public void Execute()
        {
            GlobalAudio.Register(new NAudioSound(new NAudio.Wave.DirectSoundOut()));
            GlobalAudio.Instance.Initialize();
            var manager = new ProcessManager();
            //Currently skipping resource streams 
            var sythn = new Resource(new ResourceData(1000, @"Assets\ambient.wav", "wav"), null);
            var synth1 = new SoundProcess(sythn, 0, true);
            manager.AttachProcess(synth1);
            var fade = new FadeProcess(synth1, 5000, 50);

            manager.AttachProcess(fade);

            Helper.Loop(30, 1000, (delta) => manager.UpdateProcesses(delta));

        }
    }
}
