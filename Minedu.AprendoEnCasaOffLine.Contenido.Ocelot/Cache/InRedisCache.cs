using Microsoft.Extensions.Configuration;
using Ocelot.Cache;
using System;

namespace Minedu.AprendoEnCasaOffLine.Contenido.Ocelot
{
    public class RedisConfig 
    {
        public RedisConfig()
        {
            ConnectionString = new string[] { };
        }
        public string[] ConnectionString { get; set; }
    }
    public class InRedisCache<T> : IOcelotCache<T>
    {       
        public InRedisCache(IConfiguration configuration)
        {          
            var redisConfig = new RedisConfig();
            configuration.GetSection("Redis").Bind(redisConfig);

            var csredis = new CSRedis.CSRedisClient(null, redisConfig.ConnectionString);
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
}
