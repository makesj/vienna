using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Vienna.AI;
using Vienna.Extensions;
using Vienna.Maps;
using Vienna.Sprites;

namespace Vienna.Actors
{
    public class ActorFactory
    {
        public const int InvalidActorId = 0;
        private static int _lastId;
        private readonly Dictionary<string, Type> _components = new Dictionary<string, Type>();

        private int NextActorId()
        {
            return ++_lastId;
        }

        public Actor Create(XDocument doc, int actorId = InvalidActorId)
        {
            var root = doc.GetRootOrThrow();
            var name = root.GetAttributeOrThrow("name");

            if (actorId == InvalidActorId) 
                actorId = NextActorId();

            var actor = new Actor(actorId, name.Value);

            foreach (var el in root.Elements())
            {
                var component = ResolveComponent(el.Name.ToString());
                component.Resolve(el);
                actor.AddComponent(component);
            }

            return actor;
        }

        public void AddComponent<T>() where T : class, IComponent
        {
            var type = typeof (T);
            _components.Add(type.Name, type);
        }

        private IComponent ResolveComponent(string name)
        {
            if (!_components.ContainsKey(name))
            {
                throw new Exception("Invalid component type: " + name);
            }
            var type = _components[name];
            return Activator.CreateInstance(type) as IComponent;
        }

        #region Deprecated

        [Obsolete("Use CreateFromXml(string, int) or Create(XElement, int)", false)]
        public Actor Create(string type, float x = 0, float y = 0)
        {
            if (type == null) throw new ArgumentNullException("type");

            switch (type)
            {
                case "animatedsquare": return AnimatedSquare(x, y);
                case "worldmap": return CreateWorldMap();
                default: throw new Exception("Unkown actor type -> " + type);
            }
        }

        private Actor AnimatedSquare(float x, float y)
        {
            var actor = new Actor(NextActorId(), "text");

            var transform = new TransformComponent();
            transform.Move(x, y);
            actor.AddComponent(transform);

            var sprite = new SpriteComponent();
            actor.AddComponent(sprite);

            var spinner = new SpinnerComponent();
            actor.AddComponent(spinner);

            return actor;
        }

        private Actor CreateWorldMap()
        {
            var actor = new Actor(NextActorId(), "worldmap");

            var map = new MapComponent();
            actor.AddComponent(map);

            return actor;
        }

        #endregion

    }
}
