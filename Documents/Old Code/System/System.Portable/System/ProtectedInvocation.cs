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

using System.Collections.Generic;
using System.Linq;
using System.Portable;

#endregion

namespace System {
    public class ProtectedAction<T, TY> : IProtectedAction<T, TY> {
        private readonly Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>> _exceptionFuncs;
        private readonly Dictionary<int, Action<TY>> _finallyFuncs;
        private readonly Dictionary<int, Func<T, TY>> _protectedFuncs;
        private readonly T _target;
        private int _currentFunc;
        private int _indexSeed = -1;

        public ProtectedAction(T target) {
            _protectedFuncs = new Dictionary<int, Func<T, TY>>();
            _target = target;
            _exceptionFuncs = new Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>>();
            _finallyFuncs = new Dictionary<int, Action<TY>>();
        }

        #region IProtectedAction<T,TY> Members

        public IProtectedAction<T, TY> Try(Func<T, TY> tTry) {
            _protectedFuncs.Add(++_indexSeed, tTry);
            _currentFunc = _indexSeed;
            return this;
        }

        public IProtectedAction<T, TY> Catch<TX>(Func<T, TX, TY> tCatch) where TX : Exception {
            if (!_exceptionFuncs.ContainsKey(_indexSeed)) _exceptionFuncs[_indexSeed] = new Dictionary<Type, Func<T, Exception, TY>>();
            _exceptionFuncs[_indexSeed].Add(typeof (TX), (t, ex) => tCatch(t, ex as TX));
            return this;
        }

        public IProtectedAction<T, TY> Finally(Action<TY> tFinal) {
            _finallyFuncs.Add(_currentFunc, tFinal);
            return this;
        }

        public TY Act() {
            var result = default(TY);
            foreach (var f in _protectedFuncs.OrderBy(x => x.Key))
                try {
                    result = f.Value(_target);
                }
                catch (Exception ex) {
                    Type ext = ex.GetType();
                    T t = _target;
                    TY r = default(TY);
                    if (!_exceptionFuncs.ContainsKey(f.Key)) return r;
                    if (_exceptionFuncs[f.Key].ContainsKey(ext)) r = _exceptionFuncs[f.Key][ext](t, ex);
                    else r = _exceptionFuncs[f.Key].ContainsKey(typeof (Exception)) ? _exceptionFuncs[f.Key][typeof (Exception)](t, ex) : r;
                    if (_finallyFuncs.ContainsKey(f.Key)) _finallyFuncs[f.Key](r);

                    result = r;
                }

            return result;
        }

        #endregion
    }
}