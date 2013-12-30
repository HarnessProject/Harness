﻿using System.Composition;
using Caliburn.Micro;

/* Interfaces for our auto injection scheme... */

namespace Harness.Net.Caliburn.Micro {
    public interface IShell : IConductor, ISingletonDependency {} // App shell

    public interface IWindow : IScreen, IDependency {} // Window

    public interface IWindowContainer : IConductor, IDependency {} //Container

   
}