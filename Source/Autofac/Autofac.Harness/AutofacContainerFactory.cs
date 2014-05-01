#region ApacheLicense
// From the Harness Project
// Autofac.Harness
// Copyright © 2014 Nick Daniels, All Rights Reserved.
// 
// Licensed under the Apache License, Version 2.0 (the "License") with the following exception:
// 	Some source code is licensed under compatible licenses as required.
// 	See the attribution headers of the applicable source files for specific licensing terms.
// 
// You may not use this file except in compliance with its License(s).
// 
// You may obtain a copy of the Apache License, Version 2.0 at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Harness.Framework;
using Harness.Framework.Collections;
using Harness.Framework.Dependencies;
using Harness.Framework.Extensions;
using Harness.Framework.Interfaces;

namespace Autofac.Harness
{
    public class AutofacContainerFactory : IFactory<IContainer> {

        public AutofacDependencyProvider DependencyProvider { get; set; }

        public IDomainProvider TypeProvider { get; set; }

        public IContainer Create()
        {
            TypeProvider = Provider.Domain;
            var b =
                this.Try(x => Provider.State.$AutofacContainerBuilder.AsType<ContainerBuilder>())
                    .Catch<Exception>((x, ex) => null)
                    .Act();
            var container = CreateContainerBuilder(b);

            //If we obtained a builder from the State dictionary, 
            //DON'T BUILD IT.
            return b.NotNull() ? null : container.Build();
        }

        public ContainerBuilder CreateContainerBuilder(ContainerBuilder builder = null)
        {
            if (TypeProvider.IsNull()) return null;

            var builderContainer = new ContainerBuilder();

            var ts = TypeProvider.Types.ToArray();

            var iComponents = ts.Where(Requirements<IAttachToRegistration>);
            var iBuilders = ts.Where(Requirements<IRegisterDependencies>);
            var iModules = ts.Where(Requirements<IModule>);

            iComponents.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            iModules.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());
            iBuilders.Each(x => builderContainer.RegisterType(x).AsSelf().AsImplementedInterfaces());

            var container = builderContainer.Build();
            builder = builder ?? new ContainerBuilder();
            var context = new RegistrationContext();

            container
                .Resolve<IEnumerable<IAttachToRegistration>>()
                .Each(x => x.AttachToRegistration(context));


            container
                .Resolve<IEnumerable<IModule>>()
                .Each(x => builder.RegisterModule(x));

            container
                .Resolve<IEnumerable<IRegisterDependencies>>()
                .Each(x =>
                    x.Register(
                        TypeProvider,
                        new AutofacDependencyRegistrar(builder, TypeProvider)
                    ));

            var iDependencies = ts.Where(Requirements<IDependency>).ToArray();

            var suppressionAttributes =
                iDependencies.SelectMany(
                    x =>
                    {
                        var i = 0;
                        return x.GetCustomAttributes(typeof(SuppressDependencyAttribute), true)
                            .Cast<SuppressDependencyAttribute>()
                            .WhereNotDefault()
                            .Select(y => new RegisteredSuppression { OwnerType = x, SuppressionType = y.Type, ScoreSeed = i++ })
                            .OrderBy(y => y.Score);
                    }
                ).ToArray();

            iDependencies.Each(type =>
            {
                var suppresed = suppressionAttributes.Where(x => x.SuppressionType.Is(type));
                if (suppresed.FirstOrDefault(x => x.OwnerType != type).NotNull()) return;

                var register =
                    new AutofacDependencyRegistrar(builder, TypeProvider)
                    .Register(type)
                    .AsAny()
                    .InjectProperties(true);

                var handlers = context.HandlersFor(type);
                handlers.Each(x => x(register));
            });

            builder.Register<IDependencyProvider>(
                (c, p) =>
                    new AutofacDependencyProvider(
                        Provider.Dependencies
                        .AutofacContainer()
                        .BeginLifetimeScope()
                    )
            );

            builder.RegisterType<Scope>()
                   .AsImplementedInterfaces()
                   .InstancePerDependency();
            builder.RegisterInstance(TypeProvider)
                   .AsImplementedInterfaces()
                   .SingleInstance();

            container.Dispose();

            return builder;
        }

        public void Dispose()
        {

        }

        protected bool Requirements<T>(Type type)
        {
            return 
                Filter
                .If<Type>(x => x.Is<T>()) 
                .And(x => x.IsPublic)
                .AndNot(x => x.IsAbstract)
                .AndNot(x => x.IsInterface)
                .True(type);
        }
    }
}