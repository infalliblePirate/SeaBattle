using Npgsql;
using System;
using System.Windows;

using SeaBattle.Services;

namespace SeaBattle.Views
{
    public partial class CreateEnterGame : Window
    {
        private readonly AuthService _authService;

        public CreateEnterGame(AuthService authService) {
            _authService = authService;
            InitializeComponent();
        }

        private async void CreateGameClick(object sender, RoutedEventArgs e) {
            MessageBox.Show("You want to create game");
        }

        private async void EnterGameClick(object sender, RoutedEventArgs e) {
            MessageBox.Show("You want to create a game");
        }
    }
}
