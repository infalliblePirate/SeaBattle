using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;

namespace SeaBattle.Services;

public class GameService {
    private readonly IGameRepository _gameRepository;
    public GameService(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }

    public async Task<GameModel> CreateGame() {
        if (SessionService.Activeuser?.Id == null) {
            throw new InvalidOperationException("No active user session found.");
        }

        int joinedPlayerId = SessionService.Activeuser.Id;
        var gameEntity = new GameEntity { User1Id = joinedPlayerId };

        try {
            int gameId = await _gameRepository.AddGameAsync(gameEntity);
            return new GameModel {
                Id = gameId,
                User1Id = gameEntity.User1Id,
            };
        } catch (Exception ex) {
            Console.Error.WriteLine($"Error creating game: {ex.Message}");
            return null;
        }
    }
}