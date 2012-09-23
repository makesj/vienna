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
            GlobalAudio.Register(new OpenAlAudio()).Initialize();            
            var manager = new ProcessManager();
            //Currently skipping resource streams 
            const string soundPath = @"assets\ambient.wav";
            var memoryStream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(soundPath));
            var sythn = new Resource(new ResourceData(1000, soundPath, "wav"), memoryStream);
            var synth1 = new SoundProcess(sythn, SoundType.Background, 0, false);
            manager.AttachProcess(synth1);
            var fade = new FadeProcess(synth1, 5000, 50);

            manager.AttachProcess(fade);

            Helper.Loop(8, 1000, (delta) => manager.UpdateProcesses(delta));
        }
    }
}
