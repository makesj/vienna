using System.Collections.Generic;

namespace Vienna.Rendering
{
    public class Textures
    {
        /// <summary>
        /// Thread safe static initializer
        /// </summary>
        private class Nested
        {
            internal static readonly Textures Textures = new Textures();
        }

        /// <summary>
        /// Gets the instance. 
        /// </summary>
        public static Textures Instance
        {
            get { return Nested.Textures; }
        }


        public Dictionary<string, Texture> Items { get; protected set; }
        private string _current;

        protected Texture Current
        {
            get { return Items[_current]; }
        }

        public Textures()
        {
            Items = new Dictionary<string, Texture>();
        }

        public void Initialize()
        {
            var texture1 = new Texture();
            texture1.Load(Data.Images.Bomb);
            Items.Add(texture1.Name, texture1);

            var texture2 = new Texture();
            texture2.Load(Data.Images.Terrain);
            Items.Add(texture2.Name, texture2);

            var texture3 = new Texture();
            texture3.Load(Data.Images.Tile1);
            Items.Add(texture3.Name, texture3);

            var texture4 = new Texture();
            texture4.Load(Data.Images.TerrainDebug);
            Items.Add(texture4.Name, texture4);

            var texture5 = new Texture();
            texture5.Load(Data.Images.CartoonTiles);
            Items.Add(texture5.Name, texture5);
        }

        public Texture Activate(string name)
        {
            if(_current == name) return Current;
            var texture = Items[name];
            texture.Bind();
            _current = texture.Name;
            return texture;
        }
    }
}
