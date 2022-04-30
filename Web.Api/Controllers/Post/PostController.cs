using Application.Post.Queries;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Blog
{
    [Route("api/[controller]")]
    public class PostController
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<PostPublishedReadDto> Get(Guid id)
        {
            return await _mediator.Send(new GetPostPublished(id));
        }

    }
}
