namespace SeaBattle.Dtos;

public class GameResultDto
{
    public int? WinnerId { get; set; }
    public int? Score { get; set; }
    public bool IsGameOver { get; set; }
}
