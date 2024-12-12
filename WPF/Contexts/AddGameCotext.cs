namespace SeaBattle.Contexts;

public class AddGameContext
{
    public int GameId { get; set; }
    public AddGameContext(int gameId)
    {
        GameId = gameId;
    }
}