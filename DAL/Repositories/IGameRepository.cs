using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public interface IGameRepository {
    Task<List<GameEntity>> GetPlayedGamesByUsernameAsync(string username);
    Task<int> AddGameAsync(GameEntity game);
    Task<GameEntity> GetGameByIdAsync(int id);
    void UpdateGameAsync(GameEntity game);
} 