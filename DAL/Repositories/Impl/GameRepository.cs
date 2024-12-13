using Npgsql;
using SeaBattle.Entities;
using SeaBattle.Dtos;

namespace SeaBattle.Data;

public class GameRepository : IGameRepository
{
    private readonly string _connectionString;

    public GameRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }

    public List<GameSummaryDto> GetGameSummariesByUserId(int userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            SELECT 
                CASE 
                    WHEN user1_id = @UserId THEN user2_id 
                    ELSE user1_id 
                END AS opponent_id,
                winner_id,
                score
            FROM games
            WHERE user1_id = @UserId OR user2_id = @UserId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@UserId", userId);

        using var reader = command.ExecuteReader();
        var gameSummaries = new List<GameSummaryDto>();

        while (reader.Read())
        {
            gameSummaries.Add(new GameSummaryDto
            {
                OpponentId = reader.IsDBNull(reader.GetOrdinal("opponent_id")) 
                            ? null : reader.GetInt32(reader.GetOrdinal("opponent_id")),
                WinnerId = reader.IsDBNull(reader.GetOrdinal("winner_id")) 
                        ? null : reader.GetInt32(reader.GetOrdinal("winner_id")),
                Score = reader.IsDBNull(reader.GetOrdinal("score")) 
                        ? null : reader.GetInt32(reader.GetOrdinal("score"))
            });
        }

        return gameSummaries;
    }


    public int CreateGame(int user1Id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            INSERT INTO games (user1_id, is_player1_turn, is_player1_ready, is_player2_ready)
            VALUES (@User1Id, @IsPlayer1Turn, @IsPlayer1Ready, @IsPlayer2Ready)
            RETURNING id;";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("@User1Id", user1Id);
        command.Parameters.AddWithValue("@IsPlayer1Turn", true);
        command.Parameters.AddWithValue("@IsPlayer1Ready", false);
        command.Parameters.AddWithValue("@IsPlayer2Ready", false);

        var result = command.ExecuteScalar();
        return Convert.ToInt32(result);
    }


    public void AddOpponentToGame(int gameId, int opponentId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            UPDATE games
            SET user2_id = @OpponentId
            WHERE id = @GameId AND user2_id IS NULL;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@GameId", gameId);
        command.Parameters.AddWithValue("@OpponentId", opponentId);

        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Unable to add opponent to game.");
        }
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

    public int GetOpponentId(int gameId, int playerId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = @"
            SELECT 
                CASE 
                    WHEN user1_id = @PlayerId THEN user2_id
                    WHEN user2_id = @PlayerId THEN user1_id
                END as opponent_id
            FROM games
            WHERE id = @GameId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("PlayerId", playerId);

        var result = command.ExecuteScalar();
        if (result != DBNull.Value && result != null)
        {
            return (int)result;
        }

        throw new InvalidOperationException("Opponent not found.");
    }

    public bool IsPlayerTurn(int gameId, int playerId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            SELECT 
                (is_player1_turn AND user1_id = @PlayerId) OR
                (NOT is_player1_turn AND user2_id = @PlayerId) AS is_turn
            FROM games
            WHERE id = @GameId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("PlayerId", playerId);

        var result = command.ExecuteScalar();

        if (result != null && result != DBNull.Value)
        {
            return Convert.ToBoolean(result);
        }

        throw new InvalidOperationException("Game or player not found.");
    }

    public void SwitchTurn(int gameId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            UPDATE games
            SET is_player1_turn = NOT is_player1_turn
            WHERE id = @GameId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@GameId", gameId);

        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Unable to switch turn. Game not found.");
        }
    }

    public void UpdateWinner(int gameId, int winnerId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        const string query = @"
            UPDATE games
            SET winner_id = @WinnerId
            WHERE id = @GameId;";
        
        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("@GameId", gameId);
        command.Parameters.AddWithValue("@WinnerId", winnerId);

        var rowsAffected = command.ExecuteNonQuery();
        if (rowsAffected == 0)
        {
            throw new InvalidOperationException("Unable to set winner. Game not found.");
        }
    }
    
}
