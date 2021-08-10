using Autofac;
using Infrastructure.Read.Post;

namespace Infrastructure.Read.Ioc
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Core.Ioc.Module());

            builder.RegisterType<PostReadRepository>()
                   .As<IPostReadRepository>()
                   .SingleInstance();
        }
    }
}
