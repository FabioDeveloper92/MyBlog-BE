﻿using System;
using Application.Post.Commands;
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

        //[Fact]
        //public void should_resolve_TaskReadService()
        //{
        //    _scopeResolver.IsInstancePerLifetimeScope<TaskReadService>();
        //}

        public void Dispose()
        {
            _scopeResolver.Dispose();
        }
    }
}
