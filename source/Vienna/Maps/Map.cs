using System.Collections.Generic;
using Vienna.Rendering;

namespace Vienna.Maps
{
    public class Map
    {
        public Tile[] Tiles { get; private set; }
        public int TilesPerCol { get; private set; }
        public int TilesPerRow { get; private set; }
        public int TilesSize { get; private set; }

        public Map(int tilesPerRow, int tilesPerCol, int tileSize)
        {
            TilesPerCol = tilesPerCol;
            TilesPerRow = tilesPerRow;
            TilesSize = tileSize;
            Tiles = new Tile[TilesPerRow * TilesPerCol];
        }

        public void Initialize(int numberOfFrames)
        {
            var counter = 0;
            for (var r = 0; r < TilesPerRow; r++)
            {
                for (int c = 0; c < TilesPerCol; c++)
                {
                    var frame = frames[r, c];
                    Tiles[counter] = new Tile(r, c, frame, TilesSize);
                    counter++;
                }
            }
        }

        int[,] frames = new[,]
                             {
                                 {0, 2, 2, 2, 2, 2, 2, 0},
                                 {0, 0, 0, 0, 2, 0, 0, 0},
                                 {0, 0, 0, 0, 2, 0, 0, 0},
                                 {0, 0, 0, 0, 2, 0, 0, 0},
                                 {0, 0, 0, 0, 2, 0, 0, 0},
                                 {0, 0, 0, 0, 2, 0, 0, 0},
                                 {0, 2, 0, 0, 2, 0, 0, 0},
                                 {0, 2, 2, 2, 2, 0, 0, 0},
                             };


        

        public Vertex[] GetMapVertices(TextureAtlas atlas)
        {
            var vertices = new List<Vertex>();
            for (var i = 0; i < Tiles.Length; i++)
            {
                //vertices.AddRange(Tiles[i].GetVertices(atlas));
            }
            return vertices.ToArray();
        }
    }
}
