using System.Runtime.Environment;
using Autofac;

namespace Harness.Autofac {
    public interface IContainerBuilderService {
        void AttachToBuilder(ITypeProvider typeProvider, ContainerBuilder builder);
    }

    
}