using System.Collections.Generic;

namespace Vienna.Rendering
{
    public class Shaders
    {
        /// <summary>
        /// Thread safe static initializer
        /// </summary>
        private class Nested
        {
            internal static readonly Shaders Shaders = new Shaders();
        }

        /// <summary>
        /// Gets the instance. 
        /// </summary>
        public static Shaders Instance
        {
            get { return Nested.Shaders; }
        }

        public Dictionary<string, Shader> Items { get; protected set; }
        private string _current;

        protected Shader Current
        {
            get { return Items[_current]; }
        }

        protected Shaders()
        {
            Items = new Dictionary<string, Shader>();
        }

        public void Initialize()
        {
            Add(new Shader(Data.Shaders.SpriteName, Data.Shaders.SpriteVert, Data.Shaders.SpriteFrag));
            Add(new Shader(Data.Shaders.TileName, Data.Shaders.TileVert, Data.Shaders.TileFrag));
        }

        public void Add(Shader shader)
        {
            shader.Initialize();
            Items.Add(shader.Name, shader);
        }

        public Shader Activate(string name)
        {
            if(_current == name) return Current;
            var shader = Items[name];
            _current = shader.Name;
            return shader.Bind();
        }

        public Shader Activate(Shader shader)
        {
            if (Current.Name == shader.Name) return shader;
            _current = shader.Name;
            shader.Bind();
            return shader;
        }
    }
}
