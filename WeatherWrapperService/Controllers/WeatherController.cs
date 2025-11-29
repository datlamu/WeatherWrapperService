using Microsoft.AspNetCore.Mvc;
using WeatherWrapperService.Models;
using WeatherWrapperService.Services.Interfaces;

public class WeatherController : Controller
{
    private readonly IWeatherService _weatherService;

    public WeatherController(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public async Task<IActionResult> Index(string city)
    {
        if (string.IsNullOrEmpty(city))
            return View("Index", null);

        var weather = await _weatherService.GetWeatherAsync(city);

        return View("Index", weather);
    }
}
