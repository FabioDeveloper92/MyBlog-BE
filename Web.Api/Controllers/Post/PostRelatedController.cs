using Application.Post.Commands;
using Application.Post.Queries;
using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Models.Post;

namespace Web.Api.Controllers.Blog
{
    [Route("api/[controller]")]
    public class PostRelatedController 
    {
        private readonly IMediator _mediator;

        public PostRelatedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task Post([FromBody] AddPostRelated item)
        {
            await _mediator.Send(new AddPostRelated(item.PostId));
        }
    }
}
