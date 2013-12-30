using System.Linq;

namespace System.Collections.Generic
{
    

    public class LookupTable<TKey,TValue> : ILookup<TKey,TValue> {
        private int _nextIndex;
        protected HashSet<Tuple<int, TKey>> KeysDictionary { get; set; }
        protected HashSet<Tuple<int,TValue>> ValuesList { get; set; }

        public IEnumerable<TKey> Keys { get { return KeysDictionary.Values; } }
        public IEnumerable<TValue> Values { get { return ValuesList.Select(x => x.Item2); } }

        protected IEnumerable<IGrouping<TKey,TValue>> ToGrouping() {
            return ValuesList
                .GroupBy(v => KeysDictionary[v.Item1], v => v.Item2);
        }

        public IEnumerable<KeyValuePair<TKey, TValue>> ToList() {
            return 
                ToGrouping()
                .SelectMany(
                    g =>
                        g.Select(v => new KeyValuePair<TKey, TValue>(g.Key, v))
                );
        } 

        public bool Add(TKey key, params TValue[] values) {
            var i = _nextIndex++;
            KeysDictionary.Add(i,key);
            values.Each(x => ValuesList.Add(new Tuple<int, TValue>(i,x)));
            return true;
        }

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() {
            return ToGrouping().GetEnumerator();
        }

       
        IEnumerator IEnumerable.GetEnumerator() {
                return GetEnumerator(); 
        }

        public bool Contains(TKey key) {
            return KeysDictionary.Values.Contains(key);
        }

        public int Count { get {
            return ToGrouping().Count();
        } }

        public IEnumerable<TValue> this[TKey key] {
            get {
                IEqualityComparer<TKey> are = EqualityComparer<TKey>.Default;
                return
                    ToList()
                        .Where(k => are.Equals(k.Key, key))
                        .Select(p => p.Value);

            }
        }
    }
}
