using System;
using System.Collections.Generic;
using MediatR;
using Test.Common.Builders.Commands;

namespace Test.Infrastructure.Common.Scenario
{
    public class Scenario
    {
        private readonly IMediator _mediator;

        public Scenario(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Scenario WithPost()
        {
            var createTask = new CreatePostBuilder()
                .WithId(Guid.NewGuid())
                .WithTitle("Default Name")
                .WithText("Default Description")
                .WithTags(new[] { 1 })
                .WithImageMain("imgMainUrl")
                .WithImageThumb("imgThumbUrl")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("Fabio")
                .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }

        public Scenario WithPost(Guid postId)
        {
            var createTask = new CreatePostBuilder()
                .WithId(postId)
                .WithTitle("Default Name")
                .WithText("Default Description")
                .WithTags(new[] { 1 })
                .WithImageMain("imgMainUrl")
                .WithImageThumb("imgThumbUrl")
                .WithCreateDate(DateTime.Now)
                .WithUpdateDate(DateTime.Now)
                .WithPublishDate(null)
                .WithCreateBy("Fabio")
                .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }


        public Scenario WithPost(Guid id, string title, string text, int[] tags, string imageMain, string imageThumb, DateTime createDate, DateTime updateDate, DateTime? publishDate, string createBy, List<Guid> postsRelated)
        {
            var createPost = new CreatePostBuilder()
               .WithId(id)
               .WithTitle(title)
               .WithText(text)
               .WithTags(tags)
               .WithImageMain(imageMain)
               .WithImageThumb(imageThumb)
               .WithCreateDate(createDate)
               .WithUpdateDate(updateDate)
               .WithPublishDate(publishDate)
               .WithCreateBy(createBy)
               .WithPostRelated(postsRelated)
               .Build();

            _mediator.Send(createPost).Wait();

            return this;
        }

        public Scenario WithPostComment(Guid postCommentId, Guid postId, string text, string username, DateTime createDate)
        {
            var createPostComment = new CreatePostCommentBuilder()
                .WithPostCommentId(postCommentId)
                .WithPostId(postId)
                .WithText(text)
                .WithUsername(username)
                .WithCreateDate(createDate)
                .Build();

            _mediator.Send(createPostComment).Wait();

            return this;
        }

        public Scenario WithUser()
        {
            var createUser = new CreateGoogleUserBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Name")
                .WithSurname("Surname")
                .WithEmail("default@test.it")
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }

        public Scenario WithGoogleUser(Guid id, string name, string surname, string email)
        {
            var createUser = new CreateGoogleUserBuilder()
                .WithId(id)
                .WithName(name)
                .WithSurname(surname)
                .WithEmail(email)
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }

        public Scenario WithJWTUser(Guid id, string name, string surname, string email, string password)
        {
            var createUser = new CreateJwtUserBuilder()
                .WithId(id)
                .WithName(name)
                .WithSurname(surname)
                .WithEmail(email)
                .WithPassword(password)
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }
        public Scenario And()
        {
            return this;
        }

    }
}
