﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Application.Code;
using Application.Post.Queries;
using Autofac;
using FluentAssertions;
using Infrastructure.Write;
using NSubstitute;
using Test.Common.Builders;
using Test.Infrastructure.Common;
using Xunit;

namespace Application.Test.PostTest.Queries
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

            BsonClassMapHelper.Clear();
            MongoDBInstallmentMap.Map();
        }

        [Fact]
        public async Task get_post_with_id_return_one_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();
            const string postTitle = "My First Post";
            const string postText = "This is an example";
            var postCategories = new int[1] { 0 };
            const string postImageUrl = "myUrl";
            const string postImageThumbUrl = "myUrl2";
            var d = new DateTime(2021, 8, 16);
            var postCreateDate = DateTime.SpecifyKind(d, DateTimeKind.Utc);
            var publishDate = DateTime.SpecifyKind(new DateTime(2021, 11, 27), DateTimeKind.Utc);
            const string postCreateBy = "Admin";

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPost(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);
        }

        [Fact]
        public async Task create_three_and_return_one_post_with_id_return_one_post()
        {
            //ARRANGE
            var postId = Guid.NewGuid();
            const string postTitle = "My Second Post";
            const string postText = "This is a simple example";
            var postCategories = new int[1] { 0 };
            const string postImageUrl = "fakeulr";
            const string postImageThumbUrl = "fakeurl2";
            var d = new DateTime(2021, 7, 10);
            var postCreateDate = DateTime.SpecifyKind(d, DateTimeKind.Utc);
            var publishDate = DateTime.SpecifyKind(new DateTime(2021, 11, 27), DateTimeKind.Utc);
            const string postCreateBy = "Fabio";

            _sandbox.Scenario.WithPost()
                       .And().WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy)
                       .And().WithPost();

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPost(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);
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
            var postCategories1 = new[] { 0 };
            const string postImageUrl1 = "myFirstUrlPost";
            const string postImageThumbUrl1 = "myFirstUrlPostThumb";
            var d1 = new DateTime(2020, 7, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2021, 10, 25), DateTimeKind.Utc);
            const string postCreateBy1 = "FabioAdmin";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post";
            const string postText2 = "This is a fake post";
            var postCategories2 = new[] { 0 };
            const string postImageUrl2 = "mySecondUrl";
            const string postImageThumbUrl2 = "mySecondUrlThumb2";
            var d2 = new DateTime(2021, 8, 18);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);
            var publishDate2 = DateTime.SpecifyKind(new DateTime(2022, 12, 1), DateTimeKind.Utc);
            const string postCreateBy2 = "Fabio";

            _sandbox.Scenario.WithPost(postId1, postTitle1, postText1, postCategories1, postImageUrl1, postImageThumbUrl1, postCreateDate1, postCreateDate1, publishDate1, postCreateBy1)
                             .And()
                             .WithPost(postId2, postTitle2, postText2, postCategories2, postImageUrl2, postImageThumbUrl2, postCreateDate2, postCreateDate2, publishDate2, postCreateBy2);

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetPosts());

            //ASSERT
            var firstPost = posts.Single(p => p.Id == postId1);
            var secondPost = posts.Single(p => p.Id == postId2);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.Text.Should().Be(postText1);
            firstPost.ImageMain.Should().Be(postImageUrl1);
            firstPost.PublishDate.Should().NotBeNull().And.Be(publishDate1);
            firstPost.CreateBy.Should().Be(postCreateBy1);
            firstPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories1.Length).And.Contain(postCategories1);

            secondPost.Id.Should().Be(postId2);
            secondPost.Title.Should().Be(postTitle2);
            secondPost.Text.Should().Be(postText2); ;
            secondPost.ImageMain.Should().Be(postImageUrl2);
            secondPost.PublishDate.Should().NotBeNull().And.Be(publishDate2);
            secondPost.CreateBy.Should().Be(postCreateBy2);
            secondPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories1.Length).And.Contain(postCategories1);
        }

        [Fact]
        public async Task create_three_post_and_return_two_post_published()
        {
            //ARRANGE
            var postId1 = Guid.NewGuid();
            const string postTitle1 = "My First Post";
            const string postText1 = "This is an example";
            var postCategories1 = new[] { 0 };
            const string postImageUrl1 = "myFirstUrlPost";
            const string postImageThumbUrl1 = "myFirstUrlPostThumb";
            var d1 = new DateTime(2020, 7, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2021, 10, 25), DateTimeKind.Utc);
            const string postCreateBy1 = "FabioAdmin";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post";
            const string postText2 = "This is a fake post";
            var postCategories2 = new[] { 0 };
            const string postImageUrl2 = "mySecondUrl";
            const string postImageThumbUrl2 = "mySecondUrlThumb2";
            var d2 = new DateTime(2021, 8, 18);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);
            var publishDate2 = DateTime.SpecifyKind(new DateTime(2022, 12, 1), DateTimeKind.Utc);
            const string postCreateBy2 = "Fabio";

            _sandbox.Scenario.WithPost(postId1, postTitle1, postText1, postCategories1, postImageUrl1, postImageThumbUrl1, postCreateDate1, postCreateDate1, publishDate1, postCreateBy1)
                             .And()
                             .WithPost(postId2, postTitle2, postText2, postCategories2, postImageUrl2, postImageThumbUrl2, postCreateDate2, postCreateDate2, publishDate2, postCreateBy2)
                             .And()
                             .WithPost();

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetPosts());

            //ASSERT
            posts.Should().HaveCount(2);

            var firstPost = posts.Single(p => p.Id == postId1);
            var secondPost = posts.Single(p => p.Id == postId2);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.Text.Should().Be(postText1);
            firstPost.ImageMain.Should().Be(postImageUrl1);
            firstPost.PublishDate.Should().NotBeNull().And.Be(publishDate1);
            firstPost.CreateBy.Should().Be(postCreateBy1);
            firstPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories1.Length).And.Contain(postCategories1);

            secondPost.Id.Should().Be(postId2);
            secondPost.Title.Should().Be(postTitle2);
            secondPost.Text.Should().Be(postText2); ;
            secondPost.ImageMain.Should().Be(postImageUrl2);
            secondPost.PublishDate.Should().NotBeNull().And.Be(publishDate2);
            secondPost.CreateBy.Should().Be(postCreateBy2);
            secondPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories1.Length).And.Contain(postCategories1);
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
