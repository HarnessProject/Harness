using System.Collections.Generic;
using System.Linq;

namespace System {
    public class ProtectedInvocation<T, TY> {
        private readonly Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>> _exceptionFuncs;
        private readonly Dictionary<int, Action<dynamic>> _finallyFuncs;
        private readonly Dictionary<int, Func<T, TY>> _protectedFuncs;
        private int _currentFunc;
        private int _indexSeed = -1;
        private dynamic _target;

        public ProtectedInvocation(T target) {
            _protectedFuncs = new Dictionary<int, Func<T, TY>>();
            _target = target;
            _exceptionFuncs = new Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>>();
            _finallyFuncs = new Dictionary<int, Action<dynamic>>();
        }

        public ProtectedInvocation<T, TY> Try(Func<T, TY> tTry) {
            _protectedFuncs.Add(++_indexSeed, tTry);
            _currentFunc = _indexSeed;
            return this;
        }

        

        public ProtectedInvocation<T, TY> Catch<TX>(Func<T, TX, TY> tCatch) where TX : Exception {
            if (!_exceptionFuncs.ContainsKey(_indexSeed)) _exceptionFuncs[_indexSeed] = new Dictionary<Type, Func<T, Exception, TY>>();
            _exceptionFuncs[_indexSeed].Add(typeof (TX), (t, ex) => tCatch(t, ex as TX));
            return this;
        }

        public ProtectedInvocation<T, TY> Finally(Action<dynamic> tFinal) {
            _finallyFuncs.Add(_currentFunc, tFinal);
            return this;
        }

        public TY Invoke() {
            foreach (var f in _protectedFuncs.OrderBy(x => x.Key))
                try {
                    _target = f.Value(_target);
                }
                catch (Exception ex) {
                    Type ext = ex.GetType();
                    T t = _target;
                    TY r = default(TY);

                    if (_exceptionFuncs[f.Key].ContainsKey(ext)) r = _exceptionFuncs[f.Key][ext](t, ex);

                    r = _exceptionFuncs[f.Key].ContainsKey(typeof (Exception)) ? _exceptionFuncs[f.Key][typeof (Exception)](t, ex) : r;

                    if (_exceptionFuncs[f.Key].ContainsKey(ext)) _finallyFuncs[f.Key](r);

                    _target = r;
                }

            return (TY) _target;
        }

        
    }
}