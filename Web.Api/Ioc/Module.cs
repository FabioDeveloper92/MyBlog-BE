using Application.Code;
using Autofac;
using Microsoft.Extensions.Configuration;
using Web.Api.Code;

namespace Web.Api.Ioc
{
    public class Module : Autofac.Module
    {
        private readonly IConfiguration _configuration;

        public Module(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Config.Ioc.Module(_configuration));
            builder.RegisterModule(new Application.Ioc.Module());

            builder.RegisterType<HttpContextProvider>().As<IContextProvider>();
        }
    }
}
