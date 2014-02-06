using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Collections.Generic
{
    public class DoubleKeyedDictionary<TKey1,TKey2,TY>
    {
        protected List<Tuple<TKey1, TKey2, TY>> Values { get; set; }

        protected IEnumerable<Tuple<TKey1, TKey2, TY>> Tuples(TKey1 key1, TKey2 key2) {
            return Values.Where(x => x.Item1.Equals(key1) && x.Item2.Equals(key2));
        } 

        public IEnumerable<TY> WithKeys(TKey1 key1, TKey2 key2) {
            return Tuples(key1,key2).Select(x => x.Item3);
        }

        public bool Contains(TKey1 key1, TKey2 key2) {
            return WithKeys(key1, key2).Any();
        }

        public IEnumerable<TY> this[TKey1 key1, TKey2 key2] {
            get {
                return WithKeys(key1, key2);
            }
            set {
                value.Each(x => Add(key1,key2,x));
            }
        } 

        public void Add(TKey1 key1, TKey2 key2, TY value) {
            Values.Add(new Tuple<TKey1, TKey2, TY>(key1, key2, value));
        }

        public void Remove(TKey1 key1, TKey2 key2) {
            Tuples(key1,key2).Each(Remove);
        }

        public void Remove(Tuple<TKey1, TKey2, TY> item) {
            Values.Remove(item);
        }
    }
}
