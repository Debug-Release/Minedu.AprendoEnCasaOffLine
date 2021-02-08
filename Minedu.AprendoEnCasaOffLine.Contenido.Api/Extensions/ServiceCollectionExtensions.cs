using Microsoft.Extensions.DependencyInjection;
using Minedu.IS4.Security.Auth;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public enum EnumCors
    {
        AllowSpecificOrigin
    }
    public static class ServiceCollectionExtensions
    {
        public static void AddCustomCors(this IServiceCollection services, EnumCors enumCors = EnumCors.AllowSpecificOrigin)
        {
            string corsName = enumCors.ToString();

            services.AddCors(o =>
            {
                {
                    o.AddPolicy(corsName, SecurityConfig.GetCORSPolicy());
                }
            });
        }
    }
}
