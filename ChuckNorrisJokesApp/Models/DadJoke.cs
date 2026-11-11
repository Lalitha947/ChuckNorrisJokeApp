using System.Text.Json.Serialization;

namespace ChuckNorrisJokesApp.Models;

public class DadJoke
{
    [JsonPropertyName("joke")]
    public string? Joke { get; set; }
}