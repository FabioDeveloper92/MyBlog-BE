using System;
using Application.Post.Commands;
using Application.Post.Queries;
using Application.User.Commands;
using Application.User.Queries;
using Config.Ioc;
using Test.Common;
using Test.Common.Builders;
using Xunit;

namespace Application.Test.Ioc
{
    public class ModuleTest : IDisposable
    {
        private readonly ScopeResolver _scopeResolver;

        public ModuleTest()
        {
            _scopeResolver = new ScopeResolver();

            var configBuilder = new ConfigBuilder();

            _scopeResolver.BuildContainer(new Module(configBuilder.Build()), new Application.Ioc.Module());
        }

        [Fact]
        public void should_resolve_PostWriteService()
        {
            _scopeResolver.IsInstancePerLifetimeScope<PostWriteService>();
        }

        [Fact]
        public void should_resolve_TaskReadService()
        {
            _scopeResolver.IsInstancePerLifetimeScope<PostReadService>();
        }

        [Fact]
        public void should_resolve_UserWriteService()
        {
            _scopeResolver.IsInstancePerLifetimeScope<UserWriteService>();
        }

        [Fact]
        public void should_resolve_UserReadService()
        {
            _scopeResolver.IsInstancePerLifetimeScope<UserReadService>();
        }

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
