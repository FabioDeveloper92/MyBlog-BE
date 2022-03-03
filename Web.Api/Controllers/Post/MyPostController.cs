using Application.Post.Queries;
using Infrastructure.Core.Enum;
using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Web.Api.Controllers.Post
{
    [Route("api/[controller]")]
    [Authorize]
    public class MyPostController:ControllerBase
    {
        private readonly IMediator _mediator;

        public MyPostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<List<PostMyOverviewReadDto>> Post([FromBody] Models.Post.FilterMyPost item)
        {
            return await _mediator.Send(new GetMyPostOverview(User.Identity.Name, item.Title, (FilterPostStatus)item.Status, (OrderPostDate)item.OrderByDate, item.Limit));
        }
    }
}
