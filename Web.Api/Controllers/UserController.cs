using Application.User.Commands;
using Application.User.Queries;
using Infrastructure.Read.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Web.Api.Code;
using Web.Api.Models.Auth;
using Web.Api.Models.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IJwtHandler _jwtHandler;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public UserController(IMediator mediator, IJwtHandler jwtHandler, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;

            _jwtHandler = jwtHandler;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] LoginUser item)
        {
            var appUser = await _userManager.FindByEmailAsync(item.Email);
            if (appUser == null)
            {
                throw new Exception();
            }

            var token = _jwtHandler.GenerateToken(appUser);

            return token;
        }

        [HttpGet]
        [Authorize]
        public async Task<UserReadDto> Get()
        {
            return await _mediator.Send(new GetUser(User.Identity.Name));
        }
    }
}
