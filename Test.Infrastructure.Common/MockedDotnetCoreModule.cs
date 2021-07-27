using Autofac;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using NSubstitute;

namespace Test.Infrastructure.Common
{
    /// <summary>
    /// Register some types that are automatically registered by .net core
    /// </summary>
    public class MockedDotnetCoreModuleTest : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(ctx => Substitute.For<IDistributedCache>()).As<IDistributedCache>();
            builder.Register(ctx => Substitute.For<IDataProtectionProvider>())
                .As<IDataProtectionProvider>();
            builder.Register(ctx => Substitute.For<IHttpContextAccessor>()).As<IHttpContextAccessor>();
            builder.Register(ctx => Substitute.For<IMemoryCache>()).As<IMemoryCache>();
        }
    }
}
