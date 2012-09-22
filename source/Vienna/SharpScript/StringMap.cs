using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Vienna.SharpScript
{
    [Serializable]
    public class StringMap : Dictionary<string,string>
    {
        public StringMap()
        {
        }

        public StringMap(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

        public override string ToString()
        {
            var sb = new StringBuilder("[\n");
            foreach (var pair in this)
            {
                sb.Append("[").Append(pair.Key).Append(",").Append(pair.Value).Append("]\n");
            }
            return sb.Append("]").ToString();
        }
    }
}
