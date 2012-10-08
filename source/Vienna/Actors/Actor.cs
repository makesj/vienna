using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vienna.Actors
{
//test
    public partial class Actor
    {
        protected IDictionary<string, IComponent> Components { get; set; }
        public long Id { get; protected set; }
        public string ActorType { get; protected set; }

        protected Actor()
        {
            Components = new Dictionary<string, IComponent>();
        }

        protected void Init(JObject json)
        {
            ActorType = json["type"].ToString();
        }

        public void PostInit()
        {
            foreach (var component in Components)
            {
                component.Value.PostInit();
            }
        }

        public void Update(int delta)
        {
            foreach (var component in Components)
            {
                component.Value.Update(delta);
            }
        }

        public void Destroy()
        {
            Components.Clear();
        }

        public IComponent GetComponent(string id)
        {
            return Components[id];
        }

        public JObject Serialize()
        {
            throw new NotImplementedException();
        }

        private void AddComponent(IComponent component)
        {
            Components.Add(component.Id, component);
        }
    }
}
