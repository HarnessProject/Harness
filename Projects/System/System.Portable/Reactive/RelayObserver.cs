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

using System.Reactive.Disposables;

#endregion

namespace System.Reactive {
    public abstract class Relay<T, TY> : IObserver<T>, IObservable<TY> {
        #region IObservable<TY> Members

        public IDisposable Subscribe(IObserver<TY> observer) {
            Next += observer.OnNext;
            Error += observer.OnError;
            Complete += observer.OnCompleted;
            return Disposable.Empty;
        }

        #endregion

        #region IObserver<T> Members

        public void OnNext(T value) {
            Next.NotNull(n => n(Process(value)));
        }

        public void OnError(Exception error) {
            Error.NotNull(e => e(error));
        }

        public void OnCompleted() {
            Complete.NotDefault(c => c());
        }

        #endregion

        public event Action<TY> Next;
        public event Action<Exception> Error;
        public event Action Complete;

        public abstract TY Process(T t);
    }
}