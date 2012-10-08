using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Vienna.Actors2
{
    public partial class Actor
    {
        public class Factory
        {
            protected readonly Dictionary<string, Type> ComponentTypes = new Dictionary<string, Type>();
            private static int _lastActorId;

            public Factory()
            {
                //Register built-in components here
                RegisterComponent<TransformComponent>(TransformComponent.ComponentId);
                RegisterComponent<AiComponent>(AiComponent.ComponentId);
                RegisterComponent<ScriptComponent>(ScriptComponent.ComponentId);
            }

            public Actor Create(JObject json)
            {
                var actor = new Actor
                {
                    Id = ++_lastActorId
                };
                
                actor.Init(json);

                foreach (var node in json["components"])
                {
                    var property = node as JProperty;
                    var component = ResolveComponent(property.Name);
                    component.Owner = actor;
                    //component.Init(node);
                    actor.AddComponent(component);
                }

                actor.PostInit();

                return actor;
            }

            public void RegisterComponent<T>(string name) where T : class, IComponent
            {
                if (ComponentTypes.ContainsKey(name.ToLower())) return;
                var type = typeof(T);
                ComponentTypes.Add(name.ToLower(), type);
            }

            private IComponent ResolveComponent(string name)
            {
                if (!ComponentTypes.ContainsKey(name.ToLower()))
                {
                    throw new Exception(string.Format("Could not resolve component named '{0}'", name.ToLower()));
                }
                var type = ComponentTypes[name.ToLower()];
                return Activator.CreateInstance(type) as IComponent;
            }
        }        
    }
}
