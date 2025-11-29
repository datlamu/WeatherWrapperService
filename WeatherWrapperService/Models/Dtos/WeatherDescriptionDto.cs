using System.Text.Json.Serialization;

public class WeatherDescriptionDto
{
    [JsonPropertyName("description")]
    public string? Description { get; set; }
}