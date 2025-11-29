using StackExchange.Redis;
using WeatherWrapperService.Services.Interfaces;
using System;
using System.Threading.Tasks;

public class RedisService : IRedisService
{
    private readonly IConnectionMultiplexer _redis;

    public RedisService(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    private IDatabase Db => _redis.GetDatabase();

    public async Task<string> GetAsync(string key)
    {
        var value = await Db.StringGetAsync(key);
        return value.HasValue ? value.ToString() : null;
    }

    public async Task SetAsync(string key, string value, TimeSpan? expiry = null)
    {
        await Db.StringSetAsync(key, value);
        if (expiry.HasValue)
            await Db.KeyExpireAsync(key, expiry.Value);
    }
}
