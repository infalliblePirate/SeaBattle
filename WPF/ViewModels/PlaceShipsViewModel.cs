using System.Collections.ObjectModel;
using System.Windows;
using SeaBattle.Contexts;
using SeaBattle.Views;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Models;
using SeaBattle.Common;

namespace SeaBattle.ViewModels;
public class PlaceShipsViewModel : BaseViewModel, IInitializable
{
    private System.Timers.Timer _readyCheckTimer;
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;
    private readonly INavigationService _navigationService;
    private int _gameId;

    public ObservableCollection<CellViewModel> Board { get; } = new ObservableCollection<CellViewModel>();

    public RelayCommand PlaceNextShipCommand { get; }
    public RelayCommand ToggleOrientationCommand { get; }

    private readonly Queue<(int Decks, int RemainingCount)> _shipsToPlace = new Queue<(int Decks, int RemainingCount)>();
    private readonly BoardProps _boardProps;
    private int _currentShipDecks;
    private bool _isHorizontal = true;

    public PlaceShipsViewModel(GameService gameService, SessionService sessionService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _gameService = gameService;
        _sessionService = sessionService;
        _boardProps = new BoardProps();

        InitializeBoard();
        InitializeShipsToPlace();

        PlaceNextShipCommand = new RelayCommand(param => PromptForNextShip());
        ToggleOrientationCommand = new RelayCommand(param => ToggleOrientation());
    }

    public void InitializeAdditional(object param)
    {
        if (param is AddGameContext gameContext)
        {
            _gameId = gameContext.GameId;
        }
    }

    private void InitializeBoard()
    {
        for (int row = 0; row < _boardProps.Size; row++)
        {
            for (int col = 0; col < _boardProps.Size; col++)
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

    private void InitializeShipsToPlace()
    {
        _shipsToPlace.Enqueue((4, _boardProps.FourDeck));
        _shipsToPlace.Enqueue((3, _boardProps.ThreeDeck));
        _shipsToPlace.Enqueue((2, _boardProps.TwoDeck));
        _shipsToPlace.Enqueue((1, _boardProps.OneDeck));

        PromptForNextShip();
    }

    private void PromptForNextShip()
    {
        if (_shipsToPlace.Count > 0)
        {
            var (decks, remainingCount) = _shipsToPlace.Peek();
            _currentShipDecks = decks;

            MessageBox.Show($"Place ship with {decks} decks, remaining: {remainingCount}. Select a starting cell and specify orientation.");
        }
        else
        {
            MessageBox.Show("All ships placed. Game ready to start!");
        }
    }

    private void PlaceShip(CellViewModel cell)
    {
        if (_shipsToPlace.Count == 0)
        {
            MessageBox.Show("WAIT THE FUCK UP! YOU HAVE ALREADY PLACED ALL YOUR SHIPS!!");
            return;
        }

        int playerId = _sessionService.ActiveUser.Id;
        Vector2D startCoords = new Vector2D(cell.Row, cell.Column);

        ShipModel ship = new ShipModel(_currentShipDecks, _isHorizontal, startCoords);
        bool isPlaced = _gameService.PlaceShip(_gameId, playerId, ship);

        if (isPlaced)
        {
            UpdateBoardUI(playerId);

            var (decks, remainingCount) = _shipsToPlace.Dequeue();
            remainingCount--;

            if (remainingCount > 0)
            {
                _shipsToPlace.Enqueue((decks, remainingCount));
            }

            if (_shipsToPlace.Count > 0)
            {
                PromptForNextShip();
            }
            else
            {
                _gameService.SetPlayerReady(_gameId, _sessionService.ActiveUser.Id, true);
                StartReadyCheckTimer();
                MessageBox.Show("All ships placed! Game is ready to start.");
            }
        }
        else
        {
            MessageBox.Show("Ship could not be placed here. Try another position.");
        }
    }

    public void ToggleOrientation()
    {
        _isHorizontal = !_isHorizontal;
        MessageBox.Show($"Orientation changed to {(_isHorizontal ? "Vertical" : "Horizontal")}.");
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

    private void StartReadyCheckTimer()
    {
        _readyCheckTimer = new System.Timers.Timer(2000);
        _readyCheckTimer.Elapsed += (sender, args) =>
        {
            CheckIfBothPlayersReady();
        };
        _readyCheckTimer.AutoReset = true;
        _readyCheckTimer.Start();
    }

    private void StopReadyCheckTimer()
    {
        _readyCheckTimer?.Stop();
        _readyCheckTimer?.Dispose();
    }

    private void CheckIfBothPlayersReady()
    {
        if (_gameService.AreBothPlayersReady(_gameId))
        {
            Application.Current.Dispatcher.Invoke(StopReadyCheckTimer);
            HandleSuccess();
        }
    }

    private void HandleSuccess()
    {
        Application.Current.Dispatcher.Invoke(() =>
        {
            MessageBox.Show("Success!");
            _navigationService.NavigateTo<BattleViewModel>(new AddGameContext(_gameId));
        });
    }
}
