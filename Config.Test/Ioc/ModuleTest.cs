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
            _scopeResolver.Resolve<Database>().Name.Should().Be("MyBlog_TEST");
        }

        [Fact]
        public void should_resolve_SqlConnectionString()
        {
            _scopeResolver.IsSingleInstance<MongoDBConnectionString>();
            _scopeResolver.Resolve<MongoDBConnectionString>().Value.Should().Be("mongodb://localhost:27017");
        }

        [Fact]
        public void should_resolve_Cors()
        {
            _scopeResolver.IsSingleInstance<Cors>();
            _scopeResolver.Resolve<Cors>().Enabled.Should().BeTrue();
        }

        [Fact]
        public void should_resolve_GooogleAuth()
        {
            _scopeResolver.IsSingleInstance<GoogleAuth>();
            _scopeResolver.Resolve<GoogleAuth>().ClientId.Should().Be("clientId");
            _scopeResolver.Resolve<GoogleAuth>().ClientSecretId.Should().Be("clientSecretId");
        }

        [Fact]
        public void should_resolve_JwtKeys()
        {
            _scopeResolver.IsSingleInstance<JwtKeys>();
            _scopeResolver.Resolve<JwtKeys>().PrivateKey.Should().Be("key1");
            _scopeResolver.Resolve<JwtKeys>().PublicKey.Should().Be("key2");
        }
        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
