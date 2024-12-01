using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public interface IGameRepository {
    List<GameEntity> GetPlayedGamesByUsername(string username);
    int AddGame(GameEntity game);
    GameEntity GetGameById(int id);
    void UpdateGame(GameEntity game);
} 