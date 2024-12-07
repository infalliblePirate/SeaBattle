using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Views;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels;

public class CreateEnterGameViewModel : BaseViewModel 
{
    private string _gameCode;
    private readonly UserService _userService;
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;

    public ICommand EnterGameCommand { get; }
    public ICommand CreateGameCommand { get; }

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

    public CreateEnterGameViewModel(UserService userService, GameService gameService, SessionService sessionService)
    {
        _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
        _gameService = gameService ?? throw new ArgumentNullException(nameof(gameService));

        EnterGameCommand = new RelayCommand((param) => OnEnter(), CanEnterGame);
        CreateGameCommand = new RelayCommand((param) => OnCreate(), CanCreateGame);
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
                var placeShipsVM = new PlaceShipsViewModel(gameId.Value, _gameService, _sessionService);
                var placeShipsScreen = new PlaceShipsScreen(placeShipsVM);
                placeShipsScreen.Show();
            }
            else
            {
                MessageBox.Show("Failed to create game.");
            }
        } catch (Exception ex) {
            MessageBox.Show($"An error occurred: {ex.Message}");
        }
    }

    private void OnEnter()
    {
        try {
            if (int.TryParse(GameCode, out int gameId))
            {
                _userService.JoinGame(gameId, _sessionService.ActiveUser.Id);
                var placeShipsVM = new PlaceShipsViewModel(gameId,_gameService, _sessionService);
                var placeShipsScreen = new PlaceShipsScreen(placeShipsVM);
                placeShipsScreen.Show();
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


}