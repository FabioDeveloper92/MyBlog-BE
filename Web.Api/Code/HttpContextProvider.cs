using Application.Code;
using Config;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Code
{
    public class HttpContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Admins _admins;
        public HttpContextProvider(IHttpContextAccessor httpContextAccessor, Admins admins)
        {
            _httpContextAccessor = httpContextAccessor;
            _admins = admins;
        }
        public string BuildUrl(string relativeUrl = null)
        {
            var scheme = _httpContextAccessor.HttpContext.Request.Scheme;
            var host = _httpContextAccessor.HttpContext.Request.Host.Value;

            var prefix = $"{scheme}://{host}";

            return !string.IsNullOrEmpty(relativeUrl) ? $"{prefix}/{relativeUrl}" : prefix;
        }

        public string TryGetEmail()
        {
            throw new System.NotImplementedException();
        }

        public string TryGetName()
        {
            throw new System.NotImplementedException();
        }
    }
}
