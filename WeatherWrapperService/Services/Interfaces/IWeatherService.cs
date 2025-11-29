namespace WeatherWrapperService.Services.Interfaces
{
    public interface IWeatherService
    {
        Task<WeatherDto> GetWeatherAsync(string city);
    }
}
