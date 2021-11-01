using Application.User.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Threading.Tasks;
using Web.Api.Code;
using Web.Api.Models.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserJwtController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IJwtGenerator _jwtGenerator;
        public UserJwtController(IMediator mediator, IJwtGenerator jwtGenerator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] NewUser item)
        {
            var expiredDate = DateTime.Now.AddDays(5);
            var internalToken = _jwtGenerator.CreateUserAuthToken(item.Email, DateTime.Now.AddDays(5));

            await _mediator.Send(new CreateUserFromJwt(item.Name, item.Surname, item.Email, item.Password, item.ExternalToken, item.LoginWith, internalToken, expiredDate));

            return internalToken;
        }


    }
}
