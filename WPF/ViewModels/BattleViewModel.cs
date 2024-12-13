using System.Collections.ObjectModel;

using SeaBattle.Services;
using SeaBattle.Managers;
using SeaBattle.Utils;
using SeaBattle.Contexts;
using SeaBattle.Common;

namespace SeaBattle.ViewModels;

public class BattleViewModel : BaseViewModel, IInitializable
{
    private readonly INavigationService _navigationService;
    private readonly ActionService _actionService;
    private readonly GameStateService _gameStateService;
    private readonly TurnService _turnService;
    private readonly BoardProps _props;
    public ObservableCollection<CellViewModel> MyBoard { get; } = new ObservableCollection<CellViewModel>();
    public ObservableCollection<CellViewModel> OpponentBoard { get; } = new ObservableCollection<CellViewModel>();

    private  TurnManager _turnManager;

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
    private bool _isGameOver;
    public bool IsGameOver
    {
        get => _isGameOver;
        private set
        {
            if (_isGameOver != value)
            {
                _isGameOver = value;
                OnPropertyChanged(nameof(IsGameOver));  
                OnPropertyChanged(nameof(CanFire));
            }
        }
    }

    public RelayCommand FireAtOpponentCommand { get; }
    public bool CanFire => IsMyTurn;
    private int _gameId;

    public BattleViewModel(GameStateService gameStateService, TurnService turnService, ActionService actionService, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _actionService = actionService;
        _turnService = turnService;
       _gameStateService = gameStateService;
       _props = new BoardProps();

        FireAtOpponentCommand = new RelayCommand(
            param => FireAtOpponent((CellViewModel)param),
            param => CanFire && !IsGameOver);
        InitializeBoards();
    }

    public void InitializeAdditional(object param)
    {
        if (param is AddGameContext gameContext)
        {
            _gameId = gameContext.GameId;
            _turnManager = new TurnManager(_turnService, UpdateTurnState, _gameId);
            _turnManager.TurnChanged += OnTurnChanged;
            UpdatePlayerBoard();
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

    public void UpdatePlayerBoard()
    {
        var cells = _gameStateService.GetPlayerBoard(_gameId);

        App.Current.Dispatcher.Invoke(() =>
        {
            foreach (var cell in cells)
            {
                var uiCell = MyBoard[cell.Row * _props.Size + cell.Col];
                uiCell.State = cell.State.ToString();
            }
            OnPropertyChanged(nameof(MyBoard));
        });
    }

    private void FireAtOpponent(CellViewModel cell)
    {
        if (_actionService.FireAtOpponent(_gameId, cell.Row, cell.Column))
        {
            cell.State = "Hit";
        }
        else
        {
            _turnManager.SwitchTurn();
            cell.State = "Miss";
        }
    }

    private void OnTurnChanged(object sender, EventArgs e)
    {
        UpdateTurnState();
        UpdatePlayerBoard();
        CheckGameOver();
    }

    private void UpdateTurnState()
    {
        App.Current.Dispatcher.Invoke(() =>
        {
            IsMyTurn = _turnManager.CheckTurnState();
        });
    }

    private void CheckGameOver()
    {
        var gameResult = _gameStateService.CheckAndGetGameResult(_gameId);
        if (gameResult.IsGameOver)
        {
            IsGameOver = true;
            _gameStateService.HandleGameOver(_gameId, gameResult.WinnerId, gameResult.Score);
        }
    }


    public void Dispose()
    {
        _turnManager.Dispose();
    }
}
