﻿using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Test.Common.Builders
{
    public class ConfigBuilder
    {
        private readonly ConfigurationBuilder _configurationBuilder;

        public ConfigBuilder()
        {
            _configurationBuilder = new ConfigurationBuilder();

            var defaultConfig = new Dictionary<string, string>
            {
                {"Database:Name", "MyBlog_TEST"},
                {"ConnectionStrings:MongoDBConnectionString","mongodb://localhost:27017"},
                { "Cors:Enabled", "true" },
                {"Google:ClientId","clientId"},
                {"Google:ClientSecretId","clientSecretId"},
                {"JwtKeys:PrivateKey","key1"},
                {"JwtKeys:PublicKey","key2"}
            };

            Add(defaultConfig);
        }

        public ConfigBuilder Add(Dictionary<string, string> settings)
        {
            _configurationBuilder.AddInMemoryCollection(settings);
            return this;
        }

        public IConfiguration Build()
        {
            return _configurationBuilder.Build();
        }

        public Autofac.Module BuildModule()
        {
            return new Config.Ioc.Module(Build());
        }
    }
}
