using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ImpromptuInterface;
using ImpromptuInterface.InvokeExt;

namespace System.Portable.Runtime {
    public class TypeProvider : ITypeProvider {
        private IEnumerable<Assembly> AssemblyCache { get; set; }
        private IEnumerable<Type> TypeCache { get; set; }
        public async Task<IEnumerable<Assembly>> GetAssemblies(string extensionsPath = null) {
            extensionsPath = extensionsPath ?? AppDomain.CurrentDomain.BaseDirectory;
            
            if (Directory.Exists(extensionsPath))
                await 
                Directory.EnumerateFiles(
                    extensionsPath, "*.dll", SearchOption.AllDirectories
                ).EachAsync(x => x.Try(Assembly.LoadFrom).Invoke());

            AssemblyCache = AppDomain.CurrentDomain.GetAssemblies().Where(x => !x.IsDynamic);
            return AssemblyCache;
        }

        
        public async Task<IEnumerable<Type>> GetTypesAsync(Func<Type, bool> predicate, string extensionsPath)
        {
            TypeCache = await TypeCache.IsNullAsync(async () =>
                (AssemblyCache.IsNull() ? await GetAssemblies(extensionsPath) : AssemblyCache)
                .SelectMany(x => x.Try(
                    y => predicate.NotNull() ? y.ExportedTypes.Where(predicate) : y.ExportedTypes
                ).Invoke()));
            
            return TypeCache;
        }

        public IEnumerable<Type> GetTypes(Func<Type, bool> predicate = null) {
            return GetTypesAsync(predicate, null).AwaitResult();
        }

        private TypeProvider() {
            GetTypes();
        }

        public TypeProvider(string extensionPath) {
            

            GetTypesAsync(null, extensionPath).Await();
        }

        public IEnumerable<Assembly> Assemblies { get { return AssemblyCache; } }
        public IEnumerable<Type> Types { get { return TypeCache; } }

        private static TypeProvider _provider;
        public static TypeProvider Instance {
            get {
                return _provider ?? (_provider = new TypeProvider());
            }
        }

        public object GetDefault(Type t)
        {
            
            Func<object> f = GetDefault<object>;
            return (object)Impromptu.InvokeMember(this, "GetDefault".WithGenericArgs(t));
        }

        public T GetDefault<T>()
        {
            return default(T);
        }

        public static IFactory<T> FactoryFor<T>(Action<IFactory<T>> initalizer = null, params object[] args)
        {
            var factoryType = Instance.Types.FirstOrDefault(x => x.Is<IFactory<T>>());
            if (factoryType.IsNull()) return Instance.GetDefault<IFactory<T>>();

            var factory = Impromptu.InvokeConstructor(factoryType, args).As<IFactory<T>>();

            initalizer.NotNull(x => factory.NotNull(x));

            return factory;
        }
    }

    public class DynamicObjectFactory<T> : IFactory<object>, IDependencyProvider
    {
        public Task<object> CreateAsync()
        {
            throw new NotImplementedException();
        }

        public object Create(params object[] args)
        {
            return Impromptu.InvokeConstructor(typeof (T), args);
        }

        public object Create()
        {
            return Create(new object[] {});
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public object GetService(Type serviceType)
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

        public IEnumerable<object> GetAll(Type serviceType)
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

        public IEnumerable<T> GetAll<T>()
        {
            throw new NotImplementedException();
        }
    }
}