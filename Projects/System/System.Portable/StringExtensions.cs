﻿#region ApacheLicense

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

using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#endregion

namespace System {
    public static class StringExtensions {
        public static bool IsMatch(this string str, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.IsMatch(str, pattern, options);
        }

        public static IEnumerable<Match> Matches(this string str, string pattern, RegexOptions options = RegexOptions.None) {
            return Regex.Matches(str, pattern, options).Cast<Match>();
        }

        public static string Format(this string format, params object[] args) {
            return String.Format(format, args);
        }
    }
}