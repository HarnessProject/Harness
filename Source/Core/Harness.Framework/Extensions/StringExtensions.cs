#region ApacheLicense
// From the Harness Project
// Harness.Framework
// Copyright © 2014 Nick Daniels, All Rights Reserved.
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
using System.Text.RegularExpressions;
using Harness.Framework.Collections;

#endregion

namespace Harness.Framework.Extensions {
    public static class StringExtensions {
        public static bool IsMatch(this string str, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.IsMatch(str, pattern, options);
        }

        public static IEnumerable<Match> Matches(this string str, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.Matches(str, pattern, options).Cast<Match>();
        }

        public static string WithParams(this string format, params object[] args) {
            return string.Format(format, args);
        }

        public static string WithParams(this string format, IFormatProvider formatProvider, params object[] args) {
            return string.Format(formatProvider, format, args);
        }

        public static TY Parse<TY>(this string format, params object[] args) {
            var types = Provider.Domain;
            var reflector = Provider.Reflector;

            var arguments =
                args.AddTo(new List<object> {format})
                .ToArray();

            var isNullable = types.IsGeneric<TY>(typeof (Nullable<>));
            
            var ty = typeof (TY);

            var t = isNullable ? 
                    Nullable.GetUnderlyingType(ty) : 
                    ty;

            return
                arguments.Try(x =>
                    reflector
                    .InvokeStaticMember(t, "Parse", x)
                    .AsType<TY>()
                ).Catch<Exception>(
                    (x, ex) => types.GetDefault<TY>()
                ).Act();
        }

        public static bool EndsIn(this string format, params string[] suffixes) {
            return suffixes.Any(format.EndsWith);
        }

        public static string RemoveExtension(this string format) {
            var parts = format.Split('.');
            parts = parts.TakeWhile((s, i) => i < parts.LastIndex()).ToArray();
            
            return string.Join(".", parts);
        }
    }
}