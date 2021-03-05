using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Ocelot
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOcelot()
                    .AddCacheManager(x =>
                    {
                        x.WithDictionaryHandle();
                    });
            services.AddControllers();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseOcelot().Wait();
            //app.Use(async (context, next) =>
            //{
            //    //4GB
            //    context.Features.Get<IHttpMaxRequestBodySizeFeature>().MaxRequestBodySize = 4294967295;
            //    await next.Invoke();
            //});
        }
    }
}
