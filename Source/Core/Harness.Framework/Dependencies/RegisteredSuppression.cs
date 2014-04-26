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
using System.Linq;
using Harness.Framework.Extensions;

#endregion

namespace Harness.Framework.Dependencies {
    public class RegisteredSuppression {
        public Type OwnerType { get; set; }
        public Type SuppressionType { get; set; }

        public int ScoreSeed { get; set; }

        public int Score {
            get {
                var implmnts = SuppressionType.IsInterface && OwnerType.GetInterfaces().Contains(SuppressionType);
                var isImmediateDecendant = OwnerType.BaseType == SuppressionType;
                var isDecendant = OwnerType.Is(SuppressionType);

                var i = 0;
                if (isImmediateDecendant) i++; //a point for supressing your immediate base type
                if (implmnts) i++; //a point for supressing all implementations of an interface you implement
                if (isDecendant) i++; //a point for supressing a type in your lineage

                return i + ScoreSeed; //usually it's ordinal appearence when reflecting the appdomain.
            }
        }
    }
}