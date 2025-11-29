using System.Text.Json.Serialization;

public class WeatherDto
{
    [JsonPropertyName("name")]
    public string? City { get; set; }

    [JsonPropertyName("main")]
    public MainDto? Main { get; set; }

    [JsonPropertyName("weather")]
    public WeatherDescriptionDto[]? Weather { get; set; }
}