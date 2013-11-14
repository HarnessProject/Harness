using Autofac;

namespace Harness.Autofac {
    public interface IContainerBuilderService {
        void AttachToBuilder(IEnvironment environment, ContainerBuilder builder);
    }

    
}