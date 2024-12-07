namespace SeaBattle.Entities;

public class CellEntity {
    public int Id { get; set; }
    public int GameId { get; set; }
    public int UserId { get; set; }
    public int Row { get; set; }
    public int Col { get; set; }
    public string State { get; set; } = "Empty";
}