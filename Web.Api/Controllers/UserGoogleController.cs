using Application.User.Commands;
using Application.User.Queries;
using Google.Apis.Auth;
using Infrastructure.Read.User;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Web.Api.Code;
using Web.Api.Exceptions;
using Web.Api.Models.User;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class UserGoogleController
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly IJwtGenerator _jwtGenerator;
        private readonly GoogleJsonWebSignature.ValidationSettings _settings;
        public UserGoogleController(IMediator mediator, Config.GoogleAuth googleAuth, IJwtGenerator jwtGenerator, ILogger logger)
        {
            _mediator = mediator;
            _logger = logger;

            _settings = new GoogleJsonWebSignature.ValidationSettings();
            _settings.Audience = new List<string>() { googleAuth.ClientId };

            _jwtGenerator = jwtGenerator;
        }

        [HttpPost]
        public async Task<string> Post([FromBody] NewUser item)
        {
            var payload = GoogleJsonWebSignature.ValidateAsync(item.ExternalToken, _settings).Result;
            if (payload == null)
                throw new InvalidTokenException();

            var expiredDate = DateTime.UtcNow.AddDays(5);
            var internalToken = _jwtGenerator.CreateUserAuthToken(item.Email, DateTime.UtcNow.AddDays(5));

            await _mediator.Send(new CreateOrUpdateUser(item.Name, item.Surname, item.Email, string.Empty, item.ExternalToken, item.LoginWith, internalToken, expiredDate));

            return internalToken;
        }
    }
}
