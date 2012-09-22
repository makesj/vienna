using System;
using System.Collections.Generic;

namespace Vienna.SharpScript
{
    public class SharpScriptMarshaller : MarshalByRefObject, IDisposable
    {
        private SharpScriptCompiler _compiler = new SharpScriptCompiler();
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public string AppDomainName
        {
            get { return AppDomain.CurrentDomain.FriendlyName; }
        }

        public void Compile(string[] scripts)
        {
            var assembly = _compiler.Compile(scripts);

            foreach (var type in assembly.GetTypes())
            {
                _types.Add(type.FullName, type);
            }
        }

        public ScriptProxy Activate(string typeName, object[] args)
        {
            if (!_types.ContainsKey(typeName))
            {
                throw new Exception(string.Format("Unknown type '{0}'",typeName));
            }

            var instance = Activator.CreateInstance(_types[typeName], args);
            return new ScriptProxy(instance);
        }

        public void Dispose()
        {
            _types.Clear();
            _compiler = null;
        }
    }
}