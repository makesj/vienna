using System;
using OpenTK;
using OpenTK.Graphics;
using Vienna.Actors;
using Vienna.Input;
using Vienna.Rendering;
using Vienna.Audio;
using Vienna.Physics;

namespace Vienna.Core
{
    public class Bootstrap
    {
        public void Initialize(out ViennaWindow window)
        {
            Console.WriteLine("Vienna Sandbox Test");
            Console.WriteLine("Initializing...");

            GameSettings.Current.Initialize();

            var game = new Game();

            //=======================================================
            //  Window
            //=======================================================

            Console.WriteLine("Creating host window");

            var title = GameSettings.Current.Get("wintitle");
            var gldebug = GameSettings.Current.GetBool("gldebug");
            var resolution = GameSettings.Current.GetManyInt("resolution");

            Console.WriteLine("window mode [{0}x{1}]", resolution[0], resolution[1]);

            var ws = new WindowSettings();
            ws.Width = resolution[0];
            ws.Height = resolution[1];
            ws.Title = title;
            ws.GraphicsMode = GraphicsMode.Default;
            ws.Options = 0;
            ws.DisplayDevice = DisplayDevice.Default;
            ws.GlVersionMajor = 3;
            ws.GlVersionMinor = 0;

            if (gldebug)
            {
                Console.WriteLine("gldebug is enabled");
                ws.ContextFlags = GraphicsContextFlags.ForwardCompatible | GraphicsContextFlags.Debug;
            }
            else
            {
                ws.ContextFlags = GraphicsContextFlags.ForwardCompatible;
            }

            window = new ViennaWindow(ws, game);

            //=======================================================
            //  Audio
            //=======================================================

            Console.WriteLine("Creating audio");
            GlobalAudio.Register(new Audio.OpenAlAudio())
                .Initialize();

            //=======================================================
            //  physics
            //=======================================================

            Console.WriteLine("Creating physics");
            GlobalPhysics.Register(new BulletGamePhysics())
                .Initialize();
            GlobalPhysics.Instance.RenderDiagnostics();

            //=======================================================
            //  Rendering System
            //=======================================================

            Console.WriteLine("Creating rendering system");

            Textures.Instance.Initialize();

            Shaders.Instance.Initialize();

            var renderer = new Renderer();
            renderer.Initialize(resolution[0], resolution[1]);

            //=======================================================
            //  Input System
            //=======================================================

            Console.WriteLine("Creating input manager");

            game.InputManager = new InputManager();

            //=======================================================
            //  Camera System
            //=======================================================

            Console.WriteLine("Creating camera");

            game.Camera = new Camera();
            game.Camera.SetViewport(resolution[0], resolution[1]);

            //=======================================================
            //  Scene Graph
            //=======================================================

            Console.WriteLine("Creating scene");

            game.SceneGraph = new SceneGraph(renderer, new ActorFactory());
            game.SceneGraph.Initialize();

           
        }
    }
}
