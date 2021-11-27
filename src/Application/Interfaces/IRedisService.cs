using Application.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IRedisService
    {
        Task<List<T>> TolistAsync<T>(string key);
        Task<Result> AddAsync<T>(string key, T data, TimeSpan? expireTime = null, bool isAbsoluteExpire = true);
        Task<Result> SetCacheAsync<T>(string key, T model, TimeSpan? expireTime = null, bool isAbsoluteExpire = true);
        Task<T> GetCacheAsync<T>(string key);
        Task<Result> RemoveCacheAsync(string key);
    }
}