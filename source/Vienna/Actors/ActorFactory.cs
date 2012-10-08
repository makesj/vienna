using System;
using Vienna.AI;
using Vienna.Maps;
using Vienna.Sprites;

namespace Vienna.Actors
{
    public class ActorFactory
    {
        private static int _lastId;

        private int NextActorId()
        {
            return ++_lastId;
        }

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
            transform.Move(x,y);
            actor.AddComponent(transform);

            var sprite = new SpriteComponent();
            actor.AddComponent(sprite);

            var spinner = new SpinnerComponent();
            actor.AddComponent(spinner);

            return actor;
        }

        public Actor CreateWorldMap()
        {
            var actor = new Actor(NextActorId(), "worldmap");

            var map = new MapComponent();
            actor.AddComponent(map);

            return actor;
        }


    }
}
