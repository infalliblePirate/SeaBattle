using Npgsql;
using System;
using System.Windows;

using SeaBattle.Services;
using SeaBattle.Models;

namespace SeaBattle.Views
{
    /// <summary>
    /// Interaction logic for LoginScreen.xaml
    /// </summary>
    public partial class LoginScreen : Window
    {
        private readonly UserService _userService;
        private readonly GameService _gameService;

        public LoginScreen(UserService userService, GameService gameService)
        {
            _userService = userService;
            _gameService = gameService;
            InitializeComponent();
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var loginUser = await _userService.LoginUserAsync(txtUsername.Text, txtPassword.Password);
            if (loginUser != null) {
                SessionService.ActiveUser = loginUser;
                CreateEnterGame createEnterGame = new CreateEnterGame(_gameService, _userService);
                createEnterGame.Show();
                this.Close();
            } else {
                MessageBox.Show("Username or password incorrect.");
            }
        }

        private async void RegisterSubmitClick(object sender, RoutedEventArgs e) {
            var registeredUser = await _userService.RegisterUserAsync(txtUsername.Text, txtPassword.Password);
            if (registeredUser != null) {
                SessionService.ActiveUser = registeredUser;
                CreateEnterGame createEnterGame = new CreateEnterGame(_gameService, _userService);
                createEnterGame.Show();
                this.Close();
            } else {
                MessageBox.Show("Registration failed.");
            }
        }
    }
}
