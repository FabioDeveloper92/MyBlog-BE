using System;

namespace Infrastructure.Read.Test.Ioc
{
    public class ModuleTest : IDisposable
    {
        //private readonly ScopeResolver _scopeResolver;

        public ModuleTest()
        {
            //_scopeResolver = new ScopeResolver();

            //var configBuilder = new ConfigBuilder();

            //_scopeResolver.BuildContainer(new Module(configBuilder.Build()),
            //    new Read.Ioc.Module());
        }

        //[Fact]
        //public void should_resolve_TaskReadService()
        //{
        //    _scopeResolver.IsSingleInstance<ITaskReadRepository, TaskReadRepository>();
        //}

        public void Dispose()
        {
            //_scopeResolver.Dispose();
        }
    }
}
