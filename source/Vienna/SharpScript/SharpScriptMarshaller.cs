﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace Vienna.SharpScript
{
    public class SharpScriptMarshaller : MarshalByRefObject, IDisposable
    {
        private SharpScriptCompiler _compiler = new SharpScriptCompiler();
        private readonly List<Assembly> _assemblies = new List<Assembly>();
        private readonly Dictionary<string, Type> _types = new Dictionary<string, Type>();

        public string AppDomainName
        {
            get { return AppDomain.CurrentDomain.FriendlyName; }
        }

        public void Compile(string script)
        {
            var assembly = _compiler.Compile(script);
            _assemblies.Add(assembly);

            foreach (var type in assembly.GetTypes())
            {
                _types.Add(type.FullName, type);
            }
        }

        public ScriptProxy Activate(string typeName, object[] args)
        {
            if (!_types.ContainsKey(typeName))
            {
                throw new Exception("Unknown type " + typeName);
            }

            var instance = Activator.CreateInstance(_types[typeName], args);
            return new ScriptProxy(instance);
        }

        public void Dispose()
        {
            _assemblies.Clear();
            _types.Clear();
            _compiler = null;
        }
    }
}