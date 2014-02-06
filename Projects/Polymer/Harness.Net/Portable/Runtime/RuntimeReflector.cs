using System.Portable.Runtime.Reflection;
using ImpromptuInterface;

namespace System.Portable.Runtime
{
    public class RuntimeReflector : IReflector
    {
        public object InvokeReturn(Delegate del, params object[] args) {
            return del.FastDynamicInvoke(args);
        }

        public void InvokeAction(Delegate del, params object[] args) {
            del.FastDynamicInvoke(args);
        }

        public object GetPropertyValue(object target, string property) {
            return Impromptu.InvokeGet(target, property);
        }

        public void SetPropertyValue(object target, string property, object value) {
            Impromptu.InvokeSet(target, property, value);
        }
    }
}
