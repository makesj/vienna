using System;
using OpenTK;
using OpenTK.Graphics;
using Vienna.Actors;
using Vienna.Input;
using Vienna.Rendering;

namespace Vienna.Core
{
    public class Bootstrap
    {
        public void Initialize(out ViennaWindow window)
        {
            Console.WriteLine("Vienna Sandbox Test");
            Console.WriteLine("Initializing...");

            var game = new Game();

            //=======================================================
            //  Window
            //=======================================================

            Console.WriteLine("Creating host window");

            var title = GameSettings.Current.Get("Window", "title");
            var gldebug = GameSettings.Current.GetBool("Window", "gldebug");
            var resolution = GameSettings.Current.GetDelimitedInt("Window", "resolution");
            var vsync = GameSettings.Current.GetBool("Window", "enablevsync");

            Console.WriteLine("window mode [{0}x{1}]", resolution[0], resolution[1]);

            var ws = new WindowSettings();
            ws.Width = resolution[0];
            ws.Height = resolution[1];
            ws.EnableVSync = vsync;
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
