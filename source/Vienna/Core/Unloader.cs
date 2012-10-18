using System;

namespace Vienna.Core
{
    public class Unloader
    {
        public void Destroy(ref ViennaWindow window)
        {
            Console.WriteLine("Shutting down");

            if (window == null) return;

            if (window.Game.SceneManager != null)
            {
                window.Game.SceneManager.Destroy();
            }

            window.Dispose();
        }
    }
}
