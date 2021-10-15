using Application.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using Web.Api.Models.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        public UserController(IMediator mediator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] NewUser item)
        {
            var internalToken = Guid.NewGuid().ToString();
            var expiredDate = DateTime.Now.AddDays(5);

            await _mediator.Send(new CreateOrUpdateUser(item.Name, item.Surname, item.Email, item.ExternalToken, item.LoginWith, internalToken, expiredDate));

            return internalToken;
        }
    }
}
