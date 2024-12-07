using System.Collections.ObjectModel;
using System.Windows;
using SeaBattle.Models;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Common;

namespace SeaBattle.ViewModels
{
    public class PlaceShipsViewModel : BaseViewModel
    {
        private readonly GameService _gameService;
        private readonly SessionService _sessionService;
        private readonly int _gameId;

        public ObservableCollection<CellViewModel> Board { get; } = new ObservableCollection<CellViewModel>();

        public RelayCommand PlaceNextShipCommand { get; }
        public RelayCommand ToggleOrientationCommand { get; }

        private readonly Queue<(int Decks, int RemainingCount)> _shipsToPlace = new Queue<(int Decks, int RemainingCount)>();
        private readonly BoardProps _boardProps;
        private int _currentShipDecks;
        private bool _isHorizontal = true;

        public PlaceShipsViewModel(int gameId, GameService gameService, SessionService sessionService)
        {
            _gameService = gameService;
            _sessionService = sessionService;
            _gameId = gameId;
            _boardProps = new BoardProps();

            InitializeBoard();
            InitializeShipsToPlace();

            PlaceNextShipCommand = new RelayCommand(param => PromptForNextShip());
            ToggleOrientationCommand = new RelayCommand(param => ToggleOrientation());
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
    }
}
