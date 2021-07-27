using Autofac;
using Infrastructure.Write.Post;

namespace Infrastructure.Write.Ioc
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Core.Ioc.Module());

            builder.RegisterType<PostWriteRepository>()
                   .As<IPostWriteRepository>()
                   .SingleInstance();
            
            builder.RegisterType<PostWriteMapper>()
                   .As<IPostWriteMapper>()
                   .SingleInstance();
        }
    }
}
