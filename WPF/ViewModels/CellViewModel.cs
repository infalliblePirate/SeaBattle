using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Views;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;
using System.Windows.Media;

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
            UpdateColor();
        }
    }

    private Brush _color;
    public Brush Color
    {
        get => _color;
        set
        {
            _color = value;
            OnPropertyChanged();
        }
    }

    public ICommand PlaceShipCommand { get; set; }

    private void UpdateColor()
    {
        Color = State switch
        {
            "Empty" => Brushes.Gray,
            "Hit" => Brushes.Yellow,
            "Missed" => Brushes.Blue,
            "Ship" => Brushes.Green,
            "Blocked" => Brushes.DarkGreen,
            _ => Brushes.Transparent
        };
    }
}
