
using Application.Post.Queries;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Post
{
    [Route("api/[controller]")]
    public class PostOverviewController
    {
        private readonly IMediator _mediator;

        public PostOverviewController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<List<PostOverviewReadDto>> Post([FromBody] Models.Post.FilterAllPosts item)
        {
            return await _mediator.Send(new GetPostsOverview(item.Limit, (FilterByTime)item.FilterByTime, (OrderByVisibility)item.OrderByVisibility));
        }
    }
}
