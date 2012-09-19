using System;
using System.Collections.Generic;
using System.IO;
using Ionic.Zip;

namespace Vienna.Resources
{
    public abstract class ResourceLoader : IDisposable
    {
        protected ZipFile Extractor;
        public ResourceCatalog Catalog { get; protected set; }
        public ResourceCache Cache { get; protected set; }

        public string Path { get; protected set; }

        protected ResourceLoader(string path, int cacheMb)
        {
            Path = path;
            Cache = new ResourceCache(cacheMb);
            Extractor = new ZipFile(path);
            BuildIndex();
        }

        protected void BuildIndex()
        {
            var indices = new List<ResourceData>();

            using (var zip = ZipFile.Read(Path))
            {
                foreach (ZipEntry entry in zip)
                {
                    if (entry.IsDirectory) continue;
                    var data = FilterCatalog(entry.UncompressedSize, entry.FileName);
                    if(data != null) indices.Add(data);
                }
            }

            Catalog = new ResourceCatalog(indices.Count);
            Catalog.Add(indices.ToArray());
        }

        public abstract ResourceData FilterCatalog(long size, string fileName);

        public Resource LoadFromDisk(string name)
        {
            var data = Catalog[name];
            if (data == null) return null;
            var buffer = new byte[data.Size];
            var stream = new MemoryStream(buffer);
           
            try
            {
                using (var zip = ZipFile.Read(Path))
                {
                    var entry = zip[data.FileName];
                    entry.Extract(stream);
                }  
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! could not load resource -> {0}", ex.Message);
                return null;
            }
            return new Resource(data, stream);
        }

        public Resource Load(string name)
        {
            if (Cache.Contains(name)) return Cache[name];
            var resource = LoadFromDisk(name);
            Cache.Add(resource);
            return resource;
        }

        public Resource[] LoadMany(string[] names, Action<int, int, Resource> loadFinish = null, Action<string> startLoad = null)
        {
            var size = names.Length;
            var items = new Resource[size];
            for (var i = 0; i < names.Length; i++)
            {
                if (startLoad != null) startLoad(names[i]);
                var resource = Load(names[i]);
                if (loadFinish != null) loadFinish(size, i + 1, resource);
                items[i] = resource;
            }
            return items;
        }

        public Resource[] LoadAll(Action<int, int, Resource> loadFinish = null, Action<string> startLoad = null)
        {
            return LoadMany(Catalog.Names, loadFinish, startLoad);
        }

        public void ClearCache()
        {
            Cache.Clear();
        }

        public void Dispose()
        {
            ClearCache();
            Catalog.Clear();
        }
    }
}
