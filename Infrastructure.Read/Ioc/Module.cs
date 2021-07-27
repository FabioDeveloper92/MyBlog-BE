using Autofac;

namespace Infrastructure.Read.Ioc
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Core.Ioc.Module());

            //builder.RegisterType<TaskReadRepository>()
            //       .As<ITaskReadRepository>()
            //       .SingleInstance();
        }
    }
}
