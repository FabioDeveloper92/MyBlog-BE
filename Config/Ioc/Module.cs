using System.Linq;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace Config.Ioc
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
            builder.Register(c =>
                   new Database
                   {
                       Name = _configuration.GetSection("Database:Name").Value
                   })
               .SingleInstance();

            builder.Register(c =>
                (MongoDBConnectionString)
                _configuration.GetConnectionString("MongoDBConnectionString")).SingleInstance();

        }
    }
}
