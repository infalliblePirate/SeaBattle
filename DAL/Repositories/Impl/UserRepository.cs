using Microsoft.EntityFrameworkCore.Update.Internal;
using Npgsql;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public class UserRepository : IUserRepository {
    private readonly string _connectionString;

    public UserRepository(string connectionString) {
        _connectionString = connectionString;
    }

   public async Task<UserEntity> GetUserByUsernameAsync(string username) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "SELECT Id, username, passwordhash FROM users WHERE username = @username";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("username", username);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync()) {
            return new UserEntity {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2)
            };
        }

        return null;
    }

    public async Task<int> AddUserAsync(UserEntity user) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var query = "INSERT INTO users (username, passwordhash) VALUES (@username, @passwordhash) RETURNING id;";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("passwordhash", user.PasswordHash);

        var result = await command.ExecuteScalarAsync();
    
        // Return the Id of the newly created user
        return (int)result;
    }

    public async Task JoinGame(int gameId, int userId) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();
        var query = "UPDATE games SET user2_id = @joinedUserId WHERE id = @id;";
        
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("joinedUserId", userId);
        command.Parameters.AddWithValue("id", gameId);

        await command.ExecuteNonQueryAsync();
    }


}