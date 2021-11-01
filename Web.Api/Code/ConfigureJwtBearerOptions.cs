using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace Web.Api.Code
{
    // This class is needed to inject the JWT validation options including the public key
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        public Config.JwtKeys _jwtKeys;

        public ConfigureJwtBearerOptions(Config.JwtKeys jwtKeys)
        {
            _jwtKeys = jwtKeys;
        }
        public void Configure(string name, JwtBearerOptions options)
        {
            RSA rsa = RSA.Create();
            rsa.ImportSubjectPublicKeyInfo(Convert.FromBase64String(_jwtKeys.PublicKey), out _);

            options.IncludeErrorDetails = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuer = true,
                ValidIssuer = "AuthService",
                ValidateAudience = true,
                ValidAudience = "myApi",
                CryptoProviderFactory = new CryptoProviderFactory()
                {
                    CacheSignatureProviders = false
                }
            };
        }
        public void Configure(JwtBearerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
