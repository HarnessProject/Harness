using System;
using System.Collections.Generic;
using System.Dynamic;
//using Caliburn.Micro;
//using NuGet;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Harness.Events;
using Harness.Framework;

namespace Harness {
    public static class X  {
        private static readonly ExpandoObject SessionStorage = new ExpandoObject();
        public static IEnvironment Environment { get; private set; }
        public static IServiceLocator ServiceLocator { get; private set; }
        public static dynamic Session { get { return SessionStorage; } }
        public static IEnumerable<Type> Types { get { return Environment.TypeCache; } }
        public static IEnumerable<Assembly> Assemblies { get { return Environment.AssemblyCache; } }
        public static IScope GlobalScope { get; private set; }
        public static async Task InitializeAsync<T>(
            IApplicationFactory factory,
            IEnvironment environment
        ){
            Environment = environment;
           
            await factory.CreateAsync(
                environment
            );
            factory.Dispose();

            GlobalScope = new Scope {
                ServiceLocator = ServiceLocator,
                Environment = Environment, 
                Dispatcher = ServiceLocator.GetInstance<IDispatch>(), 
                EventManager = ServiceLocator.GetInstance<IEventManager>()
            };
        }

        public static void SetServiceLocator(IServiceLocator locator) {
            ServiceLocator = locator;
            Microsoft
                .Practices
                .ServiceLocation
                .ServiceLocator
                .SetLocatorProvider(() => ServiceLocator);
        }

        public static void Dispose() {

            GlobalScope.Dispose();
        }
    }

    

    public interface IApplicationFactory: IDisposable {
        Task CreateAsync(IEnvironment environment);
    }
}