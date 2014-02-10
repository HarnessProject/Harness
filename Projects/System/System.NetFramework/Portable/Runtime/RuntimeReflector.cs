using System.Collections.Generic;
using System.Linq;
using System.Portable.Reflection;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using ImpromptuInterface;

namespace System.Portable.Runtime
{
    public class RuntimeReflector : IReflector
    {
        public object Invoke(Delegate del, params object[] args) {
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

        public IEnumerable<PropertyInfo> GetProperties(object target, Filter<PropertyInfo> filter) {
            return target.GetType().GetProperties().Where(new Func<PropertyInfo, bool>(filter));
        }

        public void InvokeMemberAction(object target, string methodName, params object[] args) {
            Impromptu.InvokeMemberAction(target, methodName, args);
        }

        public object InvokeMember(object target, string methodName, params object[] args) {
            return Impromptu.InvokeMember(target, methodName, args);
        }

        public TY ActLike<TY>(object o) where TY : class {
            return o.ActLike<TY>();
        }
    }
}
