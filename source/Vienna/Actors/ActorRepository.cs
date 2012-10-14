using System.Collections.Generic;
using System.Linq;

namespace Vienna.Actors
{
    public class ActorRepository
    {
        private readonly IDictionary<int, Actor> _items = new Dictionary<int, Actor>(500);

        public IEnumerable<Actor> Get()
        {
            foreach(var item in _items)
            {
                yield return item.Value;
            }
        }

        public void Add(Actor actor)
        {
            _items.Add(actor.Id, actor);
            actor.Initialize();
        }

        public void Remove(Actor actor)
        {
            actor.Destroy();
            _items.Remove(actor.Id);
        }

        public void Destory()
        {
            var actors = _items.Values.ToArray();
            foreach (var actor in actors)
            {
                Remove(actor);
            }
            _items.Clear();
        }
    }
}