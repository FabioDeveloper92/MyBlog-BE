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
using Infrastructure.Write;
using Application.Post.Commands;

namespace Application.Test.PostTest.Commands
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

            BsonClassMapHelper.Clear();
            MongoDBInstallmentMap.Map();
        }

        [Fact]
        public async Task create_post_should_create_a_new_post()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder().WithDefaults().Build();

            //ACT
            await _sandbox.Mediator.Send(createPost);
        }

        [Fact]
        public void create_post_with_title_is_void_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithTitle("")
                .WithText("the name is void")
                .WithTags(new[] { 1 })
                .WithImageMain("imgUrlMain")
                .WithImageThumb("imgUrlThumb")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("Fabio")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_text_is_void_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("")
                .WithTags(new[] { 1 })
                .WithImageMain("imgUrlMain")
                .WithImageThumb("imgUrlThumb")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("Fabio")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_createby_is_void_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("abcdef ghiflmno")
                .WithTags(new[] { 1 })
                .WithImageMain("imgUrlMain")
                .WithImageThumb("imgUrlThumb")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_imageurl_is_void_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("this a test")
                .WithTags(new[] { 1 })
                .WithImageMain("")
                .WithImageThumb("imgUrlThumb")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("Fabio")
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public async Task create_post_should_create_a_new_post_with_multi_tags()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder().WithDefaults().WithTags(new int[] { 1, 2, 3 }).Build();

            //ACT
            await _sandbox.Mediator.Send(createPost);
        }

        [Fact]
        public void create_post_with_one_tag_is_invalid_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithDefaults()
                .WithTags(new[] { 1, 9999 })
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_tags_is_invalid_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithDefaults()
                .WithTags(new[] { 9991 })
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void create_post_with_tags_is_double_should_exception()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithDefaults()
                .WithTags(new[] { 1, 1, 2 })
                .Build();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(createPost); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public async void create_post_is_published()
        {
            //ARRANGE
            var createPost = new CreatePostBuilder()
                .WithTitle("title test")
                .WithText("abcdef ghiflmno")
                .WithTags(new[] { 1 })
                .WithImageMain("imgUrlMain")
                .WithImageThumb("imgUrlThumb")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("FAbio")
                .WithPublishDate(DateTime.Now)
                .Build();

            //ACT 
            await _sandbox.Mediator.Send(createPost);
        }

        [Fact]
        public async void update_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();
            var d = new DateTime(2021, 8, 16);
            var postCreateDate = DateTime.SpecifyKind(d, DateTimeKind.Utc);

            _sandbox.Scenario.WithPost(postId, "Post", "<b>Hello</b>", new int[] { 1 }, "myUrl", "myFakeUrl", postCreateDate, postCreateDate, null, "alfa", null);

            //ACT 
            await _sandbox.Mediator.Send(new UpdatePost(postId, "a", "a", "a", "b", new int[] { 0 }, "b", DateTime.Now, DateTime.Now));
        }

        [Fact]
        public async void add_comment_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost(postId);

            //ACT 
            await _sandbox.Mediator.Send(new AddPostComment(Guid.NewGuid(), postId, "Wow, it's wondeful", "admin@test.it", DateTime.Now));
        }

        [Fact]
        public void add_comment_post_not_exist_should_exception()
        {
            //ARRANGE
            _sandbox.Scenario.WithPost();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new AddPostComment(Guid.NewGuid(), Guid.NewGuid(), "Wow, it's wondeful", "admin", DateTime.Now)); };

            //ASSERT
            fn.Should().Throw<PostNotFoundException>();
        }

        [Fact]
        public void add_comment_without_username_should_exception()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost(postId);

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new AddPostComment(Guid.NewGuid(), postId, "Wow, it's wondeful", "", DateTime.Now)); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public void add_comment_without_text_should_exception()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost(postId);

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new AddPostComment(Guid.NewGuid(), postId, "", "admin@test.it", DateTime.Now)); };

            //ASSERT
            fn.Should().Throw<EmptyFieldException>();
        }

        [Fact]
        public async void add_post_related()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost(postId);

            //ACT 
            await _sandbox.Mediator.Send(new AddPostRelated(Guid.NewGuid(), postId));
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
