using Application.Post.Queries;
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

        [HttpGet]
        public async Task<List<PostReadDto>> Get()
        {
            return await _mediator.Send(new GetPosts());
        }

        [HttpGet("{id}")]
        public async Task<PostReadDto> Get(Guid id)
        {
            return await _mediator.Send(new GetPost(id));
        }

    }
}
