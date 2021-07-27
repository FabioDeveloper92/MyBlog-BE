using Application.Code;
using Config;
using Microsoft.AspNetCore.Http;

namespace Web.Api.Code
{
    public class HttpContextProvider : IContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly Admins _admins;

        public string BuildUrl(string relativeUrl = null)
        {
            throw new System.NotImplementedException();
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
