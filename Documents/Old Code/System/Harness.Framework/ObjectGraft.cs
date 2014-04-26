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
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;

#endregion

namespace Harness.Framework {
    public class ObjectGraft<T, TY> {
        protected readonly List<Action<T, TY>> Actions = new List<Action<T, TY>>();
        protected readonly List<Action<TY>> PostGraftActions = new List<Action<TY>>();
        protected readonly List<Action<T>> PreGraftActions = new List<Action<T>>();
        protected readonly List<Tuple<MemberExpression, MemberExpression>> PropertyGrafts = new List<Tuple<MemberExpression, MemberExpression>>();
        //public IReflector Reflector { get; set; }

        public ObjectGraft<T, TY> GraftMember<TX, TZ>(Expression<Func<T, TX>> leftExpression, Expression<Func<TY, TZ>> rightExpression) {
            PropertyGrafts.Add(new Tuple<MemberExpression, MemberExpression>(leftExpression.Body.AsType<MemberExpression>(), rightExpression.Body.AsType<MemberExpression>()));
            return this;
        }

        public ObjectGraft<T, TY> GraftAction(Action<T, TY> action) {
            Actions.Add(action);
            return this;
        }

        public ObjectGraft<T, TY> BeforeGraft(Action<T> action) {
            PreGraftActions.Add(action);
            return this;
        }

        public ObjectGraft<T, TY> AfterGraft(Action<TY> action) {
            PostGraftActions.Add(action);
            return this;
        }

        public TY Graft(T t, TY ty) {
            PreGraftActions.Each(x => x(t));
            PropertyGrafts.Each(x => {
                var vL = Provider.Reflector.GetPropertyValue(t, x.Item1.Member.Name);
                Provider.Reflector.SetPropertyValue(ty, x.Item2.Member.Name, vL);
            });
            Actions.Each(x => x(t, ty));
            PostGraftActions.Each(x => x(ty));
            return ty;
        }

        public IEnumerable<TY> GraftAll(IEnumerable<T> tS, Func<TY> fTy) {
            return tS.Select(x => Graft(x, fTy()));
        }
    }
}