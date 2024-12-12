using SeaBattle.Entities;

namespace SeaBattle.Data;

public interface IGameRepository 
{
    List<GameEntity> GetPlayedGamesByUsername(string username);
    int CreateGame(int user1Id);
    void AddOpponentToGame(int game, int oppoenntId);
    GameEntity GetGameById(int id);
    int GetOpponentId(int gameId, int playerId);
    void UpdateGame(GameEntity game);
    void UpdatePlayerReadyStatus(int gameId, int playerId, bool isReady);
    bool IsPlayerTurn(int gameId, int playerId);
    void SwitchTurn(int gameId);
} 