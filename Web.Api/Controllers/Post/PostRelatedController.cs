using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
        public async Task Post([FromBody] Models.Post.AddPostRelated item)
        {
            await _mediator.Send(new Application.Post.Commands.AddPostRelated(item.PostRelatedId, item.PostId));
        }
    }
}
