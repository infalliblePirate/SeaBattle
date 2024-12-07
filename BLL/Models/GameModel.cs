namespace SeaBattle.Models;

public class GameModel {
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int? User2Id { get; set; }
    public int? WinnerId { get; set; }
    public int? Score { get; set; }
    public bool IsPlayer1Turn { get; set; } = true;

}

