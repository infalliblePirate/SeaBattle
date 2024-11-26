using System.Configuration;
using System.Data;
using System.Windows;
using SeaBattle.Data;
using SeaBattle.Services;

namespace SeaBattle;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    private void ApplicationStartup(object sender, StartupEventArgs e) {
        string connectionString = ConfigurationHelper.GetConnectionString("DefaultConnection");
        var userRepo = new UserRepository(connectionString);
        var authService = new AuthService(userRepo);
        var gameRepo = new GameRepository(connectionString);
        var gameService = new GameService(gameRepo);

        var loginScreen = new Views.LoginScreen(authService, gameService);

        loginScreen.Show();
    }

}

