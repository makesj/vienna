using System;
using System.Collections.Generic;
using System.Linq;

namespace Vienna.Resources
{
    public class ResourceManager
    {
        const string InvalidQualifierError = "Invalid resource name '{0}'. Resource loader must be qualified using ':'";
        private const string LoaderDoesNotExistError = "Resource loader does not exist '{0}'";

        protected Dictionary<string, ResourceLoader> Loaders;

        public ResourceManager()
        {
            Loaders = new Dictionary<string, ResourceLoader>();
        }

        public ResourceLoader this[string name]
        {
            get { return Loaders[name]; }
        }

        public void AddLoader(string name, ResourceLoader loader)
        {
            Loaders.Add(name.ToLower(), loader);
        }

        public void RemoveLoader(string name)
        {
            var loader = Loaders[name];
            loader.Dispose();
            Loaders.Remove(name);
        }

        public CacheStat[] GetCacheStats()
        {
            var list = new List<CacheStat>();
            foreach (var loader in Loaders)
            {
                var cache = loader.Value.Cache;
                var catalog = loader.Value.Catalog;

                list.Add(new CacheStat
                {
                    CacheSize = cache.CacheSize,
                    CacheAllocated = cache.Allocated,
                    ThrashCount = cache.ThrashCount,
                    ThrashSize = cache.ThrashSize,
                    CacheCount = cache.Count,
                    ItemSize = catalog.Size,
                    ItemCount = catalog.Count,
                    Loader = loader.Key,
                });
            }

            return list.ToArray();
        }

        public void ResetThrash()
        {
            foreach (var loader in Loaders)
            {
                loader.Value.Cache.ResetThrash();
            }
        }
    }
}
