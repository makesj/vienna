using System;
using System.Diagnostics;
using Vienna.Resources;

namespace ViennaConsole
{
    [TestCase(CaseNumber = 4)]
    public class TestResources
    {
        public void Execute()
        {
            var sw = new Stopwatch();

            var path = @"C:\projects\Vienna\poc\general\assets\assets.zip";
            var loader = new DefaultResourceLoader(path, 4);
            var manager = new ResourceManager();
            manager.AddLoader("default", loader);

            Load(sw, manager);


            Load(sw, manager);

        }

        private void Load(Stopwatch sw, ResourceManager manager)
        {
            manager.ResetThrash();

            sw.Restart();

            manager["default"].LoadAll();

            sw.Stop();

            var stats = manager.GetCacheStats()[0];

            Console.WriteLine("----------------------------------");
            Console.WriteLine("Elapsed {0}ms", sw.ElapsedMilliseconds);
            Console.WriteLine("ItemCount {0}", stats.ItemCount);
            Console.WriteLine("ItemSize {0}", stats.ItemSize);
            Console.WriteLine("CacheCount {0}", stats.CacheCount);
            Console.WriteLine("CacheSize {0}", stats.CacheSize);
            Console.WriteLine("CacheAllocated {0}", stats.CacheAllocated);
            Console.WriteLine("CacheRemaining {0}", stats.CacheRemaining);
            Console.WriteLine("ThrashCount {0}", stats.ThrashCount);
            Console.WriteLine("ThrashSize {0}", stats.ThrashSize);
            Console.WriteLine("CacheEfficiency {0}", stats.CacheEfficiency);
            Console.WriteLine("----------------------------------");

        }

        protected void ResourceLoaded(int size, int item, Resource resource)
        {
            var percent = (int)(((float)item / (float)size) * 100);
            Console.Write(percent + "%");
        }

        protected void StartLoad(string name)
        {
            Console.WriteLine("Loading '{0}'", name);
        }
    }
}
