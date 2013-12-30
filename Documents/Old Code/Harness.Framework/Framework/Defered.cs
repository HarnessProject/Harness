using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Harness.Framework {
    /* 
     * Defers execution of an operation until it's explictly converted into its result type or one of the result methods is called.
     * Defered is not actually used ANYWHERE in Harness, it was once upon a time, but was removed due to design changes.
     * It is provided for your convienience.
     * It's handy if you have an operation you may or maynot perform, you can create a defered and if conditions cause you to not perform the action, 
     * it's never performed. and if you need to perform the defered operation as a task, then we provide that too.
     * 
     */


    //<summary>Defers execution of an operation until it's explictly converted into its result type or one of the result methods is called.</summary>
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

        //<summary>Retur
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