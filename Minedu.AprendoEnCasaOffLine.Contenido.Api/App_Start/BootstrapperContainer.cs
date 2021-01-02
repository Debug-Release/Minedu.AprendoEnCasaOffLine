using Autofac;
using Microsoft.Extensions.Configuration;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public static class BootstrapperContainer
    {

        public static void Register(ContainerBuilder builder, IConfiguration configuration)
        {
            //Add Context No SQL            
            builder.RegisterModule(new ContextDbNoSqlModule(configuration));

            //MediatR            
            builder.RegisterModule(new MediatRModule(configuration));

            //Fluent 
            builder.RegisterModule(new FluentModule(configuration));

            //Worker            
            builder.RegisterModule(new WorkerModule(configuration));
        }
    }
}
