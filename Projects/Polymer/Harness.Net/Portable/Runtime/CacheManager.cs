using System.Portable.IO;
using System.Portable.Runtime.Environment;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RaptorDB;
using Path = System.IO.Path;

namespace System.Portable.Runtime
{
    public class RaptorDbCacheService : ICacheService
    {
        protected RaptorDB<string> Cache { get; set; } 

        public RaptorDbCacheService() {
            var f = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.cache").As<RuntimeFile>();
            if (f.Exists) f.Delete(); 
            Cache = new RaptorDB<string>(f.Path, false);
        }

        public T Get<T>(string name) {
            string r;
            Cache.Get(name, out r);
            return JObject.Parse(r).As<T>();
        }

        public void Set<T>(string name, T value) {
            Cache.Set(name, new JObject(value).ToString(Formatting.None));
        }

        public void Dispose() {
            Cache.Dispose();
        }
    }
}
