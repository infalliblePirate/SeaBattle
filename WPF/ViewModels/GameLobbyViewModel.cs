using SeaBattle.Contexts;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Views;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;

public class GameLobbyViewModel : BaseViewModel, IInitializable
{
    private string _gameCode;
    private readonly INavigationService _navigationService;
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;

    public ICommand EnterGameCommand { get; }
    public ICommand CreateGameCommand { get; }
    public ICommand ViewHistoryCommand { get; }

    public string GameCode 
    {
        get => _gameCode;
        set 
        {
            _gameCode = value;
            OnPropertyChanged();
            ((RelayCommand)EnterGameCommand).RaiseCanExecuteChanged();
        }
    }

    public void InitializeAdditional(object param) {}

    public GameLobbyViewModel(GameService gameService, SessionService sessionService, INavigationService navigationService)
    {
        _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        EnterGameCommand = new RelayCommand((param) => OnEnter(), CanEnterGame);
        CreateGameCommand = new RelayCommand((param) => OnCreate(), CanCreateGame);
        ViewHistoryCommand = new RelayCommand((param) => OnViewHistory());
    }

    private bool CanEnterGame(object param) => true;
    private bool CanCreateGame(object param) => true;

    private void OnCreate()
    {
        try {
            int? gameId = _gameService.CreateGame(_sessionService.ActiveUser.Id);
            if (gameId.HasValue)
            {
                MessageBox.Show($"Game created with ID: {gameId.Value}");
                _navigationService.NavigateTo<PlaceShipsViewModel>(new AddGameContext(gameId.Value));
            }
            else
            {
                MessageBox.Show("Failed to create game.");
            }
        } catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    private void OnEnter()
    {
        try {
            if (int.TryParse(GameCode, out int gameId))
            {
                _gameService.AddOpponentToGame(gameId, _sessionService.ActiveUser.Id);
                MessageBox.Show("Success.");
                _navigationService.NavigateTo<PlaceShipsViewModel>(new AddGameContext(gameId));
            } else
            {
                MessageBox.Show("Please enter a valid game code.");
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    private void OnViewHistory()
    {
        _navigationService.NavigateTo<GameHistoryViewModel>(_sessionService.ActiveUser.Id);
    }

}