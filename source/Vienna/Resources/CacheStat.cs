using System;

namespace Vienna.Resources
{
    public class CacheStat
    {
        public string Loader { get; set; }
        public long ItemSize { get; set; }
        public long CacheAllocated { get; set; }
        public int CacheCount { get; set; }
        public int ItemCount { get; set; }
        public long CacheSize { get; set; }
        public int ThrashCount { get; set; }
        public long ThrashSize { get; set; }

        public long CacheRemaining
        {
            get { return CacheSize - CacheAllocated; }
        }

        /// <summary>
        /// A metric indicating how efficient the cache is handling resources.
        /// Positive numbers mean the cache has more space than required
        /// Negative numbers means the cache is thrashing and more memory is needed
        /// A bigger number should indicates how severe the problem is.
        /// A zero would indicate a perfect balance.
        /// </summary>
        public float CacheEfficiency
        {
            get
            {
                var thrashPercent = (float) ThrashSize/CacheSize;
                var allocatedPercent = (float) CacheAllocated/ItemSize;
                var cacheWaste = (float) CacheAllocated/CacheSize;
                return (allocatedPercent - thrashPercent - cacheWaste) * 100.0f;
            }
        }
    }
}