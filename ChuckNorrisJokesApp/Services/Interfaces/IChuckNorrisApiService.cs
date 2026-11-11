namespace ChuckNorrisJokesApp.Services.Interfaces
{
    public interface IChuckNorrisApiService
    {
        Task<string> GetJokeAsync();
        Task<string> GetDadJokeAsync();
    }
}
