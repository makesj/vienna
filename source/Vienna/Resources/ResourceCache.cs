using System;
using System.Collections.Generic;
using System.Linq;

namespace Vienna.Resources
{
    public class ResourceCache : IDisposable
    {
        private const int MegaByteSize = 1024;

        protected Dictionary<string, Resource> Items;

        public long CacheSize { get; protected set; }
        public long Allocated { get; protected set; }
        public int ThrashCount { get; protected set; }
        public long ThrashSize { get; protected set; }

        public long Available
        {
            get { return CacheSize - Allocated; }
        }

        public int Count 
        {
            get { return Items.Count; }
        }

        public Resource this[string name]
        {
            get { return Items[name]; }
        }

        public ResourceCache(int cacheMb)
        {
            CacheSize = ((long)cacheMb) * MegaByteSize * MegaByteSize;
            Items = new Dictionary<string, Resource>();
        }

        public void Add(Resource resource)
        {
            if(resource == null) return;

            if (resource.Data.Size > CacheSize)
            {
                Console.WriteLine("Warning! resource '{0}' is too large to be cached!", resource.Data.FileName);
                return;
            }

            if (resource.Data.Size + Allocated > CacheSize)
            {
                Console.WriteLine("Warning! forced cache allocation for '{0}', size={1}, avail={2}", resource.Data.FileName, resource.Data.Size, Available);
                var size = Allocate(resource.Data.Size);
                Console.WriteLine("Reallocated {0} bytes", size);
            }
            Items.Add(resource.Data.FileName, resource);
            Allocated += resource.Data.Size;
        }

        public long Allocate(long bytes)
        {
            var items = Items.Values.OrderBy(x => x.CacheTimeStamp).ToArray();
            long reallocation = 0;
            for (var i = 0; i < items.Length; i++)
            {
                var resource = items[i];

                reallocation += resource.Data.Size;
                Remove(resource);
                ThrashCount++;
                ThrashSize += resource.Data.Size;

                if (reallocation >= bytes) break;
            }
            return reallocation;
        }

        public bool Contains(string name)
        {
            return Items.ContainsKey(name.ToLower());
        }

        public void ResetThrash()
        {
            ThrashCount = 0;
            ThrashSize = 0;
        }

        public Resource Get(string name)
        {
            name = name.ToLower();
            if (Contains(name)) return Items[name];
            Console.WriteLine("Warning! cached resource does not exist '{0}'",name);
            return null;
        }

        public void Remove(Resource resource)
        {
            Items.Remove(resource.Data.FileName);
            Allocated -= resource.Data.Size;
            resource.Stream.Close();
        }

        public void Clear()
        {
            foreach (var resource in Items)
            {
                if (resource.Value != null && resource.Value.Stream != null)
                {
                    resource.Value.Stream.Close();                    
                }
            }
            Items.Clear();
            Allocated = 0;
        }

        public void Dispose()
        {
            Clear();
        }

        private class ResourceTimeStampComparer : IComparer<Resource>
        {
            public int Compare(Resource x, Resource y)
            {
                if (x.CacheTimeStamp > y.CacheTimeStamp) return 1;
                if (x.CacheTimeStamp < y.CacheTimeStamp) return -1;
                return 0;
            }
        }
    }
}
