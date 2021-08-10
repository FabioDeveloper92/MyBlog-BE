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
                .Build();

            _mediator.Send(createTask).Wait();

            return this;
        }

        public Scenario WithPost(Guid id, string title, string text)
        {
            var createTask = new CreatePostBuilder()
               .WithId(id)
               .WithTitle(title)
               .WithText(text)
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
