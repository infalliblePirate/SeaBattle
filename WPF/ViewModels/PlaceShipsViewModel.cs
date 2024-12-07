using System.Collections.ObjectModel;

using SeaBattle.Models;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Common;
using System.Windows;

namespace SeaBattle.ViewModels;

public class PlaceShipsViewModel : BaseViewModel
{
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;
    private readonly int _gameId;

    public ObservableCollection<CellViewModel> Board { get; } = new ObservableCollection<CellViewModel>();

    public PlaceShipsViewModel(int gameId, GameService gameService, SessionService sessionService)
    {
        _gameService = gameService;
        _sessionService = sessionService;
        _gameId = gameId;

        for (int row = 0; row < 10; row++)
        {
            for (int col = 0; col < 10; col++)
            {
                Board.Add(new CellViewModel
                {
                    Row = row,
                    Column = col,
                    State = "Empty",
                    PlaceShipCommand = new RelayCommand(param => PlaceShip((CellViewModel)param))
                });
            }
        }
    }

    private void PlaceShip(CellViewModel cell)
    {
        // int gameId = _sessionService.ActiveGame.Id; 
        int playerId = _sessionService.ActiveUser.Id; 
        
        int nDecks = 3; 
        bool isHorizontal = true;
        Vector2D startCoords = new Vector2D(cell.Row, cell.Column);

        ShipModel ship = new ShipModel(nDecks, isHorizontal, startCoords);

        bool isPlaced = _gameService.PlaceShip(_gameId, playerId, ship);

        if (isPlaced)
        {
            UpdateBoardUI(playerId);
        }
        else
        {
            MessageBox.Show("Ship could not be placed here.");
        }
    }

    public void UpdateBoardUI(int playerId)
    {
        var cells = _gameService.GetCellsForPlayer(_gameId, playerId);

        foreach (var cell in cells)
        {
            var uiCell = Board[cell.Row * 10 + cell.Col];
            uiCell.State = cell.State.ToString();
        }

        OnPropertyChanged(nameof(Board));
    }
}
