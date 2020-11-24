using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Minedu.AprendoEnCasaOffLine.Contenido.Worker;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class WorkerModule : Autofac.Module
    {
        public static IConfiguration Configuration;

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ProgramacionService>()
                   .As<IHostedService>()
                   .InstancePerDependency();
        }
    }
}
