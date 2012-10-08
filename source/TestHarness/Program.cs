using System;
using Vienna.Core;

namespace TestHarness
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            using (var game = new Runtime())
            {
                game.Initialize();
                game.Run();
                game.Shutdown();
            }
        }
    }
}
