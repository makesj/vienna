using System.Collections.Generic;
using Vienna.Actors;

namespace Vienna.Rendering
{
    public class SceneGraph
    {
        public IDictionary<int, Actor> Actors = new Dictionary<int, Actor>();
        protected Renderer Renderer { get; set; }
        protected ActorFactory ActorFactory { get; set; }

        public SceneGraph(Renderer renderer, ActorFactory actorFactory)
        {
            Renderer = renderer;
            ActorFactory = actorFactory;
        }

        public void Initialize()
        {
            AddActor(ActorFactory.Create("worldmap"));
            
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    AddActor(ActorFactory.Create("animatedsquare", i * 400, j * 400));    
                }                   
            }
            
        }

        public void AddActor(Actor actor)
        {
            Actors.Add(actor.Id, actor);

            if (actor.CanRender)
            {
                Renderer.BindToBuffer(actor.RenderComponent.Target, actor);
            }
        }

        public void Render(double time, Camera camera)
        {
            Renderer.Render(time, camera);
        }

        public void Update(double time)
        {
            foreach (var actor in Actors)
            {
                actor.Value.Update(time);
            }
        }

        public void Destroy()
        {
            foreach (var actor in Actors)
            {
                actor.Value.Destroy();
            }
            Actors.Clear();
        }
    }
}
