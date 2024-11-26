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
        private readonly AuthService _authService;

        public LoginScreen(AuthService authService)
        {
            _authService = authService;
            InitializeComponent();
        }

        private async void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            var loginUser = await _authService.LoginUserAsync(txtUsername.Text, txtPassword.Password);
            if (loginUser != null) {
                SessionService.Activeuser = loginUser;
                CreateEnterGame createEnterGame = new CreateEnterGame(_authService);
                createEnterGame.Show();
                this.Close();
            } else {
                MessageBox.Show("Username or password incorrect.");
            }
        }

        private async void RegisterSubmitClick(object sender, RoutedEventArgs e) {
            var registeredUser = await _authService.RegisterUserAsync(txtUsername.Text, txtPassword.Password);
            if (registeredUser != null) {
                SessionService.Activeuser = registeredUser;
                CreateEnterGame createEnterGame = new CreateEnterGame(_authService);
                createEnterGame.Show();
                this.Close();
            } else {
                MessageBox.Show("Registration failed.");
            }
        }
    }
}
