using OpenTK;
using Vienna.Rendering;

namespace Vienna.Maps
{
    public class Tile
    {
        public int Frame;
        public int Row;
        public int Column;
        public int TileSize;
        public int Y;
        public int X;

        public Tile(int row, int col, int frame, int tileSize)
        {
            Frame = frame;
            Row = row;
            Column = col;
            TileSize = tileSize;
            X = col * TileSize;
            Y = row * TileSize;
        }

        public Vector2[] GetVertices()
        {
            return new []
            { 
                new Vector2(X, Y),//bl
                new Vector2(X, Y + TileSize),//tl
                new Vector2(X + TileSize, Y),//br
                new Vector2(X + TileSize, Y + TileSize),//tr
            };
        }

        public Vertex[] GetVertices(TextureAtlas atlas)
        {
            var v = GetVertices();
            var t = atlas.GetFrame(Frame);

            var restartStrip = Column == 0;

            var vertices = restartStrip ? new Vertex[5] : new Vertex[4];

            
            vertices[0] = new Vertex(v[0].X, v[0].Y, v[0].X, v[0].Y, t[0].X, t[0].Y);
            vertices[1] = new Vertex(v[1].X, v[1].Y, v[1].X, v[1].Y, t[1].X, t[1].Y);
            vertices[2] = new Vertex(v[2].X, v[2].Y, v[2].X, v[2].Y, t[2].X, t[2].Y);
            vertices[3] = new Vertex(v[3].X, v[3].Y, v[3].X, v[3].Y, t[3].X, t[3].Y);

            if (restartStrip)
            {
                vertices[4] = new Vertex(v[3].X, v[3].Y, v[3].X, v[3].Y, t[3].X, t[3].Y);
            }

            return vertices;
        }

        public override string ToString()
        {
            return string.Format("Row={0}, Col={1}, X={2}, Y={3}", Row, Column, X, Y);
        }
    }
}
