using Npgsql;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public class GameRepository : IGameRepository {
    private readonly string _connectionString;

    public GameRepository(string connectionString) {
        _connectionString = connectionString;
    }

    public async Task<List<GameEntity>> GetPlayedGamesByUsernameAsync(string username) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        return new List<GameEntity>();
    }

    public async Task<int> AddGameAsync(GameEntity game) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO games (user1_id) VALUES (@User1Id) RETURNING id;";
        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("User1Id", game.User1Id);

         var result = await command.ExecuteScalarAsync();
    
        // Return the Id
        return (int)result;
    }

    // todo: how to update, mb pass game?
    // public async Task AddUserToGame(UserEntity user) {
    //     using var connection = new NpgsqlConnection(_connectionString);
    //     await connection.OpenAsync();

    // }

    
}