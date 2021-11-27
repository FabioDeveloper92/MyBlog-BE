using System;
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

        public Scenario WithPost(Guid id, string title, string text, int[] tags, string imageMain, string imageThumb, DateTime createDate, DateTime updateDate, DateTime? publishDate, string createBy)
        {
            var createTask = new CreatePostBuilder()
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
               .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }

        public Scenario WithUser()
        {
            var createUser = new CreateGoogleUserBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Name")
                .WithSurname("Surname")
                .WithEmail("default@test.it")
                .WithExternalToken("externalToken")
                .WithInternalToken(Guid.NewGuid().ToString())
                .WithExpiredDate(DateTime.Now.AddDays(5))
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }

        public Scenario WithGoogleUser(Guid id, string name, string surname, string email, string externalToken, string internalToken, DateTime? expiredDate)
        {
            var createUser = new CreateGoogleUserBuilder()
                .WithId(id)
                .WithName(name)
                .WithSurname(surname)
                .WithEmail(email)
                .WithExternalToken(externalToken)
                .WithInternalToken(internalToken)
                .WithExpiredDate(expiredDate)
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }

        public Scenario WithJWTUser(Guid id, string name, string surname, string email, string password, string internalToken, DateTime? expiredDate)
        {
            var createUser = new CreateJwtUserBuilder()
                .WithId(id)
                .WithName(name)
                .WithSurname(surname)
                .WithEmail(email)
                .WithPassword(password)
                .WithInternalToken(internalToken)
                .WithExpiredDate(expiredDate)
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
