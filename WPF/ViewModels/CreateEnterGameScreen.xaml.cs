using Npgsql;
using System;
using System.Windows;

using SeaBattle.Services;

namespace SeaBattle.Views
{
    public partial class CreateEnterGame : Window
    {
        private readonly GameService _gameService;

        public CreateEnterGame(GameService gameService) {
            _gameService = gameService;
            InitializeComponent();
        }

        private async void CreateGameClick(object sender, RoutedEventArgs e) {
             try {
                var game = await _gameService.CreateGame();
                if (game != null) {
                    MessageBox.Show($"Game created with ID: {game.Id}");
                } else {
                    MessageBox.Show("Failed to create game.");
                }
            } catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void EnterGameClick(object sender, RoutedEventArgs e) {
            MessageBox.Show("You want to create a game");
        }
    }
}
