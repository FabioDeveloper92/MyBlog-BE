using Application.Post.Commands;
using Autofac;
using AutofacSerilogIntegration;
using MediatR;

namespace Application.Ioc
{
    public class Module : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterModule(new Infrastructure.Write.Ioc.Module());
            //builder.RegisterModule(new Infrastructure.Read.Ioc.Module());

            builder.RegisterLogger();

            RegisterMediatr(builder);

            builder.RegisterType<PostWriteService>()
                   .AsSelf()
                   .InstancePerLifetimeScope();
        }

        private void RegisterMediatr(ContainerBuilder builder)
        {
            //            https://github.com/jbogard/MediatR/issues/128
            //            builder
            //                .RegisterSource(new ContravariantRegistrationSource());
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();

            builder
                .Register<ServiceFactory>(ctx =>
                {
                    var c = ctx.Resolve<IComponentContext>();
                    return t => c.TryResolve(t, out var o) ? o : null;
                })
                .InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(Module).Assembly).AsImplementedInterfaces();
        }
    }
}
