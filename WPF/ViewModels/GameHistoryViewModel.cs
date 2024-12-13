using SeaBattle.Contexts;
using SeaBattle.Dtos;
using SeaBattle.Services;
using SeaBattle.Utils;
using SeaBattle.Views;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace SeaBattle.ViewModels
{
    public class GameHistoryViewModel : BaseViewModel, IInitializable
    {
        private readonly GameService _gameService;
        private readonly SessionService _sessionService;
        private readonly UserService _userService;
        private readonly INavigationService _navigationService;

        public ObservableCollection<GameHistoryItemViewModel> PlayedGames { get; }
        public ICommand LoadHistoryCommand { get; }

        public GameHistoryViewModel(GameService gameService, UserService userService, SessionService sessionService, INavigationService navigationService)
        {
            _gameService = gameService;
            _sessionService = sessionService;
            _userService = userService;
            _navigationService = navigationService;

            PlayedGames = new ObservableCollection<GameHistoryItemViewModel>();
            LoadHistoryCommand = new RelayCommand((param) => LoadGameHistory(), CanLoadGameHistory);
            LoadGameHistory();
        }

        public void InitializeAdditional(object param) { }

        private void LoadGameHistory()
        {
            int userId = _sessionService.ActiveUser.Id;
            List<GameSummaryDto> playedGames = _gameService.GetGameSummaries(userId);

            PlayedGames.Clear(); 

            foreach (var game in playedGames)
            {
                string opponentName = game.OpponentId.HasValue ? _userService.GetUsernameById(game.OpponentId.Value) : "Unknown";
                string winnerName = game.WinnerId.HasValue ? _userService.GetUsernameById(game.WinnerId.Value) : "Unknown";
                
                PlayedGames.Add(new GameHistoryItemViewModel
                {
                    GameInfo = $"Player played with {opponentName}",
                    Status = $"Winner: {winnerName}, Score: {game.Score ?? 0}"
                });
            }
        }

        private bool CanLoadGameHistory(object param)
        {
            return _sessionService.ActiveUser != null;
        }
    }

    public class GameHistoryItemViewModel
    {
        public string GameInfo { get; set; }
        public string Status { get; set; }
    }
}
