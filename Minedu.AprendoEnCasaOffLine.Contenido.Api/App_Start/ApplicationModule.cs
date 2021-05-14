using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class ApplicationModule : Autofac.Module
    {
        public IConfiguration _configuration;

        public ApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder
               .RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
               .Where(t => t.Name.EndsWith("Validation", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
               .AsSelf();

        }
    }
}
