using System.IO;

namespace Vienna.Resources
{
    public class Resource
    {
        public ResourceData Data { get; protected set; }
        public Stream Stream { get; protected set; }
        public long CacheTimeStamp { get; set; }

        public Resource(ResourceData data, Stream stream)
        {
            Data = data;
            Stream = stream;
        }
    }
}