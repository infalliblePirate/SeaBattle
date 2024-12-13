namespace SeaBattle.Services;

public class ActionService
{
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;

    public ActionService(GameService gameService, SessionService sessionService)
    {
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public bool FireAtOpponent(int gameId, int row, int col)
    {
        int playerId = _sessionService.ActiveUser.Id;
        int opponentId = _gameService.GetOpponentId(gameId, playerId);
        bool isHit = _gameService.FireAtOpponent(gameId, opponentId, row, col);
        return isHit;
    }
}
