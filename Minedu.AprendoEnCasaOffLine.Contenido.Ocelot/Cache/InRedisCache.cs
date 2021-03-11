using Ocelot.Cache;
using StackExchange.Redis.Extensions.Core.Abstractions;
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
        /*
        public InRedisCache(IConfiguration configuration)
        {          
            var redisConfig = new RedisConfig();
            configuration.GetSection("Redis").Bind(redisConfig);

            var csredis = new CSRedis.CSRedisClient(null, redisConfig.ConnectionString);
            RedisHelper.Initialization(csredis);

        }
        */

        private IRedisCacheClient _redisCacheClient;
        public InRedisCache(IRedisCacheClient redisCacheClient)
        {
            _redisCacheClient = redisCacheClient;
        }

        public void Add(string key, T value, TimeSpan ttl, string region)
        {
            key = GetKey(region, key);

            if (ttl.TotalMilliseconds <= 0)
            {
                return;
            }
            //RedisHelper.Set(key, value, (int)ttl.TotalSeconds);
            bool isAdded = (_redisCacheClient.Db0.AddAsync(key, value, DateTimeOffset.Now.AddSeconds((int)ttl.TotalSeconds))).Result;

        }

        public void AddAndDelete(string key, T value, TimeSpan ttl, string region)
        {
            bool isRemoved = (_redisCacheClient.Db0.RemoveAsync(key)).Result;
            Add(key, value, ttl, region);
        }

        public void ClearRegion(string region)
        {
            var listofkeys = _redisCacheClient.Db0.SearchKeysAsync(GetKey(region, "*")).Result;
            var isRemoved = (_redisCacheClient.Db0.RemoveAllAsync(listofkeys)).Result;

            //var data = RedisHelper.Keys(GetKey(region, "*"));
            //RedisHelper.Del(data);
        }

        public T Get(string key, string region)
        {
            key = GetKey(region, key);
            //var result = RedisHelper.Get<T>(key);
            var result = (_redisCacheClient.Db0.GetAsync<T>(key)).Result;

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
