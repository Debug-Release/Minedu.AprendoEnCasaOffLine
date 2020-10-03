using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
           .UseServiceProviderFactory(new AutofacServiceProviderFactory())
           .ConfigureLogging((hostingContext, builder) =>
           {
               string logPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Log", "Log-{Date}.txt");
               builder.AddFile(logPath);
           })
           .ConfigureAppConfiguration((hostBuilderContext, builder) =>
           {
               //builder.AddUserSecrets<Startup>();
           })
           .ConfigureWebHostDefaults(webBuilder =>
           {
               webBuilder.UseStartup<Startup>();
           });
    }
}
