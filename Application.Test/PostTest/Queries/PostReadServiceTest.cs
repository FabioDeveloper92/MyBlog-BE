using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Code;
using Application.Post.Queries;
using Autofac;
using FluentAssertions;
using NSubstitute;
using Test.Common.Builders;
using Test.Infrastructure.Common;
using Xunit;

namespace Application.Test.TaskTest.Queries
{
    [Trait("Type", "Integration")]
    [Trait("Category", "Database")]
    [Collection("DropCreateDatabase Collection")]
    public class PostReadServiceTest : IDisposable
    {
        private readonly Sandbox _sandbox;
        private readonly IContextProvider _contextProvider;

        public PostReadServiceTest()
        {
            var configBuilder = new ConfigBuilder();

            _contextProvider = Substitute.For<IContextProvider>();

            _sandbox = new Sandbox(configBuilder.BuildModule(), new Application.Ioc.Module(), new MockedDotnetCoreModuleTest(), new MockModule(_contextProvider));
        }

        [Fact]
        public async Task get_post_with_id_return_one_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();
            const string postTitle = "My First Post";
            const string postText = "This is an example";

            _sandbox.Scenario.WithPost(postId, postTitle, postText);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPost(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
        }

        [Fact]
        public async Task create_three_and_return_one_post_with_id_return_one_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();
            const string postTitle = "My Second Post";
            const string postText = "This is a simple example";

            _sandbox.Scenario.WithPost().And().WithPost(postId, postTitle, postText).And().WithPost();

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPost(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
        }

        [Fact]
        public async Task create_two_post_and_return_zero_post_with_id_not_exist()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost().And().WithPost();

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPost(postId));

            //ASSERT
            post.Should().BeNull();
        }

        [Fact]
        public async Task create_two_post_and_return_get_all_post()
        {
            //ARRANGE
            var postId1 = Guid.NewGuid();
            const string postTitle1 = "My First Post";
            const string postText1 = "This is an example";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post";
            const string postText2 = "This is a fake post";

            _sandbox.Scenario.WithPost(postId1, postTitle1, postText1).And().WithPost(postId2, postTitle2, postText2);

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetPosts());

            //ASSERT
            var firstPost = posts.Single(p => p.Id == postId1);
            var secondPost = posts.Single(p => p.Id == postId2);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.Text.Should().Be(postText1);

            secondPost.Id.Should().Be(postId2);
            secondPost.Title.Should().Be(postTitle2);
            secondPost.Text.Should().Be(postText2);
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
