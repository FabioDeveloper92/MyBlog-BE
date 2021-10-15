using System;
using Config.Ioc;
using Infrastructure.Read.Post;
using Infrastructure.Read.User;
using Test.Common;
using Test.Common.Builders;
using Xunit;


namespace Infrastructure.Read.Test.Ioc
{
    public class ModuleTest : IDisposable
    {
        private readonly ScopeResolver _scopeResolver;

        public ModuleTest()
        {
            _scopeResolver = new ScopeResolver();

            var configBuilder = new ConfigBuilder();

            _scopeResolver.BuildContainer(new Module(configBuilder.Build()),
                new Read.Ioc.Module());
        }

        [Fact]
        public void should_resolve_PostReadRepository()
        {
            _scopeResolver.IsSingleInstance<IPostReadRepository, PostReadRepository>();
        }

        [Fact]
        public void should_resolve_UserReadRepository()
        {
            _scopeResolver.IsSingleInstance<IUserReadRepository, UserReadRepository>();
        }

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
