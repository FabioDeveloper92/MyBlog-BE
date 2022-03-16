using Application.Post.Queries;
using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Blog
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostUpdateController : ControllerBase
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
        public async Task<string> Put(Guid id, [FromBody] Models.Post.NewPost item)
        {
            var updateDate = DateTime.Now;

            DateTime? publishDate = null;

            if (item.ToPublished)
                publishDate = DateTime.Now;

            var postsRelated = item.PostsRelated?.ToList();

            await _mediator.Send(new Application.Post.Commands.UpdatePost(id, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, User.Identity.Name, updateDate, publishDate, postsRelated));

            return id.ToString();
        }
    }
}
