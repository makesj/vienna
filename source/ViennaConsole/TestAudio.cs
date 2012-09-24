using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vienna.Resources;
using Vienna.Audio;
using Vienna.Processes;
using Vienna.Eventing.Events;
using Vienna.Eventing;
using System.Threading;
using Vienna;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 6)]
    public class TestAudio
    {
        const string ambientWav = @"assets\ambient.wav";
        const string geeWav = @"assets\Gee.wav";
        const string waaWav = @"assets\waa.wav";

        
        public void Execute()
        {
            GlobalAudio.Register(new OpenAlAudio()).Initialize();
            PlayingMultipleSounds();
            TestFade();            
            TestStopAllSounds();
            TestPlaySoundEvent();
        }

        private Resource CreateResourceFor(string filepath)
        {
            var memory = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(filepath));
            return new Resource(new ResourceData(1000, filepath, "wav"), memory);            
        }

        private void TestStopAllSounds()
        {
            var Manager = new ProcessManager();
            Logger.Debug("Testing TestStopAllSounds\n");
            //Currently skipping resource streams so have to reread the streams                                                 
            var waa = new SoundProcess(CreateResourceFor(waaWav), SoundType.Background, 50, true);
            Manager.AttachProcess(waa);
            Manager.UpdateProcesses(1000);
            Console.WriteLine("Press Enter to Pause All Sounds");
            Console.ReadLine();
            GlobalAudio.Instance.PauseAllSounds();
            Manager.UpdateProcesses(1000);
            Console.WriteLine("Hear anything? Hopefully not");
            Console.WriteLine("Press Enter to Play all sounds");
            Console.ReadLine();
            GlobalAudio.Instance.ResumeAllSounds();
            Manager.UpdateProcesses(1000);
            Console.WriteLine("Hear anything? Hopefully so");
            Console.WriteLine("Press Enter to Shutdown");
            Console.ReadLine();
            GlobalAudio.Instance.Shutdown();
            Manager.UpdateProcesses(1000);
            Console.WriteLine("Hear anything? Hopefully not");
        }

        private void TestFade()
        {
            var Manager = new ProcessManager();
            Logger.Debug("Testing Fade Process\n");
            var sythn = CreateResourceFor(ambientWav);
            var synth1 = new SoundProcess(sythn, SoundType.Background, 0, false);
            Manager.AttachProcess(synth1);
            var fade = new FadeProcess(synth1, 5000, 100);

            Manager.AttachProcess(fade);

            Helper.Loop(8, 1000, (delta) => Manager.UpdateProcesses(delta));
            GlobalAudio.Instance.Shutdown();
        }

        private void PlayingMultipleSounds()
        {
            var Manager = new ProcessManager();
            Logger.Debug("Testing PlayingMultipleSounds\n");
            //Currently skipping resource streams so have to reread the streams                                                 
            var synth1 = new SoundProcess(CreateResourceFor(ambientWav), SoundType.Background, 100, false);
            var synth3 = new SoundProcess(CreateResourceFor(ambientWav), SoundType.Background, 40, false);

            var delay = new DelayProcess(1000);
            delay.AttachChild(new SoundProcess(CreateResourceFor(geeWav), SoundType.Effect, 100, false));

            synth1.AttachChild(synth3);

            Manager.AttachProcess(synth1);
            Manager.AttachProcess(delay);

            Helper.Loop(12, 1000, (delta) => Manager.UpdateProcesses(delta));
            GlobalAudio.Instance.Shutdown();
        }

        private void TestPlaySoundEvent()
        {
            var Manager = new ProcessManager();
            Logger.Debug("Testing TestPlaySoundEvent\n");

            var audioEvent = new EventData_Play_Sound(waaWav);
            EventManager.Instance.AddListener<EventData_Play_Sound>(EventData_Play_Sound.Type, (eventData) =>
            {
                var synth1 = new SoundProcess(CreateResourceFor(eventData.SoundResource), SoundType.Effect, 80, false);
                Manager.AttachProcess(synth1);
            });
            Helper.Loop(2, 1000, (delta) =>
            {
                Manager.UpdateProcesses(delta);
                Console.WriteLine("Press Enter to Fire a sound");
                Console.ReadLine();
                EventManager.Instance.TriggerEvent(audioEvent);
                Manager.UpdateProcesses(delta);                                               
            });
            GlobalAudio.Instance.Shutdown();
        }
    }       
}
