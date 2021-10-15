using System.IO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ConfigController
    {
        private readonly ILogger _logger;

        public ConfigController(ILogger logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public Models.Config.Config Get()
        {
            var path = Path.GetFileName(@"version.info");
            var versionInfo = System.IO.File.ReadAllText(path);

            return new Models.Config.Config()
            {
                VersionInfo = versionInfo
            };
        }
    }
}
