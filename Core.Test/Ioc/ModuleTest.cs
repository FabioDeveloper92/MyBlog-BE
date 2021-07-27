using System;
using Config.Ioc;
using Test.Common;
using Test.Common.Builders;
using Xunit;

namespace Infrastructure.Core.Test.Ioc
{
    public class ModuleTest : IDisposable
    {
        private readonly ScopeResolver _scopeResolver;

        public ModuleTest()
        {
            _scopeResolver = new ScopeResolver();

            var configBuilder = new ConfigBuilder();

            _scopeResolver.BuildContainer(
                new Module(configBuilder.Build()),
                new Core.Ioc.Module());
        }

        [Fact]
        public void should_resolve_IConnectionFactory()
        {
            _scopeResolver.IsInstancePerLifetimeScope<IMongoDbConnectionFactory, MongoDbConnectionFactory>();
        }

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
