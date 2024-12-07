using Npgsql;

using SeaBattle.Common;
using SeaBattle.Entities;
namespace SeaBattle.Data;

public class CellRepository : ICellRepository
{
    private readonly string _connectionString;
    public CellRepository(string connectionString) 
    {
        _connectionString = connectionString;
    }
    
    public int AddCell(CellEntity cell)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = "INSERT INTO cells (row, col, game_id, player_id, state) " +
                "VALUES (@Row, @Col, @GameId, @UserId, @State) RETURNING id;";
        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("Row", cell.Row);
        command.Parameters.AddWithValue("Col", cell.Col);
        command.Parameters.AddWithValue("UserId", cell.UserId);
        command.Parameters.AddWithValue("GameId", cell.GameId);
        command.Parameters.AddWithValue("State", cell.State);

        var result = command.ExecuteScalar();
    
        return (int)result; // id
    }

    public CellEntity GetCellById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();
    
        var query = @"
            SELECT id, game_id, player_id, row, col, state
            FROM cells
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new CellEntity
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                GameId = reader.GetInt32(reader.GetOrdinal("game_id")),
                UserId = reader.GetInt32(reader.GetOrdinal("player_id")),
                Row = reader.GetInt32(reader.GetOrdinal("row")),
                Col = reader.GetInt32(reader.GetOrdinal("col")),
                State = reader.GetString(reader.GetOrdinal("state")),
            };
        }

        throw new ArgumentNullException($"Could not fetch game with specified id: {id}");
    }

    public bool IsCellAlreadyStored(int row, int col, int gameId, int userId, string state)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = @"
            SELECT COUNT(1)
            FROM cells
            WHERE row = @Row AND col = @Col AND game_id = @GameId AND player_id = @UserId AND state = @State;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Row", row);
        command.Parameters.AddWithValue("Col", col);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("UserId", userId);
        command.Parameters.AddWithValue("State", state);

        var count = (long)command.ExecuteScalar();
        return count > 0;
    }

    public LinkedList<CellEntity> GetCellsForPlayer(int gameId, int playerId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = @"
            SELECT id, game_id, player_id, row, col, state
            FROM cells
            WHERE game_id = @GameId AND player_id = @UserId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("UserId", playerId);

        using var reader = command.ExecuteReader();
        var cells = new LinkedList<CellEntity>();

        while (reader.Read())
        {
            var cell = new CellEntity
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                GameId = reader.GetInt32(reader.GetOrdinal("game_id")),
                UserId = reader.GetInt32(reader.GetOrdinal("player_id")),
                Row = reader.GetInt32(reader.GetOrdinal("row")),
                Col = reader.GetInt32(reader.GetOrdinal("col")),
                State = reader.GetString(reader.GetOrdinal("state")),
            };

            cells.AddLast(cell);
        }

        return cells;
    }

    public CellEntity GetCell(int row, int col, int gameId, int userId)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var query = @"
            SELECT id, game_id, player_id, row, col, state
            FROM cells
            WHERE row = @Row AND col = @Col AND game_id = @GameId AND player_id = @UserId;";

        using var command = new NpgsqlCommand(query, connection);
        command.Parameters.AddWithValue("Row", row);
        command.Parameters.AddWithValue("Col", col);
        command.Parameters.AddWithValue("GameId", gameId);
        command.Parameters.AddWithValue("UserId", userId);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            return new CellEntity
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                GameId = reader.GetInt32(reader.GetOrdinal("game_id")),
                UserId = reader.GetInt32(reader.GetOrdinal("player_id")),
                Row = reader.GetInt32(reader.GetOrdinal("row")),
                Col = reader.GetInt32(reader.GetOrdinal("col")),
                State = reader.GetString(reader.GetOrdinal("state")),
            };
        }

        throw new InvalidOperationException($"Could not fetch cell at row {row}, col {col} for game {gameId}, user {userId}");
    }


    public void UpdateCell(CellEntity cell)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.OpenAsync();

        var query = @" UPDATE cells
            SET 
                game_id = @GameId,
                player_id = @UserId,
                row = @Row,
                col = @Col,
                state = @State
            WHERE id = @Id;";

        using var command = new NpgsqlCommand(query, connection);

        command.Parameters.AddWithValue("UserId", cell.UserId);
        command.Parameters.AddWithValue("GameId", cell.GameId);
        command.Parameters.AddWithValue("Row", cell.Row);
        command.Parameters.AddWithValue("Col", cell.Col);
        command.Parameters.AddWithValue("State", cell.State);


        command.ExecuteNonQuery();
    }
}