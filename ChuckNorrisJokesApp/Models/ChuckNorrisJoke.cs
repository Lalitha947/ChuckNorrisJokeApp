using System.Text.Json.Serialization;

namespace ChuckNorrisJokesApp.Models;

public class ChuckNorrisJoke
{
    [JsonPropertyName("value")]
    public string? Value { get; set; }
}
