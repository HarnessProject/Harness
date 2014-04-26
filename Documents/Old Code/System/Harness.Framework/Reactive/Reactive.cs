#region ApacheLicense

// From the Harness Project
// System.Portable
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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

#region

using System;
using System.Reactive;
using System.Threading.Tasks;
using Harness.Framework.Extensions;
using Harness.Framework.Tasks;

#endregion

namespace Harness.Framework.Reactive
{
    /// <summary>
    ///     A reactive wrapper around a single object whose value is observed.
    ///     Reactive implements IDisposable and will dispose an IDisposable value.
    ///     
    /// </summary>
    /// <typeparam name="T">The Type of the wrapped Value</typeparam>
    public class Reactive<T> : IReactive<T>
    {
        private T _value;

        public Reactive(T val)
        {
            _value = val;
        }


       

        #region IReactive<T> Members

        public T Value { get { return _value; } set { SetValue(value); } }

        
        

        public void Dispose()
        {
            Completed();
            if (typeof(T).Is<IDisposable>())
                Value.AsType<IDisposable>().Dispose();
        }

        public event Action<T> OnNext;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            OnNext += observer.OnNext;
            OnError += observer.OnError;
            OnCompleted += observer.OnCompleted;
            return this;
        }

        public event Action<Exception> OnError;
        public event Action OnCompleted;

        #endregion

        protected void Next(T val)
        {
            OnNext.NotNull(o => o(val));
        }

        protected void Error(Exception ex)
        {
            OnError.NotNull(o => o(ex));
        }

        protected void Completed()
        {
            OnCompleted.NotNull(o => o());
        }

        public void SetValue(T value)
        {
            this.Try(x =>
            {
                _value = value;
                x.Next(value);
                return true;
            }).Catch<Exception>(
                (x, ex) =>
            {
                x.AsTask(y => y.Error(ex));
                return false;
            }).Act();
        }

        public Task SetValueAsync(T value)
{
            return this.AsTask(x => x.SetValue(value));
        }

        public static implicit operator T(Reactive<T> val)
{
            return val.Value;
        }

        public static implicit operator Reactive<T>(T val)
{
            return new Reactive<T>(val);
        }

        public override string ToString() {
            return Value.ToString();
        }
    }
}