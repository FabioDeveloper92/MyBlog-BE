using System;
using Config.Ioc;
using Infrastructure.Write.Post;
using Infrastructure.Write.User;
using Test.Common;
using Test.Common.Builders;
using Xunit;

namespace Infrastructure.Write.Test.Ioc
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
                new Write.Ioc.Module());
        }

        [Fact]
        public void should_resolve_IPostWriteRepository()
        {
            _scopeResolver.IsSingleInstance<IPostWriteRepository, PostWriteRepository>();
        }

        [Fact]
        public void should_resolve_IPostWriteMapper()
        {
            _scopeResolver.IsSingleInstance<IPostWriteMapper, PostWriteMapper>();
        }

        [Fact]
        public void should_resolve_IUserWriteRepository()
        {
            _scopeResolver.IsSingleInstance<IUserWriteRepository, UserWriteRepository>();
        }

        [Fact]
        public void should_resolve_IUserWriteMapper()
        {
            _scopeResolver.IsSingleInstance<IUserWriteMapper, UserWriteMapper>();
        }

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
