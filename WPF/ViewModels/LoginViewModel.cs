using SeaBattle.Models;
using SeaBattle.Services;
using SeaBattle.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;
public class LoginViewModel : BaseViewModel
{
    private string _username;
    private string _password;
    
    private readonly UserService _userService;
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;

    public ICommand LoginCommand { get; }
    public ICommand RegisterCommand { get; }

    public string Username
    {
        get => _username;
        set
        {
            _username = value;
            OnPropertyChanged();
            ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            _password = value;
            OnPropertyChanged();
            ((RelayCommand)LoginCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RegisterCommand).RaiseCanExecuteChanged();
        }
    }
    

    public LoginViewModel(UserService userService, GameService gameService, SessionService sessionService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));

        LoginCommand = new RelayCommand((param) => OnLogin(), CanLogin);
        RegisterCommand = new RelayCommand((param) => OnRegister(), CanRegister);
    }

    private bool CanLogin(object param) => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

    private bool CanRegister(object param) => !string.IsNullOrWhiteSpace(Username) && !string.IsNullOrWhiteSpace(Password);

    private void OnLogin()
    {
        var loginUser = _userService.LoginUser(Username, Password);
        if (loginUser != null)
        {
            _sessionService.SetActiveUser(loginUser);
            var createEnterVM = new CreateEnterGameViewModel(_userService, _gameService, _sessionService);
            var createEnterGameScreen = new Views.CreateEnterGame
            {
                DataContext = createEnterVM
            };
            var currentWindow = Application.Current.MainWindow;
            currentWindow?.Close();
            createEnterGameScreen.Show();
        }
        else
        {
            MessageBox.Show("Username or password incorrect.");
        }
    }

    private void OnRegister()
    {
        var registeredUser = _userService.RegisterUser(Username, Password);
        if (registeredUser != null)
        {
            _sessionService.SetActiveUser(registeredUser);
            var createEnterVM = new CreateEnterGameViewModel(_userService, _gameService, _sessionService);
            var createEnterGameScreen = new Views.LoginScreen
            {
                DataContext = createEnterVM
            };
            var currentWindow = Application.Current.MainWindow;
            currentWindow?.Close();
            createEnterGameScreen.Show();
        }
        else
        {
            MessageBox.Show("Registration failed.");
        }
    }
}

