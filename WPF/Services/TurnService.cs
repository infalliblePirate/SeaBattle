namespace SeaBattle.Services;

public class TurnService
{
    private readonly GameService _gameService;
    private readonly SessionService _sessionService;

    public TurnService(GameService gameService, SessionService sessionService)
    {
        _gameService = gameService;
        _sessionService = sessionService;
    }

    public bool IsPlayerTurn(int gameId)
    {
        int playerId = _sessionService.ActiveUser.Id;
        return _gameService.IsPlayerTurn(gameId, playerId);
    }

    public void SwitchTurn(int gameId)
    {
        _gameService.SwitchTurn(gameId);
    }
}
