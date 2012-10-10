using System.Collections.Generic;
using System.Linq;
using Vienna.Actors;
using Vienna.Eventing;

namespace Vienna.Rendering
{
    public static class ActorList
    {
        public static IDictionary<int, Actor> Actors = new Dictionary<int, Actor>();        
    }

    public class SceneGraph
    {        
        protected Renderer Renderer { get; set; }
        protected ActorFactory ActorFactory { get; set; }

        public SceneGraph(Renderer renderer, ActorFactory actorFactory)
        {
            Renderer = renderer;
            ActorFactory = actorFactory;
            TestEvents.DestroyActor += DestroyActor;
        }

        public void Initialize()
        {
            AddActor(ActorFactory.Create("worldmap"));
            
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    AddActor(ActorFactory.Create("animatedsquare", i * 400, j * 400));    
                }                   
            }
            
        }

        public void AddActor(Actor actor)
        {
            ActorList.Actors.Add(actor.Id, actor);
            actor.Initialize();

            if (actor.CanRender)
            {
                Renderer.BindActor(actor.RenderComponent.Target, actor);
            }
        }

        public void RemoveActor(Actor actor)
        {
            if (actor.CanRender)
            {
                Renderer.UnbindActor(actor.RenderComponent.Target, actor);
            }
            actor.Destroy();
            ActorList.Actors.Remove(actor.Id);
        }

        public void Render(double time, Camera camera)
        {
            Renderer.Render(time, camera);
        }

        public void Update(double time)
        {
            foreach (var actor in ActorList.Actors)
            {
                actor.Value.Update(time);
            }
        }

        public void DestroyActor()
        {
            var actor = ActorList.Actors.Values.FirstOrDefault();
            if(actor == null) return;
            RemoveActor(actor);
        }

        public void Destroy()
        {
            var actors = ActorList.Actors.Values.ToArray();
            foreach (var actor in actors)
            {
                RemoveActor(actor);
            }
            ActorList.Actors.Clear();
        }
    }
}
