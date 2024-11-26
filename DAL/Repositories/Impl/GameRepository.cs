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

    public async Task AddGameAsync(GameEntity game) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO games (user1_id, user2_id, winner_id, score) VALUES (@User1Id, @User2Id, @WinnerId, @Score)";
        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("User1Id", game.User1Id);
        command.Parameters.AddWithValue("User2Id", game.User2Id);
        command.Parameters.AddWithValue("WinnerId", game.WinnerId);
        command.Parameters.AddWithValue("Score", game.Score);

        await command.ExecuteNonQueryAsync();
    }

    // todo: how to update, mb pass game?
    public async Task AddUserToGame(UserEntity user) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

    }

    
}