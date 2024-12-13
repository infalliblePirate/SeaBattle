using System.Windows;

using SeaBattle.Models;
using SeaBattle.Dtos;

namespace SeaBattle.Services;

public class GameStateService
{
    private readonly GameService _gameService;
    private readonly UserService _userService;
    private readonly SessionService _sessionService;

    public GameStateService(GameService gameService, UserService userService, SessionService sessionService)
    {
        _gameService = gameService;
        _userService = userService;
        _sessionService = sessionService;
    }

    public IEnumerable<CellModel> GetPlayerBoard(int gameId)
    {
        int playerId = _sessionService.ActiveUser.Id;
        return _gameService.GetNotBlockedCellsForPlayer(gameId, playerId);
    }

    public GameResultDto CheckAndGetGameResult(int gameId)
    {
        var result = _gameService.GetGameResult(gameId);
        if (result.IsGameOver)
        {
            return new GameResultDto
            {
                IsGameOver = true,
                WinnerId = result.WinnerId,
                Score = result.Score
            };
        }

        return new GameResultDto { IsGameOver = false };
    }

    public void HandleGameOver(int gameId, int? winnerId, int? gameScore)
    {
        if (winnerId.HasValue && gameScore.HasValue)
        {
            var activeUserId = _sessionService.ActiveUser.Id;
            var opponentId = _sessionService.ActiveUser.Id == winnerId ? winnerId.Value : _gameService.GetOpponentId(gameId, activeUserId);
            var gameScoreValue = gameScore.Value;

            if (winnerId == activeUserId)
            {
                _userService.UpdateUserScore(activeUserId, gameScoreValue);
                _userService.UpdateUserScore(opponentId, -gameScoreValue);
            }
            else
            {
                _userService.UpdateUserScore(activeUserId, -gameScoreValue);
                _userService.UpdateUserScore(opponentId, gameScoreValue);
            }
        }
    }

    public void ShowGameOverMessage(int? winnerId)
    {
        string winnerMessage = winnerId == _sessionService.ActiveUser.Id ? "You Win!" : "You Lose!";
        MessageBox.Show(winnerMessage, "Game Over", MessageBoxButton.OK, MessageBoxImage.Information);
    }

}
