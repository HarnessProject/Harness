using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace System.Portable.Runtime.Reflection
{
    public class ObjectFactory<T> : IFactory<object> {
        public Task<object> CreateAsync() {
            return this.AsTask(x => x.Create());
        }

        public object Create() {
            return Create(new object[]{});
        }
       
        public object Create(params object[] args) {
            var argTypes = args.Select(y => y.GetType());
            var type = 
                App
                .TypeProvider
                .Types
                .FirstOrDefault(x => args.FirstOrDefault().NotNull() && x.Is(argTypes.First()));
            if (type == null) return default(T);
            var con = 
                type
                .Func(
                    x => x != null ?  x.GetConstructor(argTypes.Skip(1).ToArray()) : null
                );
            return con == null ? default(T) : (T)con.Invoke(args.Skip(1).ToArray());
        }


    }
}
