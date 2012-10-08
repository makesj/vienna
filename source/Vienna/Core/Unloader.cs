using System;

namespace Vienna.Core
{
    public class Unloader
    {
        public void Destroy(ref ViennaWindow window)
        {
            Console.WriteLine("Shutting down");

            if (window == null) return;

            if (window.Game.SceneGraph != null)
            {
                window.Game.SceneGraph.Destroy();
            }

            window.Dispose();
        }
    }
}
