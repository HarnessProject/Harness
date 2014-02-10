using System.Portable.Data.Caching;
using System.Portable.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RaptorDB;
using Path = System.IO.Path;

namespace System.Portable.Runtime
{
    public class RaptorDbCacheService<TKey> : ICacheProvider<TKey> where TKey : IComparable<TKey> {
        protected RaptorDB<TKey> Cache { get; set; } 

        public RaptorDbCacheService(bool clear = false) {
            var f = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.cache").As<RuntimeFile>();
            if (clear && f.Exists) f.Delete(); 
            Cache = new RaptorDB<TKey>(f.Path, false);
        }

        public T Get<T>(TKey key) where T : class {
            string r;
            Cache.Get(key, out r);
            return JObject.Parse(r).As<T>();
        }

        public void Set<T>(TKey key, T value) where T : class {
            Cache.Set(key, new JObject(value).ToString(Formatting.None));
        }
        
        public void Dispose() {
            Cache.Dispose();
        }
    }
}
