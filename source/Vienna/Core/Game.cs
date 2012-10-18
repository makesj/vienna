using Vienna.Input;
using Vienna.Rendering;

namespace Vienna.Core
{
    public class Game
    {
        public InputManager InputManager { get; set; }
        public SceneManager SceneManager { get; set; }
        public Camera Camera { get; set; }

        public void Load()
        {
            // kick off game's startup script process here
        }

        public void Update(double time)
        {
            Camera.Transform();
            InputManager.HandleInput(time);
            SceneManager.Update(time);
        }

        public void Render(double time)
        {
            SceneManager.Render(time, Camera);
        }

        public void Resized(int width, int height)
        {
            Camera.SetViewport(width, height);
        }

        public bool Closing()
        {
            return true;
        }
    }
}
