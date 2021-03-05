using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Ocelot.Cache;
using Ocelot.Cache.CacheManager;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Ocelot
{
    public class InRedisCache<T> : IOcelotCache<T>
    {
        private readonly IConfiguration _configuration;
        public InRedisCache(IConfiguration configuration)
        {
            _configuration = configuration;
            string cs = _configuration.GetSection("Redis:ConnectionString").Value;

            var csredis = new CSRedis.CSRedisClient(cs);
            RedisHelper.Initialization(csredis);

        }

        public void Add(string key, T value, TimeSpan ttl, string region)
        {
            key = GetKey(region, key);

            if (ttl.TotalMilliseconds <= 0)
            {
                return;
            }
            //RedisHelper.Set(key, value.ToJson(), (int)ttl.TotalSeconds);
            RedisHelper.Set(key, value, (int)ttl.TotalSeconds);
        }

        public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
        {
            Add(key, value, ttl, region);
        }

        public void ClearRegion(string region)
        {
            var data = RedisHelper.Keys(GetKey(region, "*"));
            RedisHelper.Del(data);
        }

        public T Get(string key, string region)
        {
            key = GetKey(region, key);
            var result = RedisHelper.Get<T>(key);
            if (result != null)
            {
                return (T)result;
            }
            return default(T);
        }

        private string GetKey(string region, string key)
        {
            return $"{region}-{key}".ToLower();
        }
    }

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
