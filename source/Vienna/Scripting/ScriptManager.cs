using System;
using System.Linq;
using IronRuby.Builtins;
using Microsoft.Scripting.Hosting;

namespace Vienna.Scripting
{
    public class ScriptManager
    {
        /// <summary>
        /// Thread safe static initializer
        /// </summary>
        private class Nested
        {
            internal static readonly ScriptManager ScriptManager = new ScriptManager();
        }

        /// <summary>
        /// Gets the instance. 
        /// </summary>
        public static ScriptManager Instance
        {
            get { return Nested.ScriptManager; }
        }

        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

        public ScriptManager()
        {
            var runtime = IronRuby.Ruby.CreateRuntime();
            _engine = runtime.GetEngine("rb");
            _scope = _engine.Runtime.CreateScope();
        }

        public void AddSearchPaths(string[] paths)
        {
            var list = _engine.GetSearchPaths().ToList();
            list.AddRange(paths);
            _engine.SetSearchPaths(paths);
        }

        public void CreateGlobalVariable(string name, object obj)
        {
            _scope.SetVariable(name, obj);         
            _engine.Execute(string.Format("${0} = {0}", name), _scope);
        }

        public void CreateVariable(string name, object obj)
        {
            _scope.SetVariable(name, obj);
        }

        public void ExecuteText(string text)
        {
            _engine.Execute(text, _scope);
        }

        public void ExecuteFile(string path)
        {
            _engine.ExecuteFile(path, _scope);
        }

        public dynamic GetInstance(string instanceName, params object[] args)
        {
            dynamic instanceVariable;
            var instanceVariableResult = _engine.Runtime.Globals.TryGetVariable(instanceName, out instanceVariable);

            if (!instanceVariableResult && instanceVariable == null)
                throw new InvalidOperationException(string.Format("Unable to find {0}", instanceName));

            return _engine.Operations.CreateInstance(instanceVariable, args);
        }

        public dynamic Invoke(RubyObject obj, string name, params object[] args)
        {
            return _engine.Operations.InvokeMember(obj, name, args);
        }

        public dynamic Invoke(string name, params object[] args)
        {
            return _engine.Operations.InvokeMember(_scope, name, args);
        }
    }
}
