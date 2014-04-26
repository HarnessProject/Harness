using System.Composition;
using System.Composition.Dependencies;
using System.Web.Optimization;
using System.Web.Routing;

namespace System.Web.Mvc.Composition
{
    public interface IController : System.Web.Mvc.IController, IDependency
    {
        IScope LocalScope { get; set; }

    }
    //Routes, Filters
    public interface IRouteConstraint : System.Web.Routing.IRouteConstraint, IDependency { }

    public interface IRouteHandler : System.Web.Routing.IRouteHandler, IDependency { }

    public interface IAreaRouteHandler : IRouteWithArea, IDependency { }

    public interface IRouteProvider : IDependency
    {
        void AddRoutes(RouteCollection routes);
    }

    public interface IFilterProvider : IDependency
    {
        void AddFilters(GlobalFilterCollection filters);
    }

    

    public interface IBundleProvider : IDependency
    {
        void AddBundle(BundleCollection bundles);
    }
}
