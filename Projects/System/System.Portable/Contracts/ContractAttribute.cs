﻿#region ApacheLicense

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

#endregion

namespace System.Contracts {
    public abstract class ContractAttribute : Attribute {
        protected IList<Assertion> Assertions;
        protected ValidationResults Assert(object v, string name, params object[] args) {
            var results = new ValidationResults();
            Assertions.Select(x => v.Assert(name, x, args)).AddTo(results);
            return results;
        }
    }
}