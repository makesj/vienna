using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vienna.Actors
{
    public class AudioComponent : IComponent
    {
        protected bool loop { get; set; }
        protected int volume { get; set; }
        protected long FadeTime { get; set; }
        protected string audioResource{get;set;}
        public Actor Owner { get; set; }

        public string Id
        {
            get { return "audio"; }
        }

        public void Init()
        {
            
        }

        public void PostInit()
        {
            
        }

        public void Update(int delta)
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
