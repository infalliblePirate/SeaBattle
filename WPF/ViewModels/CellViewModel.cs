using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Views;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;

public class CellViewModel : BaseViewModel
{
    public int Row { get; set; }
    public int Column { get; set; }

    private string _state;
    public string State
    {
        get => _state;
        set
        {
            _state = value;
            OnPropertyChanged();
        }
    }

    public ICommand PlaceShipCommand { get; set; }
}
