using Microsoft.Extensions.Options;
using System.Text.Json;
using WeatherWrapperService.Services.Interfaces;

public class WeatherService : IWeatherService
{
    private readonly HttpClient _httpClient;
    private readonly IRedisService _redis;
    private readonly OpenWeatherSettings _weatherSettings;
    private readonly CacheSettings _cacheSettings;

    public WeatherService(
        HttpClient httpClient,
        IRedisService redis,
        IOptions<OpenWeatherSettings> weatherOptions,
        IOptions<CacheSettings> cacheOptions)
    {
        _httpClient = httpClient;
        _redis = redis;
        _weatherSettings = weatherOptions.Value;
        _cacheSettings = cacheOptions.Value;
    }

    public async Task<WeatherDto> GetWeatherAsync(string city)
    {
        var cacheKey = $"weather:{city.ToLower()}";

        var cached = await _redis.GetAsync(cacheKey);
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<WeatherDto>(cached)!;

        var url = $"{_weatherSettings.BaseUrl}?q={city}&appid={_weatherSettings.ApiKey}&units=metric";
        var response = await _httpClient.GetStringAsync(url);

        await _redis.SetAsync(cacheKey, response, TimeSpan.FromHours(_cacheSettings.ExpirationHours));

        return JsonSerializer.Deserialize<WeatherDto>(response)!;
    }
}
