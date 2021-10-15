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
                .WithCategory(0)
                .WithImageUrl("urlfake")
                .WithCreateDate(DateTime.Now)
                .WithCreateBy("Fabio")
                .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }

        public Scenario WithPost(Guid id, string title, string text, int category, string imageUrl, DateTime createDate, string createBy)
        {
            var createTask = new CreatePostBuilder()
               .WithId(id)
               .WithTitle(title)
               .WithText(text)
               .WithCategory(category)
               .WithImageUrl(imageUrl)
               .WithCreateDate(createDate)
               .WithCreateBy(createBy)
               .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }

        public Scenario WithUser()
        {
            var createUser = new CreateUserBuilder()
                .WithId(Guid.NewGuid())
                .WithName("Name")
                .WithSurname("Surname")
                .WithEmail("default@test.it")
                .WithExternalToken("externalToken")
                .WithLoginWith(Domain.LoginProvider.Google)
                .WithInternalToken(Guid.NewGuid().ToString())
                .WithExpiredDate(DateTime.Now.AddDays(5))
                .Build();

            _mediator.Send(createUser).Wait();

            return this;
        }

        public Scenario WithUser(Guid id, string name, string surname, string email, string externalToken, Domain.LoginProvider loginWith, string internalToken, DateTime? expiredDate)
        {
            var createUser = new CreateUserBuilder()
                .WithId(id)
                .WithName(name)
                .WithSurname(surname)
                .WithEmail(email)
                .WithExternalToken(externalToken)
                .WithLoginWith(loginWith)
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
