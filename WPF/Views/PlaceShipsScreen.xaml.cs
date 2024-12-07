using System.Windows;
using SeaBattle.ViewModels;

namespace SeaBattle.Views;
public partial class PlaceShipsScreen : Window
{
    private readonly PlaceShipsViewModel _viewModel;

    public PlaceShipsScreen(PlaceShipsViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        InitializeComponent();
    }
}