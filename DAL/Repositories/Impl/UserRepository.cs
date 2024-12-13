using Npgsql;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public class UserRepository : IUserRepository
{
    private readonly string _connectionString;

    public UserRepository(string connectionString) 
    {
        _connectionString = connectionString;
    }

    public UserEntity GetUserByUsername(string username)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT Id, username, passwordhash FROM users WHERE username = @username";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("username", username);

        using var reader = command.ExecuteReader();
        if (reader.Read()) 
        {
            return new UserEntity 
            {
                Id = reader.GetInt32(0),
                Username = reader.GetString(1),
                PasswordHash = reader.GetString(2)
            };
        }

        return null;
    }

    public int AddUser(UserEntity user)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = "INSERT INTO users (username, passwordhash) VALUES (@username, @passwordhash) RETURNING id;";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("username", user.Username);
        command.Parameters.AddWithValue("passwordhash", user.PasswordHash);

        var result = command.ExecuteScalar();
    
        return (int)result;
    }

    public string GetUserNameById(int userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = "SELECT username FROM users WHERE id = @userId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("userId", userId);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return reader.GetString(0); 
        }

        return null;
    }

    public void UpdateUserScore(int playerId, int scoreChange)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = "UPDATE users SET score = GREATEST(score + @ScoreChange, 0) WHERE id = @PlayerId";
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@ScoreChange", scoreChange);
        command.Parameters.AddWithValue("@PlayerId", playerId);

        command.ExecuteNonQuery();
    }
}