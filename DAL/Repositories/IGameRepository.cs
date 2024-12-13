using SeaBattle.Entities;
using SeaBattle.Dtos;

namespace SeaBattle.Data;

public interface IGameRepository 
{
    int CreateGame(int user1Id);
    void AddOpponentToGame(int game, int oppoenntId);
    GameEntity GetGameById(int id);
    int GetOpponentId(int gameId, int playerId);
    void UpdateGame(GameEntity game);
    void UpdatePlayerReadyStatus(int gameId, int playerId, bool isReady);
    bool IsPlayerTurn(int gameId, int playerId);
    void SwitchTurn(int gameId);
    void UpdateWinner(int gameId, int winnerId);
    List<GameSummaryDto> GetGameSummariesByUserId(int userId);
} 