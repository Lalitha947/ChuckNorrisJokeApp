using ChuckNorrisJokesApp.Services.Interfaces;

namespace ChuckNorrisJokesApp.ViewModels;

public class JokesViewModel : BaseViewModel
{
    private readonly IChuckNorrisApiService _jokesService;
    private string _chuckNorrisJoke = string.Empty;
    private string _dadJoke = string.Empty;
    private string _errorMessage = string.Empty;
    private bool _hasError;
    private bool _isLoading;

    public JokesViewModel(IChuckNorrisApiService jokesService)
    {
        _jokesService = jokesService;
        GetJokeCommand = new Command(async () => await FetchJokesAsync(), () => !IsLoading);
    }

    public Command GetJokeCommand { get; }

    public string ChuckNorrisJoke
    {
        get => _chuckNorrisJoke;
        private set
        {
            if (_chuckNorrisJoke == value)
            {
                return;
            }

            _chuckNorrisJoke = value;
            OnPropertyChanged();
        }
    }

    public string DadJoke
    {
        get => _dadJoke;
        private set
        {
            if (_dadJoke == value)
            {
                return;
            }

            _dadJoke = value;
            OnPropertyChanged();
        }
    }

    public bool IsLoading
    {
        get => _isLoading;
        private set
        {
            if (_isLoading == value)
            {
                return;
            }

            _isLoading = value;
            OnPropertyChanged();
            GetJokeCommand.ChangeCanExecute();
        }
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        private set
        {
            if (_errorMessage == value)
            {
                return;
            }

            _errorMessage = value;
            OnPropertyChanged();
        }
    }

    public bool HasError
    {
        get => _hasError;
        private set
        {
            if (_hasError == value)
            {
                return;
            }

            _hasError = value;
            OnPropertyChanged();
        }
    }

    private async Task FetchJokesAsync()
    {
        if (IsLoading)
        {
            return;
        }

        IsLoading = true;
        ErrorMessage = string.Empty;
        HasError = false;

        var errors = new List<string>();
        Task<string> chuckNorrisTask = _jokesService.GetJokeAsync();
        Task<string> dadJokeTask = _jokesService.GetDadJokeAsync();

        try
        {
            try
            {
                ChuckNorrisJoke = await chuckNorrisTask;
            }
            catch (Exception exception)
            {
                errors.Add($"Chuck Norris joke failed: {exception.Message}");
            }

            try
            {
                DadJoke = await dadJokeTask;
            }
            catch (Exception exception)
            {
                errors.Add($"Dad joke failed: {exception.Message}");
            }

            ErrorMessage = string.Join(Environment.NewLine, errors);
            HasError = errors.Count > 0;
        }
        finally
        {
            IsLoading = false;
        }
    }
}