#region ApacheLicense

// System.Portable.Base
// Copyright © 2014 Nick Daniels et all, All Rights Reserved.
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

using System.Collections.Generic;
using System.Linq;
using System.Portable;
using System.Portable.Reflection;

#endregion

namespace System.Contracts {
    public static class Contract {
        /// <summary>
        /// Evaluates an assertion against a given object.
        /// </summary>
        /// <typeparam name="T">The type of the given object</typeparam>
        /// <param name="o">The object</param>
        /// <param name="targetName">Name of the target.</param>
        /// <param name="assertion">The assertion.</param>
        /// <param name="args">The arguments.</param>
        /// <param name="invalidMessage">a message should the assertion fail</param>
        /// <param name="thrw">if set to <c>true</c> [Throw ContractValidationException].</param>
        /// <returns></returns>
        /// <exception cref="ContractAssertionException"></exception>
        private static ValidationResult Assert<T>(this T o, string targetName, Delegate assertion, object[] args = null, string invalidMessage = null, bool thrw = false) {
            Exception e = null;
            var arguments = new List<object> {o};
            args.NotNull(a => a.AddTo(arguments));
            if (assertion.Try(a => (bool)App.Container.Get<IReflector>().Invoke(a, arguments.ToArray()))
                .Catch<Exception>((x, ex) => {
                    e = ex;
                    return false;
                }
            ).Act()) return new ValidationResult(targetName, true, ex: e);
            if (thrw) throw new ContractAssertionException();
            return new ValidationResult(targetName, false, invalidMessage ?? "failed", e);
        }

        /// <summary>
        /// Asserts the specified o.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o">The object.</param>
        /// <param name="targetName">Name of the target.</param>
        /// <param name="assertion">The assertion.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static ValidationResult Assert<T>(this T o, string targetName, Assertion assertion, params object[] args) {
            return Assert(o, targetName, assertion.Predicate, invalidMessage: assertion.InvalidMessage, args: args);
        }

        public static ValidationResults Validate<T>(this T o) {
            var t = typeof (T);
            var results = new ValidationResults();

            t.GetProperties()
                .Where(x => x.GetIndexParameters().Length == 0) // NOT AN INDEX PROPERTY
                .SelectMany(
                    x =>
                        x.GetCustomAttributes(true)
                        .Where(y => y.Is<ContractPropertyAttribute>())
                        .Select(y => y.As<ContractPropertyAttribute>())
                        .Select(y => y.Assert(x, x.GetValue(o, null)))
                        .As<IEnumerable<ValidationResult>>()
                ).AddTo(results);
            return results;
        }
    }
}