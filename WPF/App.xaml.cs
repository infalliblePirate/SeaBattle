using System.Configuration;
using System.Data;
using System.Windows;
using SeaBattle.Data;
using SeaBattle.Services;
using System.Runtime.InteropServices;
using SeaBattle.ViewModels;
using SeaBattle.Contexts;

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

        _navigationService = new NavigationService(type =>
        {
            return type switch
            {
                Type t when t == typeof(LoginViewModel) => new LoginViewModel(userService, sessionService, _navigationService),
                Type t when t == typeof(CreateEnterGameViewModel) => new CreateEnterGameViewModel(gameService, sessionService, _navigationService),
                Type t when t == typeof(PlaceShipsViewModel) => new PlaceShipsViewModel(gameService, sessionService, _navigationService),
                Type t when t == typeof(BattleViewModel) => new BattleViewModel(gameService, sessionService, _navigationService),
                _ => throw new InvalidOperationException($"No ViewModel mapping for {type.Name}")
            };
        });

        _navigationService.NavigateTo<LoginViewModel>();

        // // var loginScreen = new Views.LoginScreen(userService, gameService);
        // var loginVM = new LoginViewModel(userService, gameService, sessionService);

        // // Create the View (LoginScreen) and bind it to the ViewModel
        // var loginScreen = new Views.LoginScreen
        // {
        //     DataContext = loginVM
        // };

        // // Show the LoginScreen
        // loginScreen.Show();
    }

}

