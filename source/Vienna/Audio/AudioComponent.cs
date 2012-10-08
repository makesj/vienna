using System;
using Vienna.Actors;

namespace Vienna.Audio
{
    public class AudioComponent 
    {
        protected bool loop { get; set; }
        protected int volume { get; set; }
        protected long FadeTime { get; set; }
        protected string audioResource{get;set;}
        public Actor Parent { get; set; }


        public void Initialize()
        {
            
        }

        public void PostInit()
        {
            
        }

        public void Update(double time)
        {
            
        }

        public void Destroy()
        {
            
        }

        public void Changed()
        {
            
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
}
