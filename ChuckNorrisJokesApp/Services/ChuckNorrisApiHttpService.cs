using ChuckNorrisJokesApp.Models;
using ChuckNorrisJokesApp.Services.Interfaces;

using System.Net.Http.Headers;
using System.Text.Json;

namespace ChuckNorrisJokesApp.Services;

public class ChuckNorrisApiHttpService : IChuckNorrisApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfigurationService _configurationService;

    public ChuckNorrisApiHttpService(IConfigurationService configurationService)
    {
        _configurationService = configurationService;
        _httpClient = new HttpClient();
    }

    public async Task<string> GetJokeAsync()
    {
        ChuckNorrisJoke response = await GetResponseAsync<ChuckNorrisJoke>(
            _configurationService.ChuckNorrisApiUrl);

        return GetJokeText(response.Value, "Chuck Norris");
    }

    public async Task<string> GetDadJokeAsync()
    {
        DadJoke response = await GetResponseAsync<DadJoke>(
            _configurationService.DadJokeApiUrl,
            requiresJsonAcceptHeader: true);

        return GetJokeText(response.Joke, "dad joke");
    }

    private async Task<T> GetResponseAsync<T>(string url, bool requiresJsonAcceptHeader = false)
    {
        using var request = new HttpRequestMessage(HttpMethod.Get, url);

        if (requiresJsonAcceptHeader)
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        using HttpResponseMessage response = await _httpClient.SendAsync(request);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"The request to {url} failed with status code {(int)response.StatusCode} ({response.StatusCode}).");
        }

        string jsonContent = await response.Content.ReadAsStringAsync();

        if (string.IsNullOrWhiteSpace(jsonContent))
        {
            throw new InvalidOperationException($"The response from {url} was empty.");
        }

        try
        {
            return JsonSerializer.Deserialize<T>(jsonContent)
                ?? throw new InvalidOperationException($"The response from {url} did not contain an object.");
        }
        catch (JsonException exception)
        {
            throw new InvalidOperationException($"The response from {url} was not valid JSON.", exception);
        }
    }

    private static string GetJokeText(string? jokeText, string jokeType)
    {
        if (string.IsNullOrWhiteSpace(jokeText))
        {
            throw new InvalidOperationException($"The {jokeType} response did not contain joke text.");
        }

        return jokeText;
    }
}