using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Serilog;
using Web.Api.Filters;
using Config;
using Infrastructure.Write;
using System.Text.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Web.Api.Models.Auth;
using System;
using System.Text;

namespace Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private Microsoft.AspNetCore.Hosting.IWebHostEnvironment _env;
        private TelemetryClient _telemetryClient;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new Ioc.Module(Configuration));
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddMvc(options => options.Filters.AddService(typeof(ApiExceptionFilter)))
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.DictionaryKeyPolicy = JsonNamingPolicy.CamelCase;
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;


                    //options.JsonSerializerOptions.loop = ReferenceLoopHandling.Ignore;
                });


            // FABIO
            services.AddIdentity<ApplicationUser, ApplicationRole>()
                    .AddMongoDbStores<ApplicationUser, ApplicationRole, Guid>
                    (
                        Configuration.GetSection("ConnectionStrings:MongoDBConnectionString").Value, Configuration.GetSection("Database:Name").Value
                    );

            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration.GetSection("JWTSettings:validIssuer").Value,
                    ValidAudience = Configuration.GetSection("JWTSettings:validAudience").Value,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("JWTSettings:securityKey").Value))
                };
            });

            // FABIO


            services.AddMemoryCache();
            services.AddScoped<ApiExceptionFilter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            MongoDBInstallmentMap.Map();
        }

        private static void ConfigureCors(IApplicationBuilder app, Cors cors)
        {
            if (cors.Enabled)
            {
                app.UseCors(builder => builder
                    .WithOrigins("http://localhost:4200")
                    .AllowCredentials()
                    .WithHeaders("authorization", "content-type", "cache-control", "expires", "if-modified-since", "pragma")
                    .WithMethods("GET", "POST", "PUT", "DELETE"));
            }
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostApplicationLifetime applicationLifetime,
            Microsoft.AspNetCore.Hosting.IWebHostEnvironment env, Cors cors, IMediator mediator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureLogger();
            ConfigureCors(app, cors);
            ConfigureFileServer(app);

            app.UseRouting()
               .UseAuthentication()
               .UseAuthorization()
               .UseEndpoints(endpoints =>
               {
                   endpoints.MapControllerRoute("default", "{controller}");
               });

            applicationLifetime.ApplicationStopping.Register(OnShutdown);
        }

        private void ConfigureLogger()
        {
            var conf = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.File("logs/api-{Hour}.txt")
                .WriteTo.Console();

            Log.Logger = conf.CreateLogger();
        }

        private void ConfigureFileServer(IApplicationBuilder app)
        {
            app.Use(async (context, next) =>
            {
                await next();

                if (context.Response.StatusCode == 404 && !context.Request.Path.Value.StartsWith("/api") &&
                    !System.IO.Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });

            app.UseFileServer(new FileServerOptions()
            {
                StaticFileOptions =
                {
                    OnPrepareResponse = ctx =>
                    {
                        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
                    }
                }
            });
        }

        private void OnShutdown()
        {
            _telemetryClient?.Flush();
            Log.CloseAndFlush();
        }
    }
}
