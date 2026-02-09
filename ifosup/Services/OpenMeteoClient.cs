using System.Net.Http.Json;
using ifosup.Models;

namespace ifosup.Services;

public sealed class OpenMeteoClient
{
    private readonly HttpClient _http;

    public OpenMeteoClient(HttpClient http)
    {
        _http = http;
    }

    /// <summary>
    /// Forecast za Brussels (lat=50.85, lon=4.35).
    /// Vraća current + hourly (time + temperature).
    /// </summary>
    public async Task<OpenMeteoForecastResponse> GetBrusselsForecastAsync(CancellationToken ct = default)
    {
        var url =
            "v1/forecast" +
            "?latitude=50.85&longitude=4.35" +
            "&current=temperature_2m,wind_speed_10m" +
            "&hourly=temperature_2m" +
            "&timezone=Europe%2FBrussels";

        var res = await _http.GetFromJsonAsync<OpenMeteoForecastResponse>(url, ct);

        if (res is null)
            throw new InvalidOperationException("API returned empty response.");

        return res;
    }
}
