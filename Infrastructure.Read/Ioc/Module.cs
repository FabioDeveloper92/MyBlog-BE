using Autofac;
using Infrastructure.Read.Post;
using Infrastructure.Read.User;

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

            builder.RegisterType<UserReadRepository>()
                   .As<IUserReadRepository>()
                   .SingleInstance();
        }
    }
}
