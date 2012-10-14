using System.Drawing;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using OpenTK.Graphics.OpenGL;
using Vienna.Actors;
using Vienna.AI;
using Vienna.Eventing;
using Vienna.Rendering.Shaders;

namespace Vienna.Rendering
{
    public class SceneManager
    {
        protected ActorRepository Repository { get; set; }
        protected ActorFactory ActorFactory { get; set; }
        protected SpriteBatch SpriteBatch { get; set; }

        public SceneManager(ActorFactory actorFactory)
        {
            Repository = new ActorRepository();
            ActorFactory = actorFactory;
            TestEvents.DestroyActor += DestroyActor;

            GL.ClearColor(Color.Black);
        }

        public void Initialize()
        {

            ActorFactory.AddComponent<TransformComponent>();
            ActorFactory.AddComponent<SpriteComponent>();
            ActorFactory.AddComponent<SpinnerComponent>();

            var xml = File.ReadAllText(Data.Actors.AnimatedSquare);
            var doc = XDocument.Parse(xml);

            // create a bunch of Animated Squares
            for (int i = 0; i < 75; i++)
            {
                for (int j = 0; j < 75; j++)
                {
                    var actor = ActorFactory.Create(doc);
                    actor.TransformComponent.Move(i * 400, j * 400);
                    Repository.Add(actor);    
                }                   
            }

            SpriteBatch = new SpriteBatch(6000, new DefaultShader());

            var tex = new Texture();
            tex.Load(Data.Images.TerrainDebug);
            _atlas = new TextureAtlas(4, 4, tex, 64);
        }

        private TextureAtlas _atlas;

        

        public void Render(double time, Camera camera)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            SpriteBatch.Begin(_atlas, camera);

            foreach (var actor in Repository.Get())
            {
                SpriteBatch.Draw(actor);
            }
            
            SpriteBatch.End();
            
            OpenTK.Graphics.GraphicsContext.CurrentContext.SwapBuffers();            
        }

        public void Update(double time)
        {
            foreach (var actor in Repository.Get())
            {
                actor.Update(time);
            }
        }

        public void DestroyActor()
        {
            var actor = Repository.Get().FirstOrDefault();
            if(actor == null) return;
            Repository.Remove(actor);
        }

        public void Destroy()
        {
            Repository.Destory();
        }
    }
}
