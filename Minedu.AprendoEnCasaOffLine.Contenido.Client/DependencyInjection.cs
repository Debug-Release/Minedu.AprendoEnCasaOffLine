using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MsContenido = Minedu.AprendoEnCasaOffLine.Contenido.Client.ContenidoClient;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Client
{
    public static class DependencyInjection
    {


        public static IServiceCollection AddAplicacion(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var settings = new AppSettings();
            configuration.GetSection("AppSettings").Bind(settings);
            settings.ContenidoClient = settings.ContenidoClient.TrimEnd('/');
            services.AddSingleton(settings);


            services.AddHttpClient("MsContenido", client =>
            {
                client.BaseAddress = new Uri(settings.ContenidoClient);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Accept.Clear();
                client.MaxResponseContentBufferSize = int.MaxValue;
            })
             .SetHandlerLifetime(TimeSpan.FromMinutes(2))
             .ConfigureHttpClient(x =>
             {
                 x.Timeout = TimeSpan.FromMinutes(10);
                 x.MaxResponseContentBufferSize = 2147483647;
             });
            services.AddTransient<MsContenido.IClient, MsContenido.Client>();



            return services;
        }
    }
}
