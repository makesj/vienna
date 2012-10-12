using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vienna.Actors;
using Vienna.AI;
using Vienna.Eventing;
using Vienna.Sprites;

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
            TestEvents.DestroyActor += DestroyActor;
        }

        public void Initialize()
        {
            ActorFactory.AddComponent<TransformComponent>();
            ActorFactory.AddComponent<SpriteComponent>();
            ActorFactory.AddComponent<SpinnerComponent>();

            var xml = File.ReadAllText(Data.WorkingDirectory + @"\assets\actors\animated_square.xml");
            var a = ActorFactory.CreateFromXml(xml);

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
            Actors.Add(actor.Id, actor);
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
            Actors.Remove(actor.Id);
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

        public void DestroyActor()
        {
            var actor = Actors.Values.FirstOrDefault();
            if(actor == null) return;
            RemoveActor(actor);
        }

        public void Destroy()
        {
            var actors = Actors.Values.ToArray();
            foreach (var actor in actors)
            {
                RemoveActor(actor);
            }
            Actors.Clear();
        }
    }
}
