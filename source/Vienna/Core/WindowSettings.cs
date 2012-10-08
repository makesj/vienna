using OpenTK;
using OpenTK.Graphics;

namespace Vienna.Core
{
    public class WindowSettings
    {
        public int Width { get; set; }

        public int Height { get; set; }
        
        public string Title { get; set; }

        public GraphicsMode GraphicsMode { get; set; }

        public GameWindowFlags Options { get; set; }

        public DisplayDevice DisplayDevice { get; set; }

        public int GlVersionMajor { get; set; }

        public int GlVersionMinor { get; set; }

        public GraphicsContextFlags ContextFlags { get; set; }
    }
}