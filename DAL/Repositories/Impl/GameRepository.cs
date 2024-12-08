using Npgsql;
using SeaBattle.Entities;

namespace SeaBattle.Data;

public class GameRepository : IGameRepository
{
    private readonly string _connectionString;

    public GameRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public List<GameEntity> GetPlayedGamesByUsername(string username)
    {
        return new List<GameEntity>();
    }

    public int AddGame(GameEntity game)
    {
        if (game == null) throw new ArgumentNullException(nameof(game));

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            INSERT INTO games (user1_id, is_player1_turn, is_player1_ready, is_player2_ready)
            VALUES (@User1Id, @IsPlayer1Turn, @IsPlayer1Ready, @IsPlayer2Ready)
            RETURNING id;";
        
        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@User1Id", game.User1Id);
        command.Parameters.AddWithValue("@IsPlayer1Turn", game.IsPlayer1Turn);
        command.Parameters.AddWithValue("@IsPlayer1Ready", game.IsPlayer1Ready);
        command.Parameters.AddWithValue("@IsPlayer2Ready", game.IsPlayer2Ready);

        var result = command.ExecuteScalar();
        return Convert.ToInt32(result);
    }

    public GameEntity GetGameById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            SELECT id, user1_id, user2_id, winner_id, is_player1_turn, 
                   is_player1_ready, is_player2_ready, score
            FROM games
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new GameEntity
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                User1Id = reader.GetInt32(reader.GetOrdinal("user1_id")),
                User2Id = reader.IsDBNull(reader.GetOrdinal("user2_id")) 
                          ? null : reader.GetInt32(reader.GetOrdinal("user2_id")),
                WinnerId = reader.IsDBNull(reader.GetOrdinal("winner_id")) 
                           ? null : reader.GetInt32(reader.GetOrdinal("winner_id")),
                IsPlayer1Ready = reader.GetBoolean(reader.GetOrdinal("is_player1_ready")),
                IsPlayer2Ready = reader.GetBoolean(reader.GetOrdinal("is_player2_ready")),
                Score = reader.IsDBNull(reader.GetOrdinal("score")) 
                        ? null : reader.GetInt32(reader.GetOrdinal("score")),
                IsPlayer1Turn = reader.GetBoolean(reader.GetOrdinal("is_player1_turn"))
            };
        }

        throw new InvalidOperationException($"Game with ID {id} not found.");
    }

    public void UpdatePlayerReadyStatus(int gameId, int playerId, bool isReady)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            UPDATE games
            SET 
                is_player1_ready = CASE WHEN user1_id = @PlayerId THEN @IsReady ELSE is_player1_ready END,
                is_player2_ready = CASE WHEN user2_id = @PlayerId THEN @IsReady ELSE is_player2_ready END
            WHERE id = @GameId AND (user1_id = @PlayerId OR user2_id = @PlayerId);";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("IsReady", isReady);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("PlayerId", playerId);

        int rowsAffected = command.ExecuteNonQuery();

        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("No matching game or player found for the given ID.");
        }
    }



    public void UpdateGame(GameEntity game)
    {
        if (game == null) throw new ArgumentNullException(nameof(game));

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            UPDATE games
            SET 
                user1_id = @User1Id,
                user2_id = @User2Id,
                winner_id = @WinnerId,
                score = @Score,
                is_player1_turn = @IsPlayer1Turn,
                is_player1_ready = @IsPlayer1Ready,
                is_player2_ready = @IsPlayer2Ready
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@User1Id", game.User1Id);
        command.Parameters.AddWithValue("@User2Id", game.User2Id ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@WinnerId", game.WinnerId ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Score", game.Score ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@IsPlayer1Turn", game.IsPlayer1Turn);
        command.Parameters.AddWithValue("@IsPlayer1Ready", game.IsPlayer1Ready);
        command.Parameters.AddWithValue("@IsPlayer2Ready", game.IsPlayer2Ready);
        command.Parameters.AddWithValue("@Id", game.Id);

        command.ExecuteNonQuery();
    }
}
