using System;

namespace Vienna.SharpScript
{
    public class SharpScriptContext : IDisposable
    {
        public AppDomain Domain { get; protected set; }
        public SharpScriptMarshaller Marshaller { get; protected set; }

        public SharpScriptContext(AppDomain domain, SharpScriptMarshaller marshaller)
        {
            Domain = domain;
            Marshaller = marshaller;
        }


        public void Dispose()
        {
            Marshaller.Dispose();
            AppDomain.Unload(Domain);
            Marshaller = null;
            Domain = null;
        }
    }
}