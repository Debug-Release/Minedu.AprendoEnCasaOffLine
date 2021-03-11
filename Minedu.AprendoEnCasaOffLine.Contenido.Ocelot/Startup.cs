using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Ocelot
{

    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "_specificOrigins";

        public void ConfigureServices(IServiceCollection services)
        {
            /*
            var configuration = new FileConfiguration 
            {
                GlobalConfiguration = new FileGlobalConfiguration
                {
                    //RequestIdKey = "RequestId",
                    //ServiceDiscoveryProvider = new FileServiceDiscoveryProvider
                    //{
                    //    Scheme = "https",
                    //    Host = "127.0.0.1",
                    //},
                },
                Routes = new List<FileRoute>()
                {
                    new FileRoute()
                    {
                        DownstreamHostAndPorts = new List<FileHostAndPort>
                        {
                            new FileHostAndPort
                            {
                                Host = "localhost",
                                Port = 80,
                            },
                        },
                        DownstreamScheme = "https",
                        DownstreamPathTemplate = "/",
                        UpstreamHttpMethod = new List<string> { "get" },
                        UpstreamPathTemplate = "/",
                        FileCacheOptions = new FileCacheOptions
                        {
                            TtlSeconds = 10,
                            Region = "Geoff",
                        },
                    }
                }
            };

            */
            services.AddOcelot()
                    .AddCacheManager(x =>
                    {
                        x.WithDictionaryHandle();

                    });
            services.AddSingleton<IOcelotCache<CachedResponse>, InRedisCache<CachedResponse>>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                                  builder =>
                                  {
                                      builder.WithOrigins("http://localhost:4200")
                                             .AllowAnyHeader()
                                             .AllowAnyMethod()
                                             .AllowCredentials();
                                  });
            });           

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });                                 

            app.UseOcelot().Wait();
            
        }
    }
}
