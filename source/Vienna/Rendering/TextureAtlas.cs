using System.Drawing;
using OpenTK;

namespace Vienna.Rendering
{
    public class TextureAtlas
    {
        protected Vector2[,] Mappings { get; set; }
        public int TilesPerCol { get; protected set; }
        public int TilesPerRow { get; protected set; }
        public Texture Texture { get; protected set; }
        public int FrameCount { get; protected set; }

        public TextureAtlas(int tilesPerCol, int tilesPerRow, Texture texture, Rectangle[] frames)
        {
            TilesPerCol = tilesPerCol;
            TilesPerRow = tilesPerRow;
            FrameCount = tilesPerRow*tilesPerCol;
            Texture = texture;
            CreateUvMappings(frames);
        }

        public TextureAtlas(int tilesPerCol, int tilesPerRow, Texture texture, int tileSize) 
            : this(tilesPerCol, tilesPerRow, texture,
                CreateFrames(tilesPerCol, tilesPerRow, tileSize))
        {
        }

        private static Rectangle[] CreateFrames(int tilesPerCol, int tilesPerRow, int tileSize)
        {
            var frames = new Rectangle[tilesPerCol * tilesPerRow];
            var count = 0;
            for (var r = 0; r < tilesPerRow; r++)
            {
                for (var c = 0; c < tilesPerCol; c++)
                {
                    frames[count] = new Rectangle(tileSize * c, tileSize * r, tileSize, tileSize);
                    count++;
                }
            }
            return frames;
        }

        public Vector2[] GetFrame(int frame)
        {
            if (frame >= FrameCount) frame = 0;

            return new[]
            {
                Mappings[frame, 0], 
                Mappings[frame, 1], 
                Mappings[frame, 2], 
                Mappings[frame, 3]
            };
        }

        private void CreateUvMappings(Rectangle[] frames)
        {
            Mappings = new Vector2[frames.Length, 4];

            var width = Texture.Width * 1.0f;
            var height = Texture.Height * 1.0f;

            // convert the tex coords to uv
            for (var i = 0; i < frames.Length; i++)
            {
                var u = frames[i].X / width;
                var v = frames[i].Y / height;
                var s = frames[i].Width / width;
                var t = frames[i].Height / height;

                Mappings[i, 0] = new Vector2(u, v);
                Mappings[i, 1] = new Vector2(u, v + t);
                Mappings[i, 2] = new Vector2(u + s, v);
                Mappings[i, 3] = new Vector2(u + s, v + t);
            }
        }

        public void Bind()
        {
            Texture.Bind();
        }

        public override string ToString()
        {
            return string.Format("{0}x{1} {2}", TilesPerCol, TilesPerRow, Texture.Name);
        }
    }
}
