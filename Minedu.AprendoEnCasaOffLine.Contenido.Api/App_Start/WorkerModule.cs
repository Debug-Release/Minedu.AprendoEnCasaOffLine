using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class WorkerModule : Autofac.Module
    {
        public static IConfiguration Configuration;

        protected override void Load(ContainerBuilder builder)
        {
            builder
               .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Worker")))
               .Where(t => t.Name.EndsWith("Service", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
               .As<IHostedService>()
               .InstancePerDependency();

        }
    }
}
