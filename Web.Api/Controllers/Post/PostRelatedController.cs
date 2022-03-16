using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Blog
{
    [Route("api/[controller]")]
    [Authorize]
    public class PostRelatedController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostRelatedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<MyPostRelatedSimpleDto>> Get()
        {
           return await _mediator.Send(new Application.Post.Queries.GetMyPostRelatedSimple(User.Identity.Name));
        }

        [HttpPost]
        public async Task Post([FromBody] Models.Post.AddPostRelated item)
        {
            await _mediator.Send(new Application.Post.Commands.AddPostRelated(item.PostRelatedId, item.PostId));
        }
    }
}
