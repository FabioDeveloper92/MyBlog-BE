using Application.Post.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Api.Models.Post;

namespace Web.Api.Controllers.Blog
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostCommentController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostCommentController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task Post([FromBody] NewPostComment item)
        {
            var guidId = Guid.NewGuid();
            var createDate = DateTime.Now;

            await _mediator.Send(new AddPostComment(guidId, item.PostId, item.Text, User.Identity.Name, createDate));
        }
    }
}
