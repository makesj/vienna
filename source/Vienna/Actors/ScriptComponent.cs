using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vienna.Actors
{
    public class ScriptComponent  : IComponent
    {
        //================================================
        // Boilerplate properties
        //================================================

        public const string ComponentId = "script";
        public string Id { get { return ComponentId; } }
        public Actor Owner { get; set; }

        //================================================
        // Component specific properties
        //================================================

        public IDictionary<string, string> ScriptEventHandlers;

        //================================================
        // Transform implementation methods
        //================================================

        public void Init()
        {
        }

        public void PostInit()
        {
            //Owner.RegisterEvent("eventName", HandleEvent);
        }

        public void HandleEvent(int x)
        {
            //var functionName = ScriptEventHandlers["event"];
            //ScriptManager.Ev
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
