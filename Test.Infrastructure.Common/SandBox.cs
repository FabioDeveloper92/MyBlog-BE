using System;
using Autofac;
using Infrastructure.Core;
using MediatR;
using Test.Infrastructure.Common.Database;

namespace Test.Infrastructure.Common
{
    public class Sandbox : IDisposable
    {
        private readonly IContainer _container;

        public readonly IMediator Mediator;
        public readonly IMongoDbConnectionFactory mongoDbConnectionFactory;

        public Scenario.Scenario Scenario { get; }
        public FluentDbAssertion Db { get; }

        public Sandbox(params Module[] modules)
        {
            var builder = new ContainerBuilder();

            foreach (var module in modules)
                builder.RegisterModule(module);

            builder.RegisterType<Scenario.Scenario>()
                   .SingleInstance()
                   .PropertiesAutowired(PropertyWiringOptions.AllowCircularDependencies);

            _container = builder.Build();

            Mediator = _container.Resolve<IMediator>();
            mongoDbConnectionFactory = _container.Resolve<IMongoDbConnectionFactory>();

            Scenario = _container.Resolve<Scenario.Scenario>();
            Db = new FluentDbAssertion(mongoDbConnectionFactory.Connection);
        }

        public void Dispose()
        {
            _container?.Dispose();
        }
    }
}
