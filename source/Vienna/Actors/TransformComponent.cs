using System;
using Newtonsoft.Json.Linq;

namespace Vienna.Actors
{
    public class TransformComponent : IComponent
    {
        //================================================
        // Boilerplate properties
        //================================================

        public const string ComponentId = "transform";
        public string Id { get { return ComponentId; } }
        public Actor Owner { get; set; }

        //================================================
        // Component specific properties
        //================================================

        public float X { get; protected set; }
        public float Y { get; protected set; }

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
