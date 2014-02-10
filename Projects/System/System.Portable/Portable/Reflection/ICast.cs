using System.Collections.Generic;

namespace System.Portable.Reflection
{
    public interface ICast<in T, out TY> {
        TY Cast(T t);
    }

    public abstract class Caster<T,TY> : ICast<T,TY> {
        
        public TY Cast(T t) {

            var result =
                t.Try(x => x.As<TY>())
                    .Catch<InvalidCastException>((x, ex) => {
                        var r = default(TY);
                        App.Container.GetAll<ICast<T, TY>>().UntilTrue(c => {
                            r = c.Try(caster => caster.Cast(t)).Catch<Exception>((caster, ez) => default(TY)).Act();
                            return r.NotDefault();
                        });
                        return r;
                    }).Catch<Exception>((x, ex) => default(TY)).Act();
            return result;

        }
    }
}
