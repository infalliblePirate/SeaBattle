using SeaBattle.Common;

namespace SeaBattle.Models;

public class CellModel {
    public int Id { get; set; }
    public int GameId { get; set; }
    public int UserId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public CellState State { get; set; }
}