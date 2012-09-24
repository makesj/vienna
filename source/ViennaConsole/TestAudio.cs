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

        readonly ProcessManager Manager = new ProcessManager();
        public void Execute()
        {
            GlobalAudio.Register(new OpenAlAudio()).Initialize();
            PlayingMultipleSounds();
            TestFade();
            TestPlaySoundEvent();     
        }

        private Resource CreateResourceFor(string filepath)
        {
            var memory = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(filepath));
            return new Resource(new ResourceData(1000, filepath, "wav"), memory);            
        }

        private void TestFade()
        {
            Logger.Debug("Testing Fade Process");
            var sythn = CreateResourceFor(ambientWav);
            var synth1 = new SoundProcess(sythn, SoundType.Background, 0, false);
            Manager.AttachProcess(synth1);
            var fade = new FadeProcess(synth1, 5000, 50);

            Manager.AttachProcess(fade);

            Helper.Loop(8, 1000, (delta) => Manager.UpdateProcesses(delta));
        }

        private void PlayingMultipleSounds()
        {
            Logger.Debug("Testing PlayingMultipleSounds");
            //Currently skipping resource streams so have to reread the streams                                                 
            var synth1 = new SoundProcess(CreateResourceFor(ambientWav), SoundType.Background, 100, false);
            var synth3 = new SoundProcess(CreateResourceFor(ambientWav), SoundType.Background, 40, false);

            var delay = new DelayProcess(1000);
            delay.AttachChild(new SoundProcess(CreateResourceFor(geeWav), SoundType.Effect, 100, false));

            synth1.AttachChild(synth3);

            Manager.AttachProcess(synth1);
            Manager.AttachProcess(delay);

            Helper.Loop(12, 1000, (delta) => Manager.UpdateProcesses(delta));    
        }

        private void TestPlaySoundEvent()
        {
            Logger.Debug("Testing TestPlaySoundEvent");

            var audioEvent = new EventData_Play_Sound(waaWav);
            EventManager.Instance.AddListener<EventData_Play_Sound>(EventData_Play_Sound.Type, (eventData) =>
            {
                var synth1 = new SoundProcess(CreateResourceFor(eventData.SoundResource), SoundType.Effect, 80, false);
                Manager.AttachProcess(synth1);
            });
            Helper.Loop(3, 1000, (delta) =>
            {
                Console.WriteLine("Press Enter to Fire a sound");
                Console.ReadLine();
                EventManager.Instance.TriggerEvent(audioEvent);
                Manager.UpdateProcesses(delta);
                //give the sound some time to finish
                Thread.Sleep(2000);
                Manager.UpdateProcesses(delta);
            });
        }
    }       
}
