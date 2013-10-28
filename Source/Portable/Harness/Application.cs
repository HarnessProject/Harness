using System;
using System.Dynamic;

//using Caliburn.Micro;
//using NuGet;

namespace Harness {
    public static class Application {
        private static readonly ExpandoObject SessionStorage = new ExpandoObject();
        public static IApplication CurrentApplication { get; internal set; }
        public static IEnvironment CurrentEnvironment { get; internal set; }

        public static dynamic Session { get { return SessionStorage; } }

        public static T ApplicationAs<T>() where T : class, IApplication {
            return CurrentApplication as T;
        }

        public static T EnvironmentAs<T>() where T : class, IEnvironment {
            return CurrentEnvironment as T;
        }

        public static T Resolve<T>() {
            return CurrentEnvironment.Resolve<T>();
        }

        public static Object Resolve(Type type) {
            return CurrentEnvironment.Resolve(type);
        }

        public static Object Resolve(string typeName) {
            return CurrentEnvironment.Resolve(typeName);
        }
    }
}