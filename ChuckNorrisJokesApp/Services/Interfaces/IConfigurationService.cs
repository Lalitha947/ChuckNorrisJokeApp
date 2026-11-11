namespace ChuckNorrisJokesApp.Services.Interfaces
{
    public interface IConfigurationService
    {
        string ChuckNorrisApiUrl { get; }
        string DadJokeApiUrl { get; }
    }
}
