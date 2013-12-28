using Caliburn.Micro;

/* Interfaces for our auto injection scheme... */
namespace Harness.Silverlight.CaliburnMicro {
    public interface IShell : IConductor, ISingletonDependency { } // App shell
    public interface IWindow : IScreen, IDependency { } // Window
    
    public interface IWindowContainer : IConductor, IDependency { } //Container
    public interface IColumn : IScreen, IDependency { } // Column in a container 
}

