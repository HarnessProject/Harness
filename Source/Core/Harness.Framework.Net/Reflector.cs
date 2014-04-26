using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using ImpromptuInterface;
using ImpromptuInterface.InvokeExt;

namespace Harness.Framework.Net
{
    public class Reflector : IReflector
    {
        public object Invoke(Delegate del, params object[] args) {
            return del.FastDynamicInvoke(args);
        }

        public void InvokeAction(Delegate del, params object[] args) {
            del.FastDynamicInvoke(args);
        }

        public void InvokeStaticMemberAction(Type target, string methodName, params object[] args) {
            Impromptu.InvokeMemberAction(new StaticContext(target), methodName, args);
        }

        public object GetPropertyValue(object target, string property) {
            return Impromptu.InvokeGet(target, property);
        }

        public void SetPropertyValue(object target, string property, object value) {
            Impromptu.InvokeSet(target, property, value);
        }

        public object CreateInstance(Type type, params object[] args) {
            return Impromptu.InvokeConstructor(type, args);
        }

        public IEnumerable<PropertyInfo> GetProperties(object target, Filter<PropertyInfo> filter) {
            return 
                target.GetType()
                .GetProperties()
                .Where(filter.AsFunc());
        }

        

        public void InvokeMemberAction(object target, string methodName, params object[] args) {
            Impromptu.InvokeMemberAction(target, methodName, args);
        }

        public object InvokeStaticMember(Type target, string methodName, params object[] args) {
            return Impromptu.InvokeMember(new StaticContext(target), methodName, args);
        }

        public object InvokeGenericMember(object target, string name, Type[] genericParameters, params object[] args) {
            return Impromptu.InvokeMember(target, name.WithGenericArgs(genericParameters), args);
        }

        public object InvokeMember(object target, string methodName, params object[] args) {
            return Impromptu.InvokeMember(target, methodName, args);
        }

        public TY Impersonate<TY>(object o) where TY : class {
            return o.ActLike<TY>();
        }

        public object InvokeStaticGenericMember(Type target, string name, Type[] genericParameters, params object[] args) {
            return Impromptu.InvokeMember(new StaticContext(target), name.WithGenericArgs(genericParameters), args);
        }

        private static Reflector _instance;

        public static Reflector Instance {
            get { return _instance ?? (_instance = new Reflector()); }
        }
    }

}
