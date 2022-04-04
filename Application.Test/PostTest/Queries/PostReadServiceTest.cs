using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Code;
using Application.Post.Queries;
using Autofac;
using Domain.Exceptions;
using FluentAssertions;
using Infrastructure.Core.Enum;
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

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostPublished(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);
            post.Comments.Should().BeEmpty();
            post.PostsRelatedCompleted.Should().BeEmpty();
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
                       .And().WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null)
                       .And().WithPost();

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostPublished(postId));

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
        public void create_two_post_and_return_zero_post_should_post_not_found_exception()
        {
            //ARRANGE
            var postId = Guid.NewGuid();

            _sandbox.Scenario.WithPost().And().WithPost();

            //ACT 
            Func<Task> fn = async () => { await _sandbox.Mediator.Send(new GetPostPublished(postId)); };

            //ASSERT
            fn.Should().Throw<PostNotFoundException>();
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

            _sandbox.Scenario.WithPost(postId1, postTitle1, postText1, postCategories1, postImageUrl1, postImageThumbUrl1, postCreateDate1, postCreateDate1, publishDate1, postCreateBy1, null)
                             .And()
                             .WithPost(postId2, postTitle2, postText2, postCategories2, postImageUrl2, postImageThumbUrl2, postCreateDate2, postCreateDate2, publishDate2, postCreateBy2, null);

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
            const string postTitle1 = "SQL Example";
            const string postText1 = "a b c d";
            var postCategories1 = new[] { 0 };
            const string postImageUrl1 = "url:code";
            const string postImageThumbUrl1 = "thumb//ecc";
            var d1 = new DateTime(2020, 8, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2021, 12, 12), DateTimeKind.Utc);
            const string postCreateBy1 = "FabioAdmin2";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "SQL Rel example";
            const string postText2 = "fake fake";
            var postCategories2 = new[] { 0, 1 };
            const string postImageUrl2 = "what";
            const string postImageThumbUrl2 = "when";
            var d2 = new DateTime(2021, 8, 21);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);
            var publishDate2 = DateTime.SpecifyKind(new DateTime(2022, 12, 3), DateTimeKind.Utc);
            const string postCreateBy2 = "FabioR";

            _sandbox.Scenario.WithPost(postId1, postTitle1, postText1, postCategories1, postImageUrl1, postImageThumbUrl1, postCreateDate1, postCreateDate1, publishDate1, postCreateBy1, null)
                             .And()
                             .WithPost(postId2, postTitle2, postText2, postCategories2, postImageUrl2, postImageThumbUrl2, postCreateDate2, postCreateDate2, publishDate2, postCreateBy2, null)
                             .And()
                             .WithPost();

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
            secondPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories2.Length).And.Contain(postCategories2);
        }

        [Fact]
        public async Task create_three_post_and_return_two_post_overview_published()
        {
            //ARRANGE
            var postId1 = Guid.NewGuid();
            const string postTitle1 = "My First Post";
            var postCategories1 = new[] { 0 };
            const string postImageThumbUrl1 = "myFirstUrlPostThumb";
            var d1 = new DateTime(2020, 7, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2099, 10, 25), DateTimeKind.Utc);
            const string postCreateBy1 = "FabioAdmin";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post";
            var postCategories2 = new[] { 0, 1 };
            const string postImageThumbUrl2 = "mySecondUrlThumb2";
            var d2 = new DateTime(2021, 8, 18);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);
            var publishDate2 = DateTime.SpecifyKind(new DateTime(2099, 12, 1), DateTimeKind.Utc);
            const string postCreateBy2 = "Fabio";

            _sandbox.Scenario.WithPost(postId1, postTitle1, "aaaa", postCategories1, "aaaa", postImageThumbUrl1, postCreateDate1, postCreateDate1, publishDate1, postCreateBy1, null)
                             .And()
                             .WithPost(postId2, postTitle2, "aaaa", postCategories2, "aaaa", postImageThumbUrl2, postCreateDate2, postCreateDate2, publishDate2, postCreateBy2, null)
                             .And()
                             .WithPost()
                             .And()
                             .WithPostComment(Guid.NewGuid(), postId1, "fake", "Test", publishDate1);

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetPostsOverview(3));

            //ASSERT
            var firstPost = posts.Single(p => p.Id == postId1);
            var secondPost = posts.Single(p => p.Id == postId2);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.ImageThumb.Should().Be(postImageThumbUrl1);
            firstPost.PublishDate.Should().NotBeNull().And.Be(publishDate1);
            firstPost.CreateBy.Should().Be(postCreateBy1);
            firstPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories1.Length).And.Contain(postCategories1);
            firstPost.CommentNumber.Should().Be(1);

            secondPost.Id.Should().Be(postId2);
            secondPost.Title.Should().Be(postTitle2);
            secondPost.ImageThumb.Should().Be(postImageThumbUrl2);
            secondPost.PublishDate.Should().NotBeNull().And.Be(publishDate2);
            secondPost.CreateBy.Should().Be(postCreateBy2);
            secondPost.Tags.Should().NotBeEmpty().And.HaveCount(postCategories2.Length).And.Contain(postCategories2);
            secondPost.CommentNumber.Should().Be(0);
        }

        [Fact]
        public async Task get_post_update_with_id_return_one_post()
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

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostUpdate(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageThumb.Should().Be(postImageThumbUrl);
            post.ImageMain.Should().Be(postImageUrl);
            post.CreateDate.Should().Be(postCreateDate);
            post.UpdateDate.Should().Be(postCreateDate);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);
        }

        [Fact]
        public async Task get_my_post_overview_return_two_post()
        {
            //ARRANGE
            var postId1 = Guid.NewGuid();
            const string postTitle1 = "My First Post Overview";
            var d1 = new DateTime(2022, 7, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2099, 10, 25), DateTimeKind.Utc);
            const string postCreateBy = "FabioAdmin";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post Overview";
            var d2 = new DateTime(2022, 8, 18);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);

            _sandbox.Scenario.WithPost(postId1, postTitle1, "desc1", new[] { 1, 2 }, "fistUrlT", "fistUrl", postCreateDate1, postCreateDate1, publishDate1, postCreateBy, null)
                             .And()
                             .WithPost(postId2, postTitle2, "desc2", new[] { 1 }, "secondUrlT", "secondUrl", postCreateDate2, postCreateDate2, null, postCreateBy, null);

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetMyPostOverview(postCreateBy, "Overview", FilterPostStatus.AllState, OrderPostDate.RecentlyCreate, 5));

            //ASSERT
            var firstPost = posts.Single(p => p.Id == postId1);
            var secondPost = posts.Single(p => p.Id == postId2);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.PublishDate.Should().NotBeNull().And.Be(publishDate1);
            firstPost.CreateDate.Should().Be(postCreateDate1);

            secondPost.Id.Should().Be(postId2);
            secondPost.Title.Should().Be(postTitle2);
            secondPost.PublishDate.Should().BeNull();
            secondPost.CreateDate.Should().Be(postCreateDate2);
        }

        [Fact]
        public async Task get_my_post_overview_create_two_with_different_user_return_one_post()
        {
            //ARRANGE
            var postId1 = Guid.NewGuid();
            const string postTitle1 = "My First Post Overview2";
            var d1 = new DateTime(2022, 7, 11);
            var postCreateDate1 = DateTime.SpecifyKind(d1, DateTimeKind.Utc);
            var publishDate1 = DateTime.SpecifyKind(new DateTime(2099, 10, 25), DateTimeKind.Utc);
            const string postCreateBy1 = "FabioAdmin";

            var postId2 = Guid.NewGuid();
            const string postTitle2 = "My Second Post Overview2";
            var d2 = new DateTime(2022, 8, 18);
            var postCreateDate2 = DateTime.SpecifyKind(d2, DateTimeKind.Utc);
            const string postCreateBy2 = "Fabio";

            _sandbox.Scenario.WithPost(postId1, postTitle1, "desc1", new[] { 1, 2 }, "fistUrlT", "fistUrl", postCreateDate1, postCreateDate1, publishDate1, postCreateBy1, null)
                             .And()
                             .WithPost(postId2, postTitle2, "desc2", new[] { 1 }, "secondUrlT", "secondUrl", postCreateDate2, postCreateDate2, null, postCreateBy2, null);

            //ACT
            var posts = await _sandbox.Mediator.Send(new GetMyPostOverview(postCreateBy1, "Overview2", FilterPostStatus.AllState, OrderPostDate.RecentlyCreate, 5));

            //ASSERT
            var firstPost = posts.Single(p => p.Id == postId1);

            firstPost.Id.Should().Be(postId1);
            firstPost.Title.Should().Be(postTitle1);
            firstPost.PublishDate.Should().NotBeNull().And.Be(publishDate1);
            firstPost.CreateDate.Should().Be(postCreateDate1);
        }

        [Fact]
        public async Task get_my_post_with_one_comment()
        {
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

            var postCommentId = Guid.NewGuid();
            var comment = "hey, how are you";
            var commentUsername = "billy";
            var postCommentDate = DateTime.SpecifyKind(d.AddHours(1), DateTimeKind.Utc);

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null)
                             .WithPostComment(postCommentId, postId, comment, commentUsername, postCommentDate);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostPublished(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);

            post.Comments.Count().Should().Be(1);
            var firstComment = post.Comments.Single(p => p.Id == postCommentId);
            firstComment.Username.Should().Be(commentUsername);
            firstComment.Text.Should().Be(comment);
            firstComment.CreateDate.Should().Be(postCommentDate);
        }

        [Fact]
        public async Task get_my_post_with_two_comment()
        {
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

            var postCommentId = Guid.NewGuid();
            var comment = "hey, how are you";
            var commentUsername = "billy";
            var postCommentDate = DateTime.SpecifyKind(d.AddHours(1), DateTimeKind.Utc);

            var postCommentId2 = Guid.NewGuid();
            var comment2 = "sorry for the delay";
            var commentUsername2 = "jake";
            var postCommentDate2 = DateTime.SpecifyKind(d.AddDays(1), DateTimeKind.Utc);

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null)
                             .WithPostComment(postCommentId, postId, comment, commentUsername, postCommentDate)
                             .WithPostComment(postCommentId2, postId, comment2, commentUsername2, postCommentDate2);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostPublished(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);

            post.Comments.Count().Should().Be(2);
            var firstComment = post.Comments.Single(p => p.Id == postCommentId);
            firstComment.Username.Should().Be(commentUsername);
            firstComment.Text.Should().Be(comment);
            firstComment.CreateDate.Should().Be(postCommentDate);

            var secondComment = post.Comments.Single(p => p.Id == postCommentId2);
            secondComment.Username.Should().Be(commentUsername2);
            secondComment.Text.Should().Be(comment2);
            secondComment.CreateDate.Should().Be(postCommentDate2);
        }

        [Fact]
        public async Task get_my_post_with_post_related()
        {
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

            var postCommentId = Guid.NewGuid();
            var comment = "hey, how are you";
            var commentUsername = "billy";
            var postCommentDate = DateTime.SpecifyKind(d.AddHours(1), DateTimeKind.Utc);

            var postRelatedId = Guid.NewGuid();
            var postRelatedTitle = "Related";
            var postRelatedImageThumb = "UrlRelated";
            var postRelatedIds = new List<Guid>() { postRelatedId };

            _sandbox.Scenario.WithPostToBeRelated(postRelatedId, postRelatedTitle, postRelatedImageThumb)
                             .WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, postRelatedIds)
                             .WithPostComment(postCommentId, postId, comment, commentUsername, postCommentDate);

            //ACT
            var post = await _sandbox.Mediator.Send(new GetPostPublished(postId));

            //ASSERT
            post.Id.Should().Be(postId);
            post.Title.Should().Be(postTitle);
            post.Text.Should().Be(postText);
            post.ImageMain.Should().Be(postImageUrl);
            post.PublishDate.Should().NotBeNull().And.Be(publishDate);
            post.CreateBy.Should().Be(postCreateBy);
            post.Tags.Should().NotBeEmpty().And.HaveCount(postCategories.Length).And.Contain(postCategories);

            post.Comments.Count().Should().Be(1);
            var firstComment = post.Comments.Single(p => p.Id == postCommentId);
            firstComment.Username.Should().Be(commentUsername);
            firstComment.Text.Should().Be(comment);
            firstComment.CreateDate.Should().Be(postCommentDate);

            post.PostsRelatedCompleted.Count().Should().Be(1);
            var postRelated = post.PostsRelatedCompleted.Single(x => x.Id == postRelatedId);
            postRelated.Title.Should().Be(postRelatedTitle);
            postRelated.ImageThumb.Should().Be(postRelatedImageThumb);
        }

        [Fact]
        public async Task get_my_post_can_be_post_related()
        {
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

            _sandbox.Scenario.WithPost(postId, postTitle, postText, postCategories, postImageUrl, postImageThumbUrl, postCreateDate, postCreateDate, publishDate, postCreateBy, null)
                             .WithPost()
                             .WithPost();

            //ACT
            var myPostsRelated = await _sandbox.Mediator.Send(new GetMyPostRelatedSimple(postCreateBy));

            var myFirstPostRelated = myPostsRelated.Single(p => p.Id == postId);
            myFirstPostRelated.Id.Should().Be(postId);
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
