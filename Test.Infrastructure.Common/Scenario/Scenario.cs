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

        //public Scenario WithTask()
        //{
        //    var createTask = new CreateTaskBuilder()
        //        .WithId(Guid.NewGuid())
        //        .WithName("Default Name")
        //        .WithDescription("Default Description")
        //        .WithUserId(Guid.NewGuid())
        //        .Build();

        //    _mediator.Send(createTask).Wait();

        //    return this;
        //}

        //public Scenario WithTask(Guid id, string name, string description, Guid userId)
        //{
        //    var createTask = new CreateTaskBuilder()
        //        .WithId(id)
        //        .WithName(name)
        //        .WithDescription(description)
        //        .WithUserId(userId)
        //        .Build();

        //    _mediator.Send(createTask).Wait();

        //    return this;
        //}

        //public Scenario And()
        //{
        //    return this;
        //}
    
    }
}
