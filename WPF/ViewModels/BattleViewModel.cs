using System;
using System.Collections.ObjectModel;
using SeaBattle.Views;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Contexts;
using SeaBattle.Common;
using System.Timers;
using System.Windows.Threading;

namespace SeaBattle.ViewModels
{
    public class BattleViewModel : BaseViewModel, IInitializable
    {
        private readonly INavigationService _navigationService;
        private readonly GameService _gameService;
        private readonly SessionService _sessionService;
        private int _gameId;
        private readonly BoardProps _props;

        private bool _isMyTurn;
        public bool IsMyTurn
        {
            get => _isMyTurn;
            private set
            {
                if (_isMyTurn != value)
                {
                    _isMyTurn = value;
                    OnPropertyChanged(nameof(CanFire));
                    FireAtOpponentCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public ObservableCollection<CellViewModel> MyBoard { get; } = new ObservableCollection<CellViewModel>();
        public ObservableCollection<CellViewModel> OpponentBoard { get; } = new ObservableCollection<CellViewModel>();

        public RelayCommand FireAtOpponentCommand { get; }
        public bool CanFire => IsMyTurn;

        private System.Timers.Timer _turnCheckerTimer;
        private DateTime _lastTurnChecked;

        public BattleViewModel(GameService gameService, SessionService sessionService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _gameService = gameService;
            _sessionService = sessionService;
            _props = new BoardProps();

            InitializeBoards();

            FireAtOpponentCommand = new RelayCommand
            (
                param => FireAtOpponent((CellViewModel)param),
                param => CanFire
            );

            _turnCheckerTimer = new System.Timers.Timer
            {
                Interval = 1000,
                AutoReset = true, 
                Enabled = true
            };
            _turnCheckerTimer.Elapsed += CheckTurnState;
        }

        public void InitializeAdditional(object param)
        {
            if (param is AddGameContext gameContext)
            {
                _gameId = gameContext.GameId;
                UpdateMyBoardUI();
                UpdateTurnState();
            }
        }

        private void InitializeBoards()
        {
            for (int row = 0; row < _props.Size; row++)
            {
                for (int col = 0; col < _props.Size; col++)
                {
                    MyBoard.Add(new CellViewModel { Row = row, Column = col, State = "Empty" });
                    OpponentBoard.Add(new CellViewModel { Row = row, Column = col, State = "Empty" });
                }
            }
        }

        private void FireAtOpponent(CellViewModel cell)
        {
            int playerId = _sessionService.ActiveUser.Id;
            int opponentId = _gameService.GetOpponentId(_gameId, playerId);

            bool isHit = _gameService.FireAtOpponent(_gameId, opponentId, cell.Row, cell.Column);
            cell.State = isHit ? "Hit" : "Miss";

            _gameService.SwitchTurn(_gameId);
            UpdateTurnState();
            UpdateMyBoardUI();
            OnPropertyChanged(nameof(OpponentBoard));
        }

        private void UpdateMyBoardUI()
        {
            int playerId = _sessionService.ActiveUser.Id;
            var cells = _gameService.GetNotBlockedCellsForPlayer(_gameId, playerId);

            App.Current.Dispatcher.Invoke(() =>
            {
                foreach (var cell in cells)
                {
                    var uiCell = MyBoard[cell.Row * 10 + cell.Col];
                    uiCell.State = cell.State.ToString();
                }
                OnPropertyChanged(nameof(MyBoard));
            });
        }

        private void UpdateTurnState()
        {
            int playerId = _sessionService.ActiveUser.Id;
            IsMyTurn = _gameService.IsPlayerTurn(_gameId, playerId);
        }

        private void CheckTurnState(object sender, ElapsedEventArgs e)
        {
            if ((DateTime.Now - _lastTurnChecked).TotalSeconds >= 1)
            {
                _lastTurnChecked = DateTime.Now;
                
                UpdateTurnState();
                UpdateMyBoardUI();
            }
        }

        public void Dispose()
        {
            _turnCheckerTimer.Stop();
            _turnCheckerTimer.Elapsed -= CheckTurnState;
        }
    }
}
