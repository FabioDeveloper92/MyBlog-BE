using Infrastructure.Read.Post;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Application.Post.Queries;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Web.Api.Models.Post;
using Application.Post.Commands;

namespace Web.Api.Controllers.Post
{

    namespace Web.Api.Controllers.Blog
    {
        [Microsoft.AspNetCore.Mvc.Route("api/[controller]")]
        [Authorize]
        public class PostCreateController : ControllerBase
        {
            private readonly IMediator _mediator;

            public PostCreateController(IMediator mediator)
            {
                _mediator = mediator;
            }

            [HttpPost]
            public async Task<string> Post([FromBody] NewPost item)
            {
                var guidId = Guid.NewGuid();
                var createDate = DateTime.Now;

                DateTime? publishDate = null;
                if (item.ToPublished)
                    publishDate = DateTime.Now;

                await _mediator.Send(new CreatePost(guidId, item.Title, item.ImageThumb, item.ImageMain, item.Text, item.Tags, User.Identity.Name, createDate, createDate, publishDate));

                return guidId.ToString();
            }

        }
    }

}
