using Npgsql;
using System;
using System.Windows;

using SeaBattle.Services;

namespace SeaBattle.Views
{
    public partial class CreateEnterGame : Window
    {
        private readonly GameService _gameService;
        private readonly UserService _userService;

        public CreateEnterGame(GameService gameService, UserService userService) {
            _gameService = gameService;
            _userService = userService;
            InitializeComponent();
        }

        private async void CreateGameClick(object sender, RoutedEventArgs e) {
            try {
                var game = await _gameService.CreateGameAsync();
                if (game != null) {
                    SessionService.ActiveGame = game;
                    MessageBox.Show($"Game created with ID: {game.Id}");
                    PlaceShipsScreen placeShipsScreen = new PlaceShipsScreen(_gameService);
                    placeShipsScreen.Show();
                    this.Close();
                } else {
                    MessageBox.Show("Failed to create game.");
                }
            } catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void EnterGameClick(object sender, RoutedEventArgs e) {
            Console.WriteLine("Entered game");
             try {
                if (int.TryParse(txtGameCode.Text, out int gameId)) {
                    await _userService.JoinGame(gameId);
                    SessionService.ActiveGame = await _gameService.GetGameByIdAsync(gameId);
                    PlaceShipsScreen placeShipsScreen = new PlaceShipsScreen(_gameService);
                    placeShipsScreen.Show();
                    this.Close();
                } else {
                    MessageBox.Show("Please enter a valid game code.");
                }
            }
            catch (Exception ex) {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
    }
}
