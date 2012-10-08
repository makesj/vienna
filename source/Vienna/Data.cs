using System.IO;
using OpenTK;
using Vienna.Rendering;

namespace Vienna
{
    public class Data
    {
        public class Images
        {
            public static string Donut = WorkingDirectory + @"\Assets\Textures\donut.png";
            public static string Bomb = WorkingDirectory + @"\Assets\Textures\bomb.png";
            public static string Terrain = WorkingDirectory + @"\Assets\Textures\terrain-atlas.png";
            public static string TerrainDebug = WorkingDirectory + @"\Assets\Textures\terrain-atlas-debug.png";
            public static string Tile1 = WorkingDirectory + @"\Assets\Textures\tile1.png";
            public static string Tile2 = WorkingDirectory + @"\Assets\Textures\tile2.png";
            public static string CartoonTiles = WorkingDirectory + @"\Assets\Textures\cartoon-tiles.png";
        }

        public class Shaders
        {
            public static string TileVert = WorkingDirectory + @"\Assets\Shaders\tiles.vert";
            public static string TileFrag = WorkingDirectory + @"\Assets\Shaders\tiles.frag";
            public static string TileName = "tile";
            public static string SpriteVert = WorkingDirectory + @"\Assets\Shaders\sprite.vert";
            public static string SpriteFrag = WorkingDirectory + @"\Assets\Shaders\sprite.frag";
            public static string SpriteName = "sprite";
        }

        public class Triangle2D
        {
            public static Vector2[] Position = new[]
                                           {
                                               new Vector2(0, 2),
                                               new Vector2(-2, -2),
                                               new Vector2(2, -2)
                                           };

            public static int[] Elements = new [] {0, 1, 2};
        }

        public class Quad2D
        {
            public static Vector2[] Position = new[]
                                           {
                                               new Vector2(-100, -100),
                                               new Vector2(-100, 100),
                                               new Vector2(100, -100),
                                               new Vector2(100, 100)
                                           };

            
            public static Vector2[] TextureMap = new[]
                                           {
                                               new Vector2(0, 0),
                                               new Vector2(0, 1),
                                               new Vector2(1, 0),
                                               new Vector2(1, 1)
                                           };

            public static Vertex[] Vertex = new[]
                                                {
                                                    new Vertex(-100,-100,   -100,-100,   0,0  ),
                                                    new Vertex(-100, 100,   -100, 100,   0,1  ),
                                                    new Vertex( 100,-100,    100,-100,   1,0),
                                                    new Vertex( 100, 100,    100, 100,   1,1),
                                                };
        }

        public class Cube3D
        {
            public static Vector3[] Position = new []
                                                   {
                                                       new Vector3(-1.0f, -1.0f, 1.0f),
                                                       new Vector3(1.0f, -1.0f, 1.0f),
                                                       new Vector3(1.0f, 1.0f, 1.0f),
                                                       new Vector3(-1.0f, 1.0f, 1.0f),
                                                       new Vector3(-1.0f, -1.0f, -1.0f),
                                                       new Vector3(1.0f, -1.0f, -1.0f),
                                                       new Vector3(1.0f, 1.0f, -1.0f),
                                                       new Vector3(-1.0f, 1.0f, -1.0f)
                                                   };

            public static int[] Elements = new []
                                               {
                                                   // front face
                                                   0, 1, 2, 2, 3, 0,
                                                   // top face
                                                   3, 2, 6, 6, 7, 3,
                                                   // back face
                                                   7, 6, 5, 5, 4, 7,
                                                   // left face
                                                   4, 0, 3, 3, 7, 4,
                                                   // bottom face
                                                   0, 1, 5, 5, 4, 0,
                                                   // right face
                                                   1, 5, 6, 6, 2, 1,
                                               };
        }

        private static string _working_directory;
        private static string WorkingDirectory
        {
            get
            {
                if (_working_directory == null)
                {
                    var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    _working_directory = Path.GetDirectoryName(assemblyLocation);
                }
                return _working_directory;
            }
            
        }
    }
}
