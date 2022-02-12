
using Application.Post.Queries;
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

        [HttpGet("{limit}")]
        public async Task<List<PostOverviewReadDto>> Get(int limit)
        {
            return await _mediator.Send(new GetPostsOverview(limit));
        }
    }
}
