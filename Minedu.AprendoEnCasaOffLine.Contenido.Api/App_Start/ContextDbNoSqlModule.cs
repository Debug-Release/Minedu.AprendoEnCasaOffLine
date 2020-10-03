using Autofac;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Release.Helper;
using Release.MongoDB.Repository;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class ContextDbNoSqlModule : Autofac.Module
    {
        public static IConfiguration Configuration;

        protected override void Load(ContainerBuilder builder)
        {
            string cs = Functions.GetEnvironmentVariable("ASPNETCORE_MONGODB");
            var mongoUrl = new MongoUrl(cs);
            /*
            var ms = MongoClientSettings.FromConnectionString(cs);
            ms.SslSettings = new SslSettings() { EnabledSslProtocols = SslProtocols.Tls12 };
            ms.RetryWrites = false;
            ms.ServerSelectionTimeout = new TimeSpan(0, 5, 0);
            ms.WaitQueueTimeout = new TimeSpan(0, 5, 0);
            ms.WaitQueueSize = 60000;
            */
            
            //builder.RegisterType<DataContext>().Named<IDataContext>("contextNoSql").WithParameter("settings", settings).InstancePerLifetimeScope();
            builder.RegisterType<DataContext>().Named<IDataContext>("contextNoSql").WithParameter("mongoUrl", mongoUrl).InstancePerLifetimeScope();
            /*
            //-> Aplicacion
            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.Name.EndsWith("Aplicacion", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsImplementedInterfaces();

            //-> Validacion
            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.Name.EndsWith("Validacion", StringComparison.Ordinal) && t.GetTypeInfo().IsClass)
                .AsSelf();
            */
            //Base
            builder.RegisterType<CollectionContext>().As<ICollectionContext>().WithParameter((c, p) => true, (c, p) => p.ResolveNamed<IDataContext>("contextNoSql"));

            BsonSerializer.RegisterSerializer(typeof(DateTime), new BsonUtcDateTimeSerializer());

            //Repository
            /*
            builder.RegisterAssemblyTypes(Assembly.Load(new AssemblyName("Minedu.AprendoEnCasaOffLine.Contenido.Core")))
                .Where(t => t.Name.EndsWith("Repositorio", StringComparison.Ordinal) && t.IsGenericType == false && t.IsClass == true && t.BaseType.Name.Contains("CustomBaseRepository"))
                .AsImplementedInterfaces()
                .WithParameter((c, p) => true, (c, p) => p.ResolveNamed<IDataContext>("contextNoSql"))
                .InstancePerLifetimeScope();
            */
            builder.RegisterGeneric(typeof(BaseRepository<>))
                .As(typeof(IBaseRepository<>))
                .WithParameter((c, p) => true, (c, p) => p.ResolveNamed<IDataContext>("contextNoSql"))
                .InstancePerDependency();

            /*
            builder.RegisterGeneric(typeof(CustomBaseRepository<>))
                .As(typeof(ICustomBaseRepository<>))
                .InstancePerDependency();
            */

        }

    }
}
