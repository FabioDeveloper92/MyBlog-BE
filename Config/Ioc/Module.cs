using System;
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
                 new Cors
                 {
                     Enabled = Convert.ToBoolean(_configuration.GetSection("Cors:Enabled").Value)
                 })
             .SingleInstance();

            builder.Register(c =>
                (MongoDBConnectionString)
                _configuration.GetConnectionString("MongoDBConnectionString")).SingleInstance();

            builder.Register(c =>
                new GoogleAuth()
                {
                    ClientId = _configuration.GetSection("Google:ClientId").Value,
                    ClientSecretId = _configuration.GetSection("Google:ClientSecretId").Value,
                })
             .SingleInstance();

        }
    }
}
