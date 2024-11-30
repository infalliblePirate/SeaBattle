using SeaBattle.Models;

namespace SeaBattle.Dtos;

public class GameDto {
    public int Id { get; set; }
    public int User1Id { get; set; }
    public int? User2Id { get; set; }
    public int? WinnerId { get; set; }
    public int? Score { get; set; }

    public BoardDto Player1Board { get; set; }
    public BoardDto Player2Board { get; set; }
    public bool IsPlayer1Turn { get; set; }

    public GameDto(GameModel model) {
        if (model == null) throw new ArgumentNullException(nameof(model));

        Id = model.Id;
        User1Id = model.User1Id;
        User2Id = model.User2Id;
        WinnerId = model.WinnerId;
        Score = model.Score;
        Player1Board = new BoardDto(model.Player1Board);
        Player2Board = new BoardDto(model.Player2Board);
        IsPlayer1Turn = model.IsPlayer1Turn;
    }

}