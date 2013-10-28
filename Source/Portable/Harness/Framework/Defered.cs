using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;

namespace Harness.Framework {
    /*
     * Defers execution of an operation until it's explictly converted into its result type.
     */

    public class Defered<T> : Dictionary<string, object> {
        /*
         * Example
            var str = new Defered<string>(
                d => string.Format("{0} Started: {1}, Resolved: {2}", d.Message, d.StartTime, d.EndTime())
            ){
                {"Message", "Boy have we waited..."},
                {"StartTime", DateTime.Now},
                {"EndTime", new Func<DateTime>(() => DateTime.Now)}
            };
         * Usage 
            str as string OR str.As<string>() OR str.ToTask().AwaitResult()
         */

        public Defered(Func<dynamic, T> op) {
            Op = op;
        }

        protected Func<dynamic, T> Op { get; set; }

        protected ExpandoObject ToExpandoObject() {
            dynamic r = new ExpandoObject();
            Keys.Each(x => r[x] = this[x]);
            return r;
        }

        public Task<T> ToTask() {
            return new Task<T>(() => (T) this);
        }

        public static explicit operator T(Defered<T> op) {
            return op.Try(x => x.Op(x.ToExpandoObject())).Catch<Exception>((y, ex) => default(T)).Invoke();
        }
    }
}