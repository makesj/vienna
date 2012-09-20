using System;

namespace Vienna.SharpScript
{
    public class SharpScriptContext
    {
        public AppDomain Domain { get; set; }
        public SharpScriptMarshaller Marshaller { get; set; }
    }
}