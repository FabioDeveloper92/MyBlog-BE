using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Web.Api.Code
{
    public interface IJwtGenerator
    {
        string CreateUserAuthToken(string userId, DateTime expiredDate);
    }

    public class JwtGenerator : IJwtGenerator
    {
        readonly RsaSecurityKey _key;
        public JwtGenerator(Config.JwtKeys jwtKeys)
        {
            RSA privateRSA = RSA.Create();
            privateRSA.ImportEncryptedPkcs8PrivateKey("", Convert.FromBase64String(jwtKeys.PrivateKey), out _);
            _key = new RsaSecurityKey(privateRSA);
        }

        public string CreateUserAuthToken(string userId, DateTime expiredDate)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = "myApi",
                Issuer = "AuthService",
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, userId.ToString())
                }),
                Expires = expiredDate,
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.RsaSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
