using System;
using Autofac;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Serialization;
using MediatR;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Serilog;
using Web.Api.Filters;
using Config;

namespace Web.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private Microsoft.AspNetCore.Hosting.IHostingEnvironment _env;
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
                .AddMvc(options =>
                    options.Filters.AddService(typeof(ApiExceptionFilter)))
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            services.AddMemoryCache();
            services.AddScoped<ApiExceptionFilter>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    var clientId = Configuration.GetSection("Google:ClientId");
                    var clientSecret = Configuration.GetSection("Google:ClientSecret");

                    options.ClientId = clientId.Value;
                    options.ClientSecret = clientSecret.Value;
                });
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
        public void Configure(IApplicationBuilder app, Microsoft.AspNetCore.Hosting.IApplicationLifetime applicationLifetime,
            Microsoft.AspNetCore.Hosting.IHostingEnvironment env, Cors cors, IMediator mediator)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            ConfigureLogger();
            ConfigureCors(app, cors);
            ConfigureFileServer(app);

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "api/{controller}");
            });

            applicationLifetime.ApplicationStopping.Register(OnShutdown);

            //try
            //{
            //    mediator.Send(new SynchronizeAdmins()).Wait();
            //}
            //catch (Exception e)
            //{
            //    Log.Information(
            //        "Cannot synchronize admins from appsettings. Please check if admin are inserted correctly");
            //}

            // temporary
        }

        private void ConfigureLogger()
        {
            var conf = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.RollingFile("logs/api-{Hour}.txt")
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
