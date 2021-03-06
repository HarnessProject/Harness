﻿using System.Composition.Dependencies;
using System.Portable.Runtime;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;

namespace System.Composition.Owin{
    public interface IApplication : IDependency  {
        
        string BasePath { get; }
        void Configure(IAppBuilder app);
    }

    public interface IMiddleware : ITransientDependency {
        Task Invoke(OwinHandlerContext context);
    }

    
}