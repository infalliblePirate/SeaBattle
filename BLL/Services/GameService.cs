using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
using SeaBattle.Mappers;

namespace SeaBattle.Services;

public class GameService 
{
    private readonly CellService _cellService;
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository, CellService cellService) 
    {
        _gameRepository = gameRepository;
        _cellService = cellService;
    }

    public int? CreateGame(int joinedPlayerId)
    {
        try 
        {
            int gameId = _gameRepository.CreateGame(joinedPlayerId);
            return gameId;
        } catch (Exception ex) 
        {
            Console.Error.WriteLine($"Error creating game: {ex.Message}");
            return null;
        }
    }

    public void AddOpponentToGame(int gameId, int opponentId)
    {
        var gameEntity = _gameRepository.GetGameById(gameId);
    
        if (gameEntity.User2Id != null)
        {
            throw new InvalidOperationException("Game already has two players.");
        }

        _gameRepository.AddOpponentToGame(gameId, opponentId);
    }

    public GameModel GetGameById(int id) 
    {
        var gameEntity = _gameRepository.GetGameById(id); 
        return GameMapper.ToGameModel(gameEntity);
    }

    public bool PlaceShip(int gameId, int playerId, ShipModel ship)
    {
        if (!_cellService.CanPlaceShip(gameId, playerId, ship))
        {
            return false;
        }

        _cellService.PlaceShip(gameId, playerId, ship);
        return true;
    }

    public List<CellModel> GetCellsForPlayer(int gameId, int playerId)
    {
        return _cellService.GetCellsForPlayer(gameId, playerId);
    }

    public List<CellModel> GetNotBlockedCellsForPlayer(int gameId, int playerId) 
    {
        return _cellService.GetNotBlockedCellsForPlayer(gameId, playerId);
    }

    public void SetPlayerReady(int gameId, int playerId, bool isReady)
    {
        _gameRepository.UpdatePlayerReadyStatus(gameId, playerId, isReady);
    }

    public bool AreBothPlayersReady(int gameId)
    {
        var game = _gameRepository.GetGameById(gameId);
        return game.IsPlayer1Ready && game.IsPlayer2Ready;
    }

    public int GetOpponentId(int gameId, int playerId)
    {
        return _gameRepository.GetOpponentId(gameId, playerId);
    }
    
    public bool FireAtOpponent(int _gameId, int opponentId, int x, int y) 
    {
        return _cellService.FireAtOpponent(_gameId, opponentId, x, y);
    }

    public bool IsPlayerTurn(int gameId, int playerId)
    {
        return _gameRepository.IsPlayerTurn(gameId, playerId);
    }

    public void SwitchTurn(int gameId)
    {
        _gameRepository.SwitchTurn(gameId);
    }
}