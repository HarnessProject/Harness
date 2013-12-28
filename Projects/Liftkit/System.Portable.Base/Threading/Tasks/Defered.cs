using System;
namespace System.Threading.Tasks {
    
    //<summary>
    //  Defers execution of an operation until it's explictly converted into its result type or one of the result methods is called.
    //  Useful for internal property implementations where you dont want to obtain the value if you don't want to. Also a handy way 
    //  to solve the issue that nearly all code using Task or Task<T> expects the task to be "hot", as in started, and we may want 
    //  to hold a Task or 2 and only start them when the time comes, or perform them again and again, we just defer creating them and
    //  reuse the defered.
    //</summary>
    public class Defered<T> {
        /*
         * Example
            var str = Defered<string>.Create(
         *      c => string.Join(" ", new [] { c.FirstName, "\s", c.LastName }), 
         *      () => new { FirstName = NameService.GetFirst(), LastName = NameService.GetLast() }
         *  );
         * Usage 
            str as string OR str.As<string>() OR await str.AsTask(s => s.As<string>())
            OR str.Result() OR await str.ResultAsync();
         */

        private Defered() {}
       
        protected Func<T> Op { get; set; }

        public T Result() {
            return Op();
        }

        public Task<T> ResultAsync() {
            return this.AsTask(x => x.Op());
        }

        //Stupid Method Tricks
        /// <summary>
        /// Creates a defered operation
        /// </summary>
        /// <typeparam name="TY">The type of the y.</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="context">The context.</param>
        /// <returns></returns>
        public static Defered<T> Create<TY>(Func<TY,T> func, Func<TY> context) {
            var d = new Defered<T> {Op = () => func(context())};
            return d;
        }


        public static explicit operator T(Defered<T> op) {
            return op.Try(x => x.Op()).Catch<Exception>((y, ex) => default(T)).Invoke();
        }

        
    }
}