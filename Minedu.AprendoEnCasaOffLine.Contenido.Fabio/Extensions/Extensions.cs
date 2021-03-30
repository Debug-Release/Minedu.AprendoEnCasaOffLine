using Convey;
using Convey.Discovery.Consul;
using Convey.HTTP;
using Convey.LoadBalancing.Fabio;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Fabio
{
    public static class Extensions
    {
        private static readonly string ConsulSectionName = "consul";
        private static readonly string FabioSectionName = "fabio";
        private static readonly string HttpClientSectionName = "httpClient";

        public static IConveyBuilder RegisterConvey(this IConveyBuilder builder, IConfiguration configuration)
        {
            
            var uniqueId = Assembly.GetEntryAssembly().GetName().Name;

            //Consult
            /*
            var consulOptions = new ConsulOptions();
            configuration.GetSection("External::consult").Bind(consulOptions);

            //Fabio
            var fabioOptions = new FabioOptions();
            configuration.GetSection(FabioSectionName).Bind(fabioOptions);


            var httpClientOptions = builder.GetOptions<HttpClientOptions>(HttpClientSectionName);

            consulOptions.Service = uniqueId.ToLower();
            fabioOptions.Service = uniqueId.ToLower();

            builder
                .AddHttpClient()
                .AddConsul(consulOptions, httpClientOptions)
                .AddFabio(fabioOptions, consulOptions, httpClientOptions);
            */
            
            builder
                .AddHttpClient()
                .AddConsul()
                .AddFabio();
            
            return builder;
        }


    }
}
