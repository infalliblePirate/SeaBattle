using SeaBattle.Models;
using SeaBattle.Services;
using SeaBattle.Utils;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;
public class LoginViewModel : BaseViewModel, IInitializable
{
    private string _username;
    private string _password;
    
    private readonly INavigationService _navigationService;
    private readonly UserService _userService;
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

    public void InitializeAdditional(object param) {}
    

    public LoginViewModel(UserService userService, SessionService sessionService, INavigationService navigationService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));

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
            _navigationService.NavigateTo<CreateEnterGameViewModel>();
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
            _navigationService.NavigateTo<CreateEnterGameViewModel>();
        }
        else
        {
            MessageBox.Show("Registration failed.");
        }
    }
}

