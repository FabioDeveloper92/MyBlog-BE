using System;
using Application.Code;
using Autofac;
using Domain.Exceptions;
using NSubstitute;
using Test.Common.Builders;
using Test.Common.Builders.Commands;
using Test.Infrastructure.Common;
using Xunit;
using System.Threading.Tasks;
using FluentAssertions;

namespace Application.Test.TaskTest.Commands
{
    [Trait("Type", "Integration")]
    [Trait("Category", "Database")]
    [Collection("DropCreateDatabase Collection")]
    public class PostWriteServiceTest : IDisposable
    {
        private readonly Sandbox _sandbox;
        private readonly IContextProvider _contextProvider;

        public PostWriteServiceTest()
        {
            var configBuilder = new ConfigBuilder();

            _contextProvider = Substitute.For<IContextProvider>();

            _sandbox = new Sandbox(configBuilder.BuildModule(), new Application.Ioc.Module(), new MockedDotnetCoreModuleTest(),
                new MockModule(_contextProvider));
        }

        [Fact]
        public async Task create_post_should_create_a_new_post()
        {
            //ARRANGE
            var createTask = new CreatePostBuilder().WithDefaults().Build();

            //ACT
            await _sandbox.Mediator.Send(createTask);
        }

        [Fact]
        public void create_post_with_title_is_void_should_exception()
        {
            //ARRANGE
            var createTask = new CreatePostBuilder()
                .WithTitle("")
                .WithText("the name is void")
                .WithCategory(0)
                .WithCreateBy("Fabio")
                .WithImageUrl("imageUrl")
                .WithCreateDate(DateTime.Now)
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createTask); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_text_is_void_should_exception()
        {
            //ARRANGE
            var createTask = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("")
                .WithCategory(0)
                .WithCreateBy("Fabio")
                .WithImageUrl("imageUrl")
                .WithCreateDate(DateTime.Now)
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createTask); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_createby_is_void_should_exception()
        {
            //ARRANGE
            var createTask = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("abcdef ghiflmno")
                .WithCategory(0)
                .WithCreateBy("")
                .WithImageUrl("imageUrl")
                .WithCreateDate(DateTime.Now)
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createTask); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_imageurl_is_void_should_exception()
        {
            //ARRANGE
            var createTask = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("this a test")
                .WithCategory(0)
                .WithCreateBy("Fabio")
                .WithImageUrl("")
                .WithCreateDate(DateTime.Now)
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createTask); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        public void Dispose()
        {
            _sandbox?.Dispose();
        }

        private class MockModule : Autofac.Module
        {
            private readonly IContextProvider _contextProvider;

            public MockModule(IContextProvider contextProvider)
            {
                _contextProvider = contextProvider;
            }

            protected override void Load(ContainerBuilder builder)
            {
                builder.Register(ctx => _contextProvider).As<IContextProvider>().SingleInstance();
            }
        }
    }
}
