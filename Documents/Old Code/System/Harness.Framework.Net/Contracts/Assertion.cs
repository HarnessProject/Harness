using Harness.Framework.Extensions;

namespace Harness.Framework.Contracts {
    public class Assertion {
        public Filter<object> Filter { get; set; }
        public string InvalidMessage { get; set; }
        public string ValidMessage { get; set; }

        public AssertionResult Assert(object val) {
            var valid = Filter(val);
            return new AssertionResult {
                Valid = valid,
                Message = valid ? ValidMessage : InvalidMessage
            };
        }

        public static Assertion IsString() {
            return new Assertion { Filter = (o) => o.Is<string>(), InvalidMessage = "is not a string." };
        }
    }
}