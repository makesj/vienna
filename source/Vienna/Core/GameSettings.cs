using System.Collections.Generic;

namespace Vienna.Core
{
    public class GameSettings
    {
        /// <summary>
        /// Thread safe static initializer
        /// </summary>
        private class Nested
        {
            internal static readonly GameSettings GameSettings = new GameSettings();
        }

        /// <summary>
        /// Gets the instance. 
        /// </summary>
        public static GameSettings Current
        {
            get { return Nested.GameSettings; }
        }

        private readonly IDictionary<string, string> _items = new Dictionary<string, string>();

        protected GameSettings()
        {
        }

        public void Initialize()
        {
            _items.Clear();
            _items.Add("resolution", "800,600");
            _items.Add("wintitle", "OpenTk Sandbox");
            _items.Add("gldebug", "1");
        }

        public string Get(string name)
        {
            return _items[name];
        }

        public string[] GetMany(string name)
        {
            return _items[name].Split(',');
        }

        public int GetInt(string name)
        {
            return int.Parse(Get(name));
        }

        public int[] GetManyInt(string name)
        {
            var items = GetMany(name);
            var result = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                result[i] = int.Parse(items[i]);
            }
            return result;
        }

        public bool GetBool(string name)
        {
            return GetInt(name) == 1;
        }
    }
}
