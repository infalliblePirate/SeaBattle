using Npgsql;

using SeaBattle.Models;
namespace SeaBattle.Data;
public class UserRepository : IUserRepository {
    private readonly string _connectionString;

    public UserRepository(string connectionString) {
        _connectionString = connectionString;
    }

    public async Task<User> GetUserByUsernameAsync(string username) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new NpgsqlCommand("SELECT Id, Username, PasswordHash FROM Users WHERE Username = @Username", connection);
        command.Parameters.AddWithValue("Username", username);

        using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2)
            };
        }

        return null;
    }

    public async Task AddUserAsync(User user) {
        using var connection = new NpgsqlConnection(_connectionString);
        await connection.OpenAsync();

        var command = new NpgsqlCommand("INSERT INTO Users (Usernmae, PasswordHash) VALUES (@Username, @PasswordHash)", connection);
        command.Parameters.AddWithValue("Username", user.Username);
        command.Parameters.AddWithValue("PaswordHash", user.PasswordHash);

        await command.ExecuteNonQueryAsync();
    }
}