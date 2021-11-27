using Application.User.Commands;
using Application.User.Queries;
using Infrastructure.Read.User;
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
    public class UserController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IJwtGenerator _jwtGenerator;
        public UserController(IMediator mediator, IJwtGenerator jwtGenerator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;

            _jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] LoginUser item)
        {
            var expiredDate = DateTime.Now.AddDays(5);
            var internalToken = _jwtGenerator.CreateUserAuthToken(item.Email, DateTime.Now.AddDays(5));

            await _mediator.Send(new UpdateUserTokenJwt(item.Email, item.Password, internalToken, expiredDate));

            return internalToken;
        }

        [HttpGet("{token}")]
        public async Task<UserReadDto> Get(string token)
        {
            return await _mediator.Send(new GetUser(token));
        }

        [HttpDelete("{token}")]
        public async Task Delete(string token)
        {
            await _mediator.Send(new LogoutUser(token));
        }
    }
}
