using System;
using OpenTK;
using Vienna.Eventing;

namespace Vienna.Core
{
    public class ViennaWindow : GameWindow
    {
        public Game Game { get; protected set; }

        public ViennaWindow(WindowSettings settings, Game game)
            : base(settings.Width, settings.Height,
            settings.GraphicsMode, 
            settings.Title, 
            settings.Options,
            settings.DisplayDevice, 
            settings.GlVersionMajor, settings.GlVersionMinor,
            settings.ContextFlags)
        {
            Game = game;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            Game.Load();
            TestEvents.ExitGame += CloseWindow;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Game.Resized(Width, Height);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);
            Game.Update(e.Time);
            ProcessEvents(true);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            //PrintFps(e.Time);
            Game.Render(e.Time);
        }

        protected override void OnClosing(global::System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !Game.Closing();
            base.OnClosing(e);
        }

        protected void CloseWindow()
        {
            Exit();
        }

        private double _timer;
        private void PrintFps(double time)
        {
            if (_timer > 1)
            {
                Console.WriteLine(RenderFrequency);
                _timer = 0;
            }
            _timer += time;
        }
    }
}
