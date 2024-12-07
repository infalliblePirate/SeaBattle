using System.Configuration;
using System.Data;
using System.Windows;
using SeaBattle.Data;
using SeaBattle.Services;
using System.Runtime.InteropServices;
using SeaBattle.ViewModels;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;

namespace SeaBattle;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application {
    [DllImport("kernel32.dll")]
    public static extern bool AllocConsole();

    private void ApplicationStartup(object sender, StartupEventArgs e) {
        AllocConsole();

        string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        var userRepo = new UserRepository(connectionString);
        var userService = new UserService(userRepo);
        var gameRepo = new GameRepository(connectionString);
        var cellRepo = new CellRepository(connectionString);
        var cellService = new CellService(cellRepo);
        var gameService = new GameService(gameRepo, cellService);
        var sessionService = new SessionService();

        // var loginScreen = new Views.LoginScreen(userService, gameService);
        var loginVM = new LoginViewModel(userService, gameService, sessionService);

        // Create the View (LoginScreen) and bind it to the ViewModel
        var loginScreen = new Views.LoginScreen
        {
            DataContext = loginVM
        };

        // Show the LoginScreen
        loginScreen.Show();
    }

}

