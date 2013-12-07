using System.Threading.Tasks;
using Harness.Framework;

namespace System.Tasks {
    
    //<summary>
    //  Defers execution of an operation until it's explictly converted into its result type or one of the result methods is called.
    //</summary>
    public class Defered<T> {
        /*
         * Example
            var str = Defered<string>.Create(
         *      c => string.Join(" ", new [] { c.FirstName, "\s", c.LastName }), 
         *      () => new { FirstName = NameService.GetFirst(), LastName = NameService.GetLast() }
         *  );
         * Usage 
            str as string OR str.As<string>() OR await str.AsTask(s => s.As<string>());
         */

        private Defered() {}
       
        private Func<T> Op { get; set; }

        public T Result() {
            return Op();
        }

        public Task<T> ResultAsync() {
            return this.AsTask(x => x.Op());
        }

        //Stupid Method Tricks
        public static Defered<T> Create<TY>(Func<TY,T> func, Func<TY> context) {
            var d = new Defered<T> {Op = () => func(context())};
            return d;
        }


        public static explicit operator T(Defered<T> op) {
            return op.Try(x => x.Op()).Catch<Exception>((y, ex) => default(T)).Invoke();
        }

        
    }
}