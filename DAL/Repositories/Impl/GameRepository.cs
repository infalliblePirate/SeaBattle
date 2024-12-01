using Npgsql;

using SeaBattle.Entities;
namespace SeaBattle.Data;

public class GameRepository : IGameRepository {
    private readonly string _connectionString;

    public GameRepository(string connectionString) {
        _connectionString = connectionString;
    }

    public List<GameEntity> GetPlayedGamesByUsername(string username) {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        return new List<GameEntity>();
    }

    public int AddGame(GameEntity game) {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO games (user1_id, player1_board, player2_board, is_player1_turn) " +
                "VALUES (@User1Id, @Player1BoardSerialized, @Player2BoardSerialized, @IsPlayer1Turn) RETURNING id;";
        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("User1Id", game.User1Id);
        command.Parameters.AddWithValue("Player1BoardSerialized", NpgsqlTypes.NpgsqlDbType.Jsonb, game.Player1BoardSerialized);
        command.Parameters.AddWithValue("Player2BoardSerialized", NpgsqlTypes.NpgsqlDbType.Jsonb, game.Player2BoardSerialized);
        command.Parameters.AddWithValue("IsPlayer1Turn", game.IsPlayer1Turn);

        var result = command.ExecuteScalar();
    
        return (int)result; // id
    }

    public GameEntity GetGameById(int id) {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
    
         var query = @"
            SELECT id, user1_id, user2_id, winner_id, player1_board, player2_board, is_player1_turn, score
            FROM games
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new GameEntity
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                User1Id = reader.GetInt32(reader.GetOrdinal("user1_id")),
                User2Id = reader.IsDBNull(reader.GetOrdinal("user2_id")) ? null : reader.GetInt32(reader.GetOrdinal("user2_id")),
                WinnerId = reader.IsDBNull(reader.GetOrdinal("winner_id")) ? null : reader.GetInt32(reader.GetOrdinal("winner_id")),
                Score = reader.GetInt32(reader.GetOrdinal("score")),
                Player1BoardSerialized = reader.GetString(reader.GetOrdinal("player1_board")),
                Player2BoardSerialized = reader.GetString(reader.GetOrdinal("player2_board")),
                IsPlayer1Turn = reader.GetBoolean(reader.GetOrdinal("is_player1_turn"))
            };
        }

        throw new ArgumentNullException($"Could not fetch game with specified id: {id}");
    }

    public void UpdateGame(GameEntity game) {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.OpenAsync();

        var query = @" UPDATE games
            SET 
                user1_id = @User1Id,
                user2_id = @User2Id,
                winner_id = @WinnerId,
                score = @Score,
                player1_board = @Player1BoardSerialized,
                player2_board = @Player2BoardSerialized,
                is_player1_turn = @IsPlayer1Turn
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("User1Id", game.User1Id);
        command.Parameters.AddWithValue("User2Id", game.User2Id.HasValue ? game.User2Id.Value : DBNull.Value);
        command.Parameters.AddWithValue("Player1BoardSerialized", NpgsqlTypes.NpgsqlDbType.Jsonb, game.Player1BoardSerialized);
        command.Parameters.AddWithValue("Player2BoardSerialized", NpgsqlTypes.NpgsqlDbType.Jsonb, game.Player2BoardSerialized);
        command.Parameters.AddWithValue("WinnerId", game.WinnerId.HasValue ? game.WinnerId.Value : DBNull.Value);
        command.Parameters.AddWithValue("Score", game.Score.HasValue ? game.Score.Value : DBNull.Value);
        command.Parameters.AddWithValue("IsPlayer1Turn", game.IsPlayer1Turn);
        command.Parameters.AddWithValue("Id", game.Id);

        command.ExecuteNonQuery();
    }
    
}