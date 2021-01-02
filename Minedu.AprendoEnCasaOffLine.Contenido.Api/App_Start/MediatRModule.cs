using Autofac;
using MediatR;
using MediatR.Extensions.Autofac.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class MediatRModule : Autofac.Module
    {
        private IConfiguration _configuration;

        public MediatRModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            /*
            builder
                .RegisterType<Mediator>()
                .As<IMediator>()
                .InstancePerLifetimeScope();
            */
            builder.RegisterMediatR(typeof(Startup).Assembly);

            builder
                .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.IsClosedTypeOf(typeof(IRequest<>)))
                .AsImplementedInterfaces();


            builder
                .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .AsClosedTypesOf(typeof(IRequestHandler<,>));
            //.InstancePerDependency();
            //.Where(t => t.IsClosedTypeOf(typeof(IRequestHandler<,>)))
            //.AsImplementedInterfaces();                        

        }
    }
}
