#region ApacheLicense

// System.Portable.Base
// Copyright © 2013 Nick Daniels et all, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing 	terms.
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

using System.Linq;

#endregion

namespace System.Collections.Generic {
    public class LookupTable<TKey, TValue> : ILookup<TKey, TValue> where TValue : class
    {
        private int _nextIndex;
        protected IDictionary<int, TKey> KeysDictionary { get; set; }
        protected HashSet<Tuple<int, TValue>> ValuesList { get; set; }

        public IEnumerable<TKey> Keys { get { return KeysDictionary.Values; } }
        public IEnumerable<TValue> Values { get { return ValuesList.Select(x => x.Item2); } }

        #region ILookup<TKey,TValue> Members

        public IEnumerator<IGrouping<TKey, TValue>> GetEnumerator() {
            return ToGrouping().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public bool Contains(TKey key) {
            return KeysDictionary.Values.Contains(key);
        }

        public int Count { get { return ToGrouping().Count(); } }

        public IEnumerable<TValue> this[TKey key] {
            get {
                IEqualityComparer<TKey> are = EqualityComparer<TKey>.Default;
                return
                    ToList()
                        .Where(k => are.Equals(k.Key, key))
                        .Select(p => p.Value);
            }
        }

        #endregion

        protected IEnumerable<IGrouping<TKey, TValue>> ToGrouping() {
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
            KeysDictionary.Add(i, key);
            values.Each(x => ValuesList.Add(new Tuple<int, TValue>(i, x)));
            return true;
        }
    }
}