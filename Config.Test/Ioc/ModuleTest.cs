using System;
using Config.Ioc;
using FluentAssertions;
using Test.Common;
using Test.Common.Builders;
using Xunit;

namespace Config.Test.Ioc
{
    public class ModuleTest : IDisposable
    {
        private readonly ScopeResolver _scopeResolver;

        public ModuleTest()
        {
            _scopeResolver = new ScopeResolver();

            var configBuilder = new ConfigBuilder();

            _scopeResolver.BuildContainer(new Module(configBuilder.Build()));
        }

        [Fact]
        public void should_resolve_Database()
        {
            _scopeResolver.IsSingleInstance<Database>();
            _scopeResolver.Resolve<Database>().Name.Should().Be("MyBlog");
        }

        [Fact]
        public void should_resolve_SqlConnectionString()
        {
            _scopeResolver.IsSingleInstance<MongoDBConnectionString>();
            _scopeResolver.Resolve<MongoDBConnectionString>().Value.Should().Be("mongodb://localhost:27017");
        }

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
