# Chuck Norris Jokes App

> **⚠️ For educational purposes only.**
> This solution exists to demonstrate .NET MAUI, MVVM and dependency injection. It is not production software and comes with no warranty or support. It is not affiliated with or endorsed by Chuck Norris or by the [chucknorris.io](https://api.chucknorris.io) API. Jokes are fetched live from that third-party API, so content is outside this project's control.

## What it does

A small cross-platform .NET MAUI app. Tap a button and it fetches a random Chuck Norris joke and a random dad joke, then displays both results. A loading indicator is shown while the requests are running, and request errors are displayed without hiding a joke that loaded successfully.

## API response data

The app needs only the joke text from each API response:

- **Chuck Norris jokes**: send a `GET` request to `https://api.chucknorris.io/jokes/random`. The JSON response contains the joke in the `value` field.
- **Dad jokes**: send a `GET` request to `https://icanhazdadjoke.com/` with the `Accept: application/json` request header. The JSON response contains the joke in the `joke` field.

## Solution layout

```
ChuckNorrisJokesAppSln.slnx          Solution file
└── ChuckNorrisJokesApp/             Single MAUI project
    ├── MauiProgram.cs               App startup and dependency injection registration
    ├── App.xaml / AppShell.xaml     Application and Shell navigation
    ├── Models/
    │   ├── ChuckNorrisJoke.cs       DTO for the Chuck Norris API's `value` field
    │   └── DadJoke.cs                DTO for the dad-joke API's `joke` field
    ├── Services/
    │   ├── Interfaces/              IChuckNorrisApiService, IConfigurationService, IPopUps
    │   ├── ChuckNorrisApiHttpService.cs   Calls both APIs and deserializes their JSON
    │   ├── ConfigurationService.cs        Holds the API URL
    │   └── MauiPopupService.cs            Shows alerts through MAUI
    ├── ViewModels/
    │   ├── BaseViewModel.cs         INotifyPropertyChanged helper
    │   └── JokesViewModel.cs        Exposes both jokes, loading/error state, and the refresh command
    ├── Views/
    │   └── JokesView.xaml(.cs)      UI bound to JokesViewModel
    ├── Platforms/                   Android, iOS, Mac Catalyst and Windows entry points
    └── Resources/                   Icons, splash screen, fonts, images, styles
```

## Responsibilities

- **View (`JokesView.xaml`)**: presents the two jokes, refresh button, loading indicator, and error message. It contains no API or application logic.
- **ViewModel (`JokesViewModel`)**: coordinates both requests, exposes bindable joke/loading/error state, prevents repeated requests while loading, and preserves successful results when the other request fails.
- **Response models (`ChuckNorrisJoke` and `DadJoke`)**: represent the JSON fields required by the app (`value` and `joke`).
- **API service (`ChuckNorrisApiHttpService`)**: sends HTTP requests, sets the dad-joke JSON header, deserializes responses, and reports unsuccessful or invalid responses as exceptions.
- **Dependency injection**: services, view, and view model are registered in `MauiProgram.cs` and constructor-injected.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- The .NET MAUI workload: `dotnet workload install maui`
- Visual Studio 2026 with the ".NET Multi-platform App UI development" workload (recommended on Windows)
- Platform tooling for whichever target you run (Android SDK/emulator, Xcode on macOS for iOS and Mac Catalyst)

## Building and running

From Visual Studio, select a target platform and press F5. From the command line, build and run the Windows target with:

```bash
dotnet build ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
dotnet run --project ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
```

Replace the `-f` value with `net10.0-android`, `net10.0-ios` or `net10.0-maccatalyst` for the other targets. iOS and Mac Catalyst require a Mac. An internet connection is needed to fetch jokes.

## Verification

Run the app on a target platform and select **Get new jokes**.

1. **Successful responses**: verify that both labeled joke areas contain text, the loading indicator appears while requests are in progress, and the button can be used again after loading finishes.
2. **One API failure**: make one endpoint unavailable or return an unsuccessful response. Verify that the other joke remains visible and the error area identifies the failed API.
3. **Unavailable network**: disable the device or emulator network connection and select **Get new jokes**. Verify that the app leaves any previously loaded jokes visible, displays a useful error message, and becomes usable again after loading ends.
4. **Recovery**: restore network access and select **Get new jokes** again. Verify that both jokes can be refreshed successfully.

The solution should also be compiled before running:

```bash
dotnet build ChuckNorrisJokesApp/ChuckNorrisJokesApp.csproj -f net10.0-windows10.0.19041.0
```

## Known limitations (learning opportunities)

- `HttpClient` is created directly rather than via `IHttpClientFactory`.
- The API URL is hard-coded in `ConfigurationService` rather than read from settings.
- There are no automated tests.

## Disclaimer

This project is provided "as is" for learning and demonstration only. Do not use it in production or rely on it for anything important. Review the terms of the [chucknorris.io](https://api.chucknorris.io) API before using it beyond personal learning.
