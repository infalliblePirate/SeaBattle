using SeaBattle.Entities;

namespace SeaBattle.Data;

public interface ICellRepository 
{
    int AddCell(CellEntity cell);
    void UpdateCell(CellEntity cell);
    bool IsCellAlreadyStored(int row, int col, int gameId, int userId, string state);
    LinkedList<CellEntity> GetCellsForPlayer(int gameId, int playerId);
    LinkedList<CellEntity> GetNotBlockedCellsForPlayer(int gameId, int playerId);
    CellEntity GetCellById(int id);
    CellEntity GetCell(int row, int col, int gameId, int userId);
    bool FireAtOpponent(int gameId, int opponentId, int x, int y);
}