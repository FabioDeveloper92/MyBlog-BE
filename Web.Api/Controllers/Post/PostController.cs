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

        [HttpPost]
        public async Task<string> Post([FromBody] NewPost item)
        {
            var guidId = Guid.NewGuid();
            var createDate = DateTime.Now;

            DateTime? publishDate = null;
            if (item.ToPublished)
                publishDate = DateTime.Now;

            await _mediator.Send(new CreatePost(guidId, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, item.Text, createDate, createDate, publishDate));

            return guidId.ToString();
        }

    }
}
