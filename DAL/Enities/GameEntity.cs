namespace SeaBattle.Entities;

public class GameEntity {
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int? User2Id { get; set; }
    public int? WinnerId { get; set; }
    public int? Score { get; set; } = 10;
    public bool IsPlayer1Turn { get; set; } = true;
}