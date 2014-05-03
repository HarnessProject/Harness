using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Harness.Framework;
using Harness.Framework.Collections;
using Harness.Framework.Dependencies;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;
using TinyIoC;

namespace Caliburn.Micro.Harness.Win
{
    public class WinDependencyRegistration(Type type, TinyIoCContainer container, TinyIoCContainer.RegisterOptions options) : IDependencyRegistration
    {
        Type Type { get; } = type;
        TinyIoCContainer.RegisterOptions Options { get; } = options;
        TinyIoCContainer Container { get; } = container;

        public IDependencyRegistration As(Type type)
        {
            Container.Register(type, Type);
            return this;
        }

        public IDependencyRegistration As<T>()
        {
            Container.Register()
        }

        public IDependencyRegistration AsAncestors()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsAny()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsImplemented()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsSelf()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsSingleInScope()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsSingleton()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration AsTransient()
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration InjectProperties(bool preserveValues)
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration RegisterAsEach(IEnumerable<Type> types)
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration RegisterAsEach(params Type[] types)
        {
            throw new NotImplementedException();
        }
    }

    public class WinDependencyRegistrar(TinyIoCContainer container) : IDependencyRegistrar
    {
        TinyIoCContainer Container { get; } = container;
        public IDependencyRegistration FactoryFor<T>(Func<T> creator)
        {
            
            Container.Register(typeof(T), (container, overloads) => creator());
            return this;
        }

        public IDependencyRegistration Register(Type type)
        {
            throw new NotImplementedException();
        }

        public IDependencyRegistration Register<T>()
        {
            throw new NotImplementedException();
        }
    }

    public class WinDependencyResolver : IDependencyProvider, IFactory<IDependencyProvider>
    {
        TinyIoC.TinyIoCContainer container = new TinyIoC.TinyIoCContainer();

        protected bool Requirements<T>(TypeInfo type)
        {
            return
                Filter
                .If<TypeInfo>(x => x.AsType().Is<T>())
                .And(x => x.IsPublic)
                .AndNot(x => x.IsAbstract)
                .AndNot(x => x.IsInterface)
                .True(type);
                
        }

        public IDependencyProvider Create()
        {
            var domain = Provider.Domain;
            var registrar = new WinDependencyRegistrar(container);
            var types = domain.Types.Select(x => x.GetTypeInfo()).ToArray();

            var componentRegistrations = types.Where(Requirements<IRegisterDependencies>).ToArray();
            var registrationHandlers = types.Where(Requirements<IAttachToRegistration>).Select(t => t.AsType().CreateInstance().AsType<IAttachToRegistration>()).ToArray();
            var iDependencies = types.Where(Requirements<IDependency>).ToArray();

            var context = new RegistrationContext();
            registrationHandlers.Each(h => h.AttachToRegistration(context));



            return this;
        }

        public IScope CreateScope()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object Get(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public object Get(Type serviceType, string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>()
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<object> GetAll(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
        {
            throw new NotImplementedException();
        }

        public void InjectProperties(object o)
        {
            throw new NotImplementedException();
        }
    }
}
