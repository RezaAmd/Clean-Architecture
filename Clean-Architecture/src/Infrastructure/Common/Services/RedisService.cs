using Application.Models;
using Application.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace Infrastructure.Common.Services
{
    public class RedisService : IRedisService
    {
        #region Constructor
        private readonly IDistributedCache cache;
        private readonly ILogger<RedisService> logger;
        public RedisService(IDistributedCache _cache,
            ILogger<RedisService> _logger)
        {
            cache = _cache;
            logger = _logger;
        }
        #endregion

        public async Task<List<T>> TolistAsync<T>(string key)
        {
            try
            {
                string row = await cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(row))
                    return JsonConvert.DeserializeObject<List<T>>(row);
            }
            catch (Exception ex)
            {
                logger.LogWarning("Not found any item in redis. EX:" + ex.Message);
            }
            return default(List<T>);
        }

        public async Task<Result> AddAsync<T>(string key, T data, TimeSpan? expireTime = null, bool isAbsoluteExpire = true)
        {
            try
            {
                var list = await TolistAsync<T>(key);
                list.Add(data);
                var result = await SetCacheAsync(key, list, expireTime, isAbsoluteExpire);
                return result;
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex.Message);
                return Result.Failed();
            }
        }

        public async Task<Result> SetCacheAsync<T>(string key, T model, TimeSpan? expireTime = null, bool isAbsoluteExpire = true)
        {
            try
            {
                string serializedObj = JsonConvert.SerializeObject(model, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });

                if (expireTime != null)
                    if (isAbsoluteExpire)
                        await cache.SetStringAsync(key, serializedObj,
                            new DistributedCacheEntryOptions().SetAbsoluteExpiration(expireTime.Value));
                    else
                        await cache.SetStringAsync(key, serializedObj,
                            new DistributedCacheEntryOptions().SetSlidingExpiration(expireTime.Value));
                else
                    await cache.SetStringAsync(key, serializedObj);

                logger.LogInformation($"{key} key storage in Radis was successful.");
                return Result.Success;
            }
            catch (Exception ex)
            {
                logger.LogError($"{key} key storage failed.");
                return Result.Failed(new() { new(0, ex.Message) });
            }
        }

        public async Task<T> GetCacheAsync<T>(string key)
        {
            try
            {
                string row = await cache.GetStringAsync(key);
                if (!string.IsNullOrEmpty(row))
                    return JsonConvert.DeserializeObject<T>(row);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
            }
            return default(T);
        }

        public async Task<Result> RemoveCacheAsync(string key)
        {
            try
            {
                await cache.RemoveAsync(key);
                return Result.Success;
            }
            catch
            {
                return Result.Failed();
            }
        }
    }
}