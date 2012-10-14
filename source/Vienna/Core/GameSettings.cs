using System.IO;
using System.Text;
using Nini.Config;

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

        private readonly IniConfigSource _source;

        protected GameSettings()
        {
            _source = new IniConfigSource(WorkingDirectory + @"\Assets\config.ini");
        }

        public string Get(string section, string name)
        {
            return _source.Configs[section].Get(name);
        }

        public string[] GetDelimited(string section, string name)
        {
            return Get(section, name).Split(',');
        }

        public int GetInt(string section, string name)
        {
            return _source.Configs[section].GetInt(name);
        }

        public float GetFloat(string section, string name)
        {
            return _source.Configs[section].GetFloat(name);
        }

        public int[] GetDelimitedInt(string section, string name)
        {
            var items = GetDelimited(section, name);
            var result = new int[items.Length];
            for (var i = 0; i < items.Length; i++)
            {
                result[i] = int.Parse(items[i]);
            }
            return result;
        }

        public bool GetBool(string section, string name)
        {
            return _source.Configs[section].GetBoolean(name);
        }

        public void Set(string section, string name, object value)
        {
            _source.Configs[section].Set(name, value);          
        }

        public void Set(string section, string name, object[] values)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < values.Length; i++)
            {
                sb.Append(values[i]).Append(",");
            }
            _source.Configs[section].Set(name, sb.ToString(0, sb.Length - 1));
        }

        public void Save()
        {
            _source.Save();
        }

        private string _workingDirectory;
        public string WorkingDirectory
        {
            get
            {
                if (_workingDirectory == null)
                {
                    var assemblyLocation = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    _workingDirectory = Path.GetDirectoryName(assemblyLocation);
                }
                return _workingDirectory;
            }

        }
    }
}
