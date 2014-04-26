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
using System.Composition.Reflection;
using System.Threading.Tasks;
using Harness.Framework.Collections;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using Harness.Framework.Tasks;

#endregion

namespace Harness.Framework.Reflection
{
    public static class Convert<T> {
        public static TY To<TY>(T t) {
            var cast = Defered<TY>.Create(() => t.AsType<TY>());
            var fromService = Defered<TY>.Create(() => {
                var r = default(TY);

                Provider
                    .GetAll<IConvert<T, TY>>()
                    .UntilTrue(
                        c =>
                            c.Try(caster => {
                                r = caster.Convert(t);
                                return true;
                            }).Catch<Exception>(
                                (caster, ez) => false
                            ).Act()
                    );

                return r;
            });

            var result =
                t.Try(x => cast.Result())
                    .Catch<Exception>(
                        (x, ex) =>
                            x.Try(y => fromService.Result())
                            .Catch<Exception>((y, ey) => default(TY))
                            .Act()
                    ).Act();
            return result;
        }

        public static Task<TY> AsyncTo<TY>(T t) {
            return t.AsTask(x => To<TY>(x));
        }
    }
}