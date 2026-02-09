using System.Text.Json.Serialization;

namespace ifosup.Models;

public sealed class OpenMeteoForecastResponse
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }

    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }

    [JsonPropertyName("timezone")]
    public string? Timezone { get; set; }

    [JsonPropertyName("current")]
    public CurrentWeather? Current { get; set; }

    [JsonPropertyName("hourly")]
    public HourlyForecast? Hourly { get; set; }
}

public sealed class CurrentWeather
{
    [JsonPropertyName("time")]
    public string? Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public double? Temperature2m { get; set; }

    [JsonPropertyName("wind_speed_10m")]
    public double? WindSpeed10m { get; set; }
}

public sealed class HourlyForecast
{
    [JsonPropertyName("time")]
    public List<string>? Time { get; set; }

    [JsonPropertyName("temperature_2m")]
    public List<double>? Temperature2m { get; set; }
}
