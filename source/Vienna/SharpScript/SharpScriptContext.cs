using System;
using System.Collections.Generic;
using System.Linq;

namespace Vienna.SharpScript
{
    public class SharpScriptContext : IDisposable
    {
        private const string MarshallerType = "Vienna.SharpScript.SharpScriptMarshaller";
        private const string MarshallerDll = "Vienna.dll";

        private readonly Dictionary<string, AppDomainMarshaller> _contexts = new Dictionary<string, AppDomainMarshaller>();
        
        public void CreateContext(string name)
        {
            var domain = AppDomain.CreateDomain(name);
            var marshaller = (SharpScriptMarshaller)domain.CreateInstanceFromAndUnwrap(MarshallerDll, MarshallerType);
            _contexts.Add(name, new AppDomainMarshaller(domain, marshaller));
        }

        public DynamicScriptProxy Activate(string context, string type, object[] args)
        {
            if (!_contexts.ContainsKey(context))
            {
                throw new InvalidOperationException("Invalid script context " + context);
            }

            var proxy = _contexts[context].Marshaller.Activate(type, args);
            return new DynamicScriptProxy(proxy);
        }

        public void Compile(string context, string[] scripts)
        {
            if (!_contexts.ContainsKey(context))
            {
                CreateContext(context);
            }

            _contexts[context].Marshaller.Compile(scripts);
        }

        public void Unload(string context)
        {
            var c = _contexts[context];
            c.Marshaller.Dispose();
            AppDomain.Unload(c.Domain);
            c.Marshaller = null;
            c.Domain = null;
            _contexts.Remove(context);
        }

        public void UnloadAll()
        {
            foreach (var context in _contexts.Keys.ToArray())
            {
                Unload(context);
            }
        }

        public void Dispose()
        {
            UnloadAll();
        }

        class AppDomainMarshaller
        {
            public AppDomain Domain { get; set; }
            public SharpScriptMarshaller Marshaller { get; set; }

            public AppDomainMarshaller(AppDomain domain, SharpScriptMarshaller marshaller)
            {
                Domain = domain;
                Marshaller = marshaller;
            }
        }
    }
}
