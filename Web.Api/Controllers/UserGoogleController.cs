using Application.User.Commands;
using Google.Apis.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Web.Api.Code;
using Web.Api.Exceptions;
using Web.Api.Models.Auth;
using Web.Api.Models.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserGoogleController
    {
        private readonly IMediator _mediator;
        private readonly IJwtHandler _jwtHandler;
        private readonly GoogleJsonWebSignature.ValidationSettings _settings;
        private readonly UserManager<ApplicationUser> _userManager;
        public UserGoogleController(IMediator mediator, Config.GoogleAuth googleAuth, IJwtHandler jwtHandler, UserManager<ApplicationUser> userManager)
        {
            _mediator = mediator;

            _settings = new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new List<string>() { googleAuth.ClientId }
            };

            _jwtHandler = jwtHandler;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] NewUser item)
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(item.ExternalToken, _settings).Result;
            if (payload == null)
                throw new InvalidGoogleTokenException();

            var appUser = await _userManager.FindByEmailAsync(item.Email);
            if (appUser == null)
            {
                var createAppUser = new ApplicationUser
                {
                    UserName = item.Email,
                    Email = item.Email
                };

                var identityResult = await _userManager.CreateAsync(createAppUser, "Fabio_20");

                if (!identityResult.Succeeded)
                    throw new Exception();
            }

            var token = _jwtHandler.GenerateToken(appUser);

            await _mediator.Send(new CreateOrUpdateUser(item.Name, item.Surname, item.Email, string.Empty, item.LoginWith));

            return token;
        }
    }
}
