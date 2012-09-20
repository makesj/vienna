
using System.Reflection;

namespace Vienna.SharpScript
{
    public class SharpScriptManager
    {
        /// <summary>
        /// Thread safe static initializer
        /// </summary>
        private class Nested
        {
            internal static readonly SharpScriptManager SharpScriptManager = new SharpScriptManager();
        }

        /// <summary>
        /// Gets the instance. 
        /// </summary>
        public static SharpScriptManager Instance
        {
            get { return Nested.SharpScriptManager; }
        }

        private const string GlobalContext = "__Global";
        private readonly SharpScriptEnvironment _environment;

        protected SharpScriptManager()
        {
            _environment = new SharpScriptEnvironment();
            _environment.CreateContext(GlobalContext);
        }

        public void AddReference(Assembly assembly)
        {
            _environment.AddReference(GlobalContext, assembly);
        }

        public void Compile(string script)
        {
            _environment.Compile(GlobalContext, script);
        }

        public void Compile(string context, string script)
        {
            _environment.Compile(context, script);
        }

        public void CreateContext(string name)
        {
            _environment.CreateContext(name);
        }

        public void UnloadContext(string context)
        {
            _environment.UnloadContext(context);
        }

        public void GetInstance(string type, params object[] args)
        {
            _environment.GetInstance(GlobalContext, type, args);
        }

    }
}
