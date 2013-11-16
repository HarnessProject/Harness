using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Harness.Framework;

namespace Harness {
    public abstract class EnvironmentBase<T> : IEnvironment {
        protected EnvironmentBase() { }

        public async Task InitializeAsync() {
            AssemblyCache = new List<Assembly>();
            TypeCache = new List<Type>();

            await GetAssemblies();
            await GetTypes();

            IsReady = true;
        }

        public void Initialize() {
            
            AssemblyCache = new List<Assembly>();
            TypeCache = new List<Type>();
            
            GetAssemblies().Await();
            GetTypes().Await();
            
            IsReady = true;
        }
        #region IEnvironment Members

      
        public IEnumerable<Assembly> AssemblyCache { get; protected set; }
        public IEnumerable<Type> TypeCache { get; protected set; }
        public bool IsReady { get; private set; }

        #endregion

        public abstract Task<IEnumerable<Assembly>> GetAssemblies(string extensionsPath = null);
        public abstract Task<IEnumerable<Type>> GetTypes(string extensionsPath = null);

        

        
    }
}