using Npgsql;
using System;
using System.Windows;

using SeaBattle.Services;

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
            bool isRegisteredUser = await _authService.ValidataeUserAsync(txtUsername.Text, txtPassword.Password);
            if (isRegisteredUser) {
                MainWindow mainWindow = new MainWindow();
                 mainWindow.Show();
                this.Close();
            } else {
                MessageBox.Show("Username or password incorrect.");
            }
        }

        private async void RegisterSubmitClick(object sender, RoutedEventArgs e) {
            bool isSuccessfulRegistration = await _authService.RegisterUserAsync(txtUsername.Text, txtPassword.Password);
            if (isSuccessfulRegistration) {
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                this.Close();
            } else {
                MessageBox.Show("Registration failed.");
            }
        }
    }
}
