using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace System {
    public interface IProtectedInvocation<T, TY> {
        IProtectedInvocation<T, TY> Try(Func<T, TY> tTry);
        IProtectedInvocation<T, TY> Catch<TX>(Func<T, TX, TY> tCatch) where TX : Exception;
        IProtectedInvocation<T, TY> Finally(Action<TY> tFinal);
        TY Invoke();
    }

    public class ProtectedInvocation<T, TY> : IProtectedInvocation<T, TY> {
        private readonly Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>> _exceptionFuncs;
        private readonly Dictionary<int, Action<TY>> _finallyFuncs;
        private readonly Dictionary<int, Func<T, TY>> _protectedFuncs;
        private int _currentFunc;
        private int _indexSeed = -1;
        private readonly T _target;

        public ProtectedInvocation(T target) {
            _protectedFuncs = new Dictionary<int, Func<T, TY>>();
            _target = target;
            _exceptionFuncs = new Dictionary<int, Dictionary<Type, Func<T, Exception, TY>>>();
            _finallyFuncs = new Dictionary<int, Action<TY>>();
        }

        public IProtectedInvocation<T, TY> Try(Func<T, TY> tTry) {
            _protectedFuncs.Add(++_indexSeed, tTry);
            _currentFunc = _indexSeed;
            return this;
        }

        

        public IProtectedInvocation<T, TY> Catch<TX>(Func<T, TX, TY> tCatch) where TX : Exception {
            if (!_exceptionFuncs.ContainsKey(_indexSeed)) _exceptionFuncs[_indexSeed] = new Dictionary<Type, Func<T, Exception, TY>>();
            _exceptionFuncs[_indexSeed].Add(typeof (TX), (t, ex) => tCatch(t, ex as TX));
            return this;
        }

        public IProtectedInvocation<T, TY> Finally(Action<TY> tFinal) {
            _finallyFuncs.Add(_currentFunc, tFinal);
            return this;
        }

        public TY Invoke() {
            TY result = default (TY);
            foreach (var f in _protectedFuncs.OrderBy(x => x.Key))
                try {
                    result = f.Value(_target);
                }
                catch (Exception ex) {
                    Type ext = ex.GetType();
                    T t = _target;
                    TY r = default(TY);

                    if (_exceptionFuncs[f.Key].ContainsKey(ext)) r = _exceptionFuncs[f.Key][ext](t, ex);

                    r = _exceptionFuncs[f.Key].ContainsKey(typeof (Exception)) ? _exceptionFuncs[f.Key][typeof (Exception)](t, ex) : r;

                    if (_exceptionFuncs[f.Key].ContainsKey(ext)) _finallyFuncs[f.Key](r);

                    result = r;
                }

            return (TY) result;
        }

        
    }

    public static class ProtectedInvocationExtensions
    {
        public static ProtectedInvocation<T, TY> Try<T, TY>(this T obj, Func<T, TY> tTry)
        {
            var p = new ProtectedInvocation<T, TY>(obj);
            p.Try(tTry);
            return p;
        } 
    }

}