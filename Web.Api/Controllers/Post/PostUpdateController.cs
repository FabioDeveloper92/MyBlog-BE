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
    //[Authorize]
    public class PostUpdateController
    {
        private readonly IMediator _mediator;

        public PostUpdateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<PostUpdateReadDto> Get(Guid id)
        {
            return await _mediator.Send(new GetPostUpdate(id));
        }

        [HttpPut("{id}")]
        public async Task Put(Guid id, [FromBody] Models.Post.UpdatePost item)
        {
            var updateDate = DateTime.Now;

            DateTime? publishDate = null;

            if (item.ToPublished)
                publishDate = DateTime.Now;

            await _mediator.Send(new Application.Post.Commands.UpdatePost(id, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, item.Text, updateDate, publishDate));
        }

    }
}
