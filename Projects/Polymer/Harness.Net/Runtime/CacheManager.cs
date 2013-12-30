using System;
using System.Collections.Generic;
using System.Linq;
using System.Portable.IO;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RaptorDB;
using Path = System.IO.Path;

namespace System.Runtime
{
    public class CacheService 
    {
        protected RaptorDB<string> Cache { get; set; } 

        public CacheService() {
            var f = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "session.cache").As<RuntimeFile>();
            if (f.Exists) f.Delete(); 
            Cache = new RaptorDB<string>(f.Path, false);
        }

        public object this[string name] {
            get {
                string r;
                Cache.Get(name, out r);
                return JObject.Parse(r);

            }
            set {
                Cache.Set(name, new JObject(value).ToString(Formatting.None));
            }
        }
    }

    public interface ICacheService {
        object this[string name] { get; set; }
    }
}
