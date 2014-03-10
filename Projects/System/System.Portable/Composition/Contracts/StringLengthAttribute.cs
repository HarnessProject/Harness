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

using System.Contracts;
using System.Reflection;

#endregion

namespace System.Composition.Contracts {
    public class StringLengthAttribute : ContractPropertyAttribute {
        public StringLengthAttribute(int count) : base(
            new Assertion<object, PropertyInfo>((x, p) => x.Is<String>(), "is not a string"),
            new Assertion<object, PropertyInfo>((x, p) => x.As<String>().Length <= count, "is too long")
            ) {}
    }
}