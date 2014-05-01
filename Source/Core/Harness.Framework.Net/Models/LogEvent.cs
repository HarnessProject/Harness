#region ApacheLicense
// From the Harness Project
// Harness.Framework.Net
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Harness.Framework.Models
{
    public class LogEventModel : Model<LogEvent, Guid> { 
        
        public override void Define(ModelDefinition<LogEvent, Guid> defineModel)
        {
            defineModel(
                keysEqual: (k1, k2) => k1 == k2,
                defaultKey: () => Guid.NewGuid(),
                newModel: () => new LogEvent { Id = DefaultKey() }
            );
        }

        public virtual IEnumerable<LogEvent> Ancestors()
        {
            
            return EmptyCollection();
        }

        public virtual IEnumerable<LogEvent> Children()
        {
            return EmptyCollection();
        }
    }

    public class LogEvent (
        string source = "Null",
        string target = "Null",
        string message = "A logable event has occurred",
        DateTime? timestamp = null, 
        Exception ex = null)
    {
        public Guid Id { get; set; }
        public string ExceptionName { get; set; } = (ex ?? new Exception()).ToString();

        public string Message { get; set; } = message;

        public string Source { get; set; } = source;

        public string Target { get; set; } = target;

        public DateTime Timestamp { get; set; } = timestamp.HasValue ? timestamp.Value : DateTime.Now;
    }
}
