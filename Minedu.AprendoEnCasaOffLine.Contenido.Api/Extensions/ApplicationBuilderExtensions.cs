using Microsoft.AspNetCore.Builder;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Api
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomCors(this IApplicationBuilder app, EnumCors enumCors = EnumCors.AllowSpecificOrigin)
        {
            app.UseCors(enumCors.ToString());
        }

    }
}
