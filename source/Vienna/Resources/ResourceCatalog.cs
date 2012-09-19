using System;
using System.Collections.Generic;
using System.Linq;

namespace Vienna.Resources
{
    public class ResourceCatalog
    {
        protected Dictionary<string, ResourceData> Items;
        public long Size { get; protected set; }

        public ResourceCatalog(int capacity)
        {
            Items = new Dictionary<string, ResourceData>(capacity);
        }

        public ResourceData this[string name]
        {
            get
            {
                name = name.ToLower();
                if (Items.ContainsKey(name))
                {
                    return Items[name.ToLower()];
                }

                Console.WriteLine("Warning! Resource does not exist '{0}'", name);
                return null;             
            }
        }

        public string[] Names
        {
            get { return Items.Keys.ToArray(); }
        }

        public int Count
        {
            get { return Items.Count; }
        }

        public void Add(ResourceData data)
        {
            Items.Add(data.FileName, data);
            Size += data.Size;
        }

        public void Add(ResourceData[] datas)
        {
            for (var i = 0; i < datas.Length; i++)
            {
                Add(datas[i]);
            }
        }

        public void Clear()
        {
            Items.Clear();
        }
    }
}