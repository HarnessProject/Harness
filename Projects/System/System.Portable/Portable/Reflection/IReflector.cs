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

using System.Collections.Generic;
using System.Composition.Dependencies;
using System.Reflection;

#endregion

namespace System.Portable.Reflection {
    public interface IReflector : IDependency {
        object Invoke(Delegate del, params object[] args);
        void InvokeAction(Delegate del, params object[] args);
        object InvokeMember(object target, string methodName, params object[] args);
        object InvokeGenericMember(object target, string name, params Type[] genericParameters);
        void InvokeMemberAction(object target, string methodName, params object[] args);
        object InvokeStaticMember(Type target, string methodName, params object[] args);
        void InvokeStaticMemberAction(Type target, string methodName, params object[] args);
        object GetPropertyValue(object target, string property);
        void SetPropertyValue(object target, string property, object value);
        IEnumerable<PropertyInfo> GetProperties(object target, Filter<PropertyInfo> filter);
        object CreateInstance(Type type, params object[] args);
        TY Impersonate<TY>(object o) where TY : class;
    }
}