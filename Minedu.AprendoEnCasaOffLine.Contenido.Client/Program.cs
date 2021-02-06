using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using MsContenido = Minedu.AprendoEnCasaOffLine.Contenido.Client.ContenidoClient;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var builder = new HostBuilder()
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureServices((context, services) =>
                {

                    services.AddLogging(configure => configure.AddConsole());
                    services.AddAplicacion(context.Configuration);
                });

                var host = builder.Build();

                using (var serviceScope = host.Services.CreateScope())
                {
                    var services = serviceScope.ServiceProvider;

                    try
                    {
                        var client = services.GetService<MsContenido.IClient>();
                        var servidores = client.ContenidoObtenerservidoresAsync().Result;

                        var dddd = client.ContenidoObtenerprogramaciontestAsync("esto hay").Result;

                        //var erere = client.ContenidoObtenercontenidoAsync(1, 10, 0, "", MsContenido.OrderPagedEnum._0).Result;




                        Console.WriteLine("Success");
                    }
                    catch (Exception ex)
                    {

                        Console.WriteLine(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
