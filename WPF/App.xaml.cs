using System.Configuration;
using System.Data;
using System.Windows;
using SeaBattle.Data;
using SeaBattle.Services;
using System.Runtime.InteropServices;
using SeaBattle.ViewModels;
using SeaBattle.Contexts;
using System.Windows.Media.Animation;
using SeaBattle.Views;

namespace SeaBattle;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    private INavigationService _navigationService;

    private void ApplicationStartup(object sender, StartupEventArgs e)
    {
        // AllocConsole();

        string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        var userRepo = new UserRepository(connectionString);
        var userService = new UserService(userRepo);
        var gameRepo = new GameRepository(connectionString);
        var cellRepo = new CellRepository(connectionString);
        var cellService = new CellService(cellRepo);
        var gameService = new GameService(gameRepo, cellService);
        var sessionService = new SessionService();
        var turnService = new TurnService(gameService, sessionService);
        var actionService = new ActionService(gameService, sessionService);
        var gameStateService = new GameStateService(gameService, userService, sessionService);

        _navigationService = new NavigationService(type =>
        {
            return type switch
            {
                Type t when t == typeof(LoginViewModel) => new LoginViewModel(userService, sessionService, _navigationService),
                Type t when t == typeof(GameLobbyViewModel) => new GameLobbyViewModel(gameService, sessionService, _navigationService),
                Type t when t == typeof(PlaceShipsViewModel) => new PlaceShipsViewModel(gameService, sessionService, _navigationService),
                Type t when t == typeof(BattleViewModel) => new BattleViewModel(gameStateService, turnService, actionService, _navigationService),
                Type t when t == typeof(GameHistoryViewModel) => new GameHistoryViewModel(gameService, userService, sessionService, _navigationService),
                _ => throw new InvalidOperationException($"No ViewModel mapping for {type.Name}")
            };
        });

        _navigationService.NavigateTo<LoginViewModel>();
    }

}

