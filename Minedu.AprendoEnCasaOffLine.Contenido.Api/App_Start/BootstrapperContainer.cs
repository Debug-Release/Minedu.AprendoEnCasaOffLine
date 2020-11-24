using Autofac;
using Microsoft.Extensions.Configuration;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public static class BootstrapperContainer
    {
        public static IConfiguration Configuration;

        public static void Register(ContainerBuilder builder)
        {
            //Add Context No SQL
            ContextDbNoSqlModule.Configuration = Configuration;
            builder.RegisterModule<ContextDbNoSqlModule>();

            //MediatR
            ContextDbNoSqlModule.Configuration = Configuration;
            builder.RegisterModule<MediatRModule>();

            //Worker
            ContextDbNoSqlModule.Configuration = Configuration;
            builder.RegisterModule<WorkerModule>();
        }
    }
}
