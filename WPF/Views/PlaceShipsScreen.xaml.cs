using System.Windows;
using SeaBattle.ViewModels;

namespace SeaBattle.Views;

public partial class PlaceShipsScreen : Window
{
    public PlaceShipsScreen(PlaceShipsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
