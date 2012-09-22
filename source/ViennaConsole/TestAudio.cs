using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Resources;
using Vienna.Audio;
using Vienna.Processes;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 6)]
    public class TestAudio
    {
        public void Execute()
        {
            GlobalAudio.Register(new NAudioSound(new NAudio.Wave.WaveOut()));

            GlobalAudio.Instance.Initialize();
            var manager = new ProcessManager();          
            //Currently skipping resource streams 
            var sythn = new Resource(new ResourceData(1000,@"Assets\ambient.wav","wav"),null);
            var synth1 = new SoundProcess(sythn, 100, false);
            var synth2 = new SoundProcess(sythn, 50, false);
            var synth3 = new SoundProcess(sythn, 15, false);
            synth1.AttachChild(synth2);
            synth2.AttachChild(synth3);
                       
            manager.AttachProcess(synth1);

            Helper.Loop(30, 1000, (delta) => manager.UpdateProcesses(delta));
        }
    }
    
}
