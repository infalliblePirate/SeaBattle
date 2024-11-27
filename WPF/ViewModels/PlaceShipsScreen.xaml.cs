using System.Windows;
using System.Windows.Controls;

using SeaBattle.Services;
using SeaBattle.Models;

namespace SeaBattle.Views;

public partial class PlaceShipsScreen : Window {
    private readonly GameService _gameService;
    public PlaceShipsScreen(GameService gameService) {
        _gameService = gameService;
        InitializeComponent();
        CreateBoard();
    }

    private void CreateBoard() {
        _gameService.CreateGame(); // TODO: move from here

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

    private void ButtonClick(object sender, RoutedEventArgs e) {
        if (sender is Button button) {
            var (row, col) = ((int, int))button.Tag;
            // place 1 deck test
            ShipModel ship = new ShipModel(4, true, new Vector2D(col, row));
            if(_gameService.PlaceShip(ship)) {
                _gameService.PrintBoard();
                MessageBox.Show($"Placed ship at: Row {row}, Column {col}");
            } else {
                MessageBox.Show($"Cannot place ship at: Row {row}, Column {col}");
            }
        }
    }
}

