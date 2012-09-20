using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vienna.SharpScript
{
    public class SharpScriptEnvironment
    {
        private readonly IDictionary<string, SharpScriptContext> _contextCollection = new Dictionary<string, SharpScriptContext>();

        public void CreateContext(string name)
        {
            if (_contextCollection.ContainsKey(name))
            {
                throw new InvalidOperationException(string.Format("Environment already contains a script context named '{0}'", name));
            }

            var context = new SharpScriptContext(name);
            _contextCollection.Add(name, context);
        }

        public void UnloadContext(string name)
        {
            _contextCollection[name].Unload();
            _contextCollection.Remove(name);
        }

        public void AddReference(string context, Assembly assembly)
        {
            _contextCollection[context].AddReference(assembly);
        }

        public void Compile(string context, string script)
        {
            _contextCollection[context].Compile(script);
        }

        public void GetInstance(string context, string type, params object[] args)
        {
            _contextCollection[context].GetInstance(type, args);
        }
    }
}
