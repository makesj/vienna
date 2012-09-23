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
            GlobalAudio.Register(new OpenAlAudio()).Initialize();
            var manager = new ProcessManager();
            //Currently skipping resource streams so have to reread the streams             
            const string soundPath = @"assets\ambient.wav";
            const string sound2Path = @"assets\Gee.wav";
            var memoryStream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(soundPath));
            var memory2Stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(soundPath));
            var memory3Stream = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(sound2Path));
            
            var data = new ResourceData(1000, soundPath, "wav");
            var sythn = new Resource(data, memoryStream);
            var synth1 = new SoundProcess(new Resource(data, memoryStream), SoundType.Background, 100, false);
            var synth3 = new SoundProcess(new Resource(data, memory2Stream),SoundType.Background, 40, false);
            
            var delay = new DelayProcess(1000);
            delay.AttachChild(new SoundProcess(new Resource(data, memory3Stream), SoundType.Effect, 100, false));            
            
            synth1.AttachChild(synth3);            

            manager.AttachProcess(synth1);
            manager.AttachProcess(delay);

            Helper.Loop(12, 1000, (delta) => manager.UpdateProcesses(delta));                        
        }
    }
    
}
