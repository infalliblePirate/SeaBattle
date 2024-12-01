using System.Windows;
using System.Windows.Controls;

using SeaBattle.Services;
using SeaBattle.Models;
using SeaBattle.Common;

namespace SeaBattle.Views;

public partial class PlaceShipsScreen : Window {
    private readonly GameService _gameService;
    public PlaceShipsScreen(GameService gameService) {
        _gameService = gameService;
        InitializeComponent();
        CreateBoard();
    }

    private void CreateBoard() {
        // _gameService.CreateGameAsync(); // TODO: move from here

        for (int row = 0; row < 10; row++) {
            for (int col = 0; col < 10; col++) {
                Button button = new Button {
                    Content = $"{row},{col}", 
                    Tag = (row, col),      
                    Background = System.Windows.Media.Brushes.LightBlue
                };

                // Attach a click event handler
                button.Click += ButtonClick;

                // Add the button to the UniformGrid
                GameBoard.Children.Add(button);
            }
        }

    }

    private async void UpdateBoard() {
        // int activeUserId = SessionService.ActiveUser.Id;
        int activeGameId = SessionService.ActiveGame.Id;
        var _boardState = await _gameService.GetBoardStateAsync(activeGameId);
        for (int row = 0; row < 10; row++) {
            for (int col = 0; col < 10; col++) {
                // Find the button based on its tag
                var button = GameBoard.Children.Cast<Button>()
                    .FirstOrDefault(b => ((int, int))b.Tag == (row, col));

                if (button != null) {
                    // Update button color based on the board state
                    if (_boardState[col, row] == CellState.Ship) {
                        button.Background = System.Windows.Media.Brushes.Red; // Ship placed
                    } else if(_boardState[col, row] == CellState.Blocked) {
                        button.Background = System.Windows.Media.Brushes.Green; // Ship placed
                    } else {
                        button.Background = System.Windows.Media.Brushes.LightBlue; // Empty
                    }
                }
            }
        }
    }

    private async void ButtonClick(object sender, RoutedEventArgs e) {
        if (sender is Button button) {
            var (row, col) = ((int, int))button.Tag;
            // place 1 deck test
            ShipModel ship = new ShipModel(4, true, new Vector2D(col, row));
            if(await _gameService.PlaceShipAsync(ship)) {
                _gameService.PrintBoardAsync();
                UpdateBoard();
                MessageBox.Show($"Placed ship at: Row {row}, Column {col}");
            } else {
                MessageBox.Show($"Cannot place ship at: Row {row}, Column {col}");
            }
        }
    }
}

