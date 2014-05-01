#region ApacheLicense
// From the Harness Project
// Harness.Framework
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
using System;
using System.Threading.Tasks;
using Harness.Framework.Extensions;
using Harness.Framework.Reactive;

namespace Harness.Framework.Tasks {
    //<summary>
    //  Defers execution of an operation until it's explictly converted into its result type or one of the result methods is called.
    //  Useful for internal property implementations where you dont want to obtain the value if you don't have to. Also a handy way 
    //  to solve the issue that nearly all code using Task or Task<T> expects the task to be "hot", as in started, and we may want 
    //  to hold a Task or 2 and only start them when the time comes, or perform them again and again, we just defer creating them and
    //  reuse the defered.
    //</summary>
    public class Defered<T> : IReactive<T> {
        /*
         * Example
            var str = Defered<string>.Create(
         *      c => string.Join(" ", new [] { c.FirstName, c.LastName }), 
         *      () => new { FirstName = NameService.GetFirst(), LastName = NameService.GetLast() }
         *  );
         * Usage 
            str as string OR str.As<string>() OR await str.AsTask(s => s.As<string>())
            OR str.Result() OR await str.ResultAsync();
         */

        private Defered() {}

        protected T Value { get; set; }

        T IReactive<T>.Value {
            get { return Value; }
            set { Value = value; }
        }

        public bool HasValue() {
            return !Value.IsDefault();
        }

        public bool YieldOnce { get; set; }

        public void SetValue(T value) {
            throw new InvalidOperationException("Call Result() to change the value of a Defered<T>");
        }

        public Task SetValueAsync(T value) {
            throw new InvalidOperationException("Call Result() to change the value of a Defered<T>");
        }

        public event Action<T> OnNext;
        public event Action<Exception> OnError;
        public event Action OnCompleted;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            OnNext += observer.OnNext;
            OnError += observer.OnError;
            OnCompleted += observer.OnCompleted;
            return this;
        }


        public void Dispose() { }

        protected Func<T> Yield { get; set; }

        public T Result() {
            return Value.IsDefault(() => 
                this.Try(x => {
                    x.Value = YieldOnce && !x.Value.IsDefault() ? x.Value : x.Yield();
                    OnNext.NotNull(n => n(x.Value));
                    return x.Value;
                }).Catch<Exception>(
                    (y, ex) => {
                        OnError.NotNull(e => e(ex));
                        return default(T);
                    }
                ).Finally(x => OnCompleted.NotNull(c => c()))
                .Act()
            );
        }

        public Task<T> ResultAsync() {
            return this.AsTask(x => x.Result());
        }

        //Stupid Method Tricks
        /// <summary>
        ///     Creates a defered operation
        /// </summary>
        /// <typeparam name="TY">Context</typeparam>
        /// <param name="func">The function.</param>
        /// <param name="context">The context.</param>
        /// <param name="yieldOnce">When true configures the defered to yield only once</param>
        /// <returns></returns>
        public static Defered<T> Create<TY>(Func<TY, T> func, Func<TY> context, bool yieldOnce = false) {
            var d = new Defered<T> {Yield = () => func((context())), YieldOnce = yieldOnce};
            return d;
        }
        public static Defered<T> Create<TY>(Func<TY, T> func, TY context, bool yieldOnce = false)
        {
            var d = new Defered<T> { Yield = () => func(context), YieldOnce = yieldOnce };
            return d;
        }
        public static Defered<T> Create(Func<T> func, bool yieldOnce = false) {
            var d = new Defered<T> {Yield = func, YieldOnce = yieldOnce };
            return d;
        }

        public static implicit operator T(Defered<T> op) {
            return op.Result();
        }

        public static implicit operator Task<T>(Defered<T> op) {
            return op.ResultAsync();
        }

        
    }

}