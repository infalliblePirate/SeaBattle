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

    public async Task AddUserAsync(UserEntity user) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var insertQuery = "INSERT INTO users (username, passwordhash) VALUES (@username, @passwordhash);";
        using var command2 = new NpgsqlCommand(insertQuery, connection);
        command2.Parameters.AddWithValue("username", user.Username);
        command2.Parameters.AddWithValue("passwordhash", user.PasswordHash);

        await command2.ExecuteNonQueryAsync();
    }


}