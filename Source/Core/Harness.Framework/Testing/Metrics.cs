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
using System;

namespace Harness.Framework.Testing {
    public static class Metrics {
        /// <summary>
        ///     The amount of time between the
        ///     <param name="start">Start</param>
        ///     and the specified
        ///     <param name="end">Stop</param>
        /// </summary>
        /// <param name="end">The end of the duration</param>
        /// <param name="start">The start.</param>
        /// <returns></returns>
        public static TimeSpan Elapsed(this DateTime start, DateTime end) {
            return end.Subtract(start);
        }

        /// <summary>
        ///     Returns the duration of an action.
        /// </summary>
        /// <param name="action">an <see cref="Action" /></param>
        /// <returns></returns>
        public static TimeSpan TimeAction(Action action) {
            var start = DateTime.Now;
            action();
            var stop = DateTime.Now;
            return start.Elapsed(stop);
        }
    }
}