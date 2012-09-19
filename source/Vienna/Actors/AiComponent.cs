using System;
using Newtonsoft.Json.Linq;

namespace Vienna.Actors
{
    public class AiComponent : IComponent
    {
        //================================================
        // Boilerplate properties
        //================================================

        public const string ComponentId = "ai";
        public string Id { get { return ComponentId; } }
        public Actor Owner { get; set; }

        //================================================
        // Component specific properties
        //================================================

        

        //================================================
        // Transform implementation methods
        //================================================

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
