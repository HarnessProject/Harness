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

using System.Linq;

#endregion

namespace System.Collections.Generic {
    public class DoubleKeyedDictionary<TKey1, TKey2, TY> {
        protected List<Tuple<TKey1, TKey2, TY>> Values { get; set; }
        public IEnumerable<TY> this[TKey1 key1, TKey2 key2] { get { return WithKeys(key1, key2); } set { value.Each(x => Add(key1, key2, x)); } }

        protected IEnumerable<Tuple<TKey1, TKey2, TY>> Tuples(TKey1 key1, TKey2 key2) {
            return Values.Where(x => x.Item1.Equals(key1) && x.Item2.Equals(key2));
        }

        public IEnumerable<TY> WithKeys(TKey1 key1, TKey2 key2) {
            return Tuples(key1, key2).Select(x => x.Item3);
        }

        public bool Contains(TKey1 key1, TKey2 key2) {
            return WithKeys(key1, key2).Any();
        }

        public void Add(TKey1 key1, TKey2 key2, TY value) {
            Values.Add(new Tuple<TKey1, TKey2, TY>(key1, key2, value));
        }

        public void Remove(TKey1 key1, TKey2 key2) {
            Tuples(key1, key2).Each(Remove);
        }

        public void Remove(Tuple<TKey1, TKey2, TY> item) {
            Values.Remove(item);
        }
    }
}