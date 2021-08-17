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

        public Scenario And()
        {
            return this;
        }

    }
}
