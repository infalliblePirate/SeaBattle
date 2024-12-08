using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
using SeaBattle.Mappers;
using SeaBattle.Common;

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
        var gameEntity = new GameEntity 
        { 
            User1Id = joinedPlayerId,
            IsPlayer1Turn = true
        };

        try 
        {
            int gameId = _gameRepository.AddGame(gameEntity);
            gameEntity.Id = gameId;
            // return GameMapper.ToGameModel(gameEntity);
            return gameId;
        } catch (Exception ex) {
            Console.Error.WriteLine($"Error creating game: {ex.Message}");
            return null;
        }
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

    public void SetPlayerReady(int gameId, int playerId, bool isReady)
    {
        _gameRepository.UpdatePlayerReadyStatus(gameId, playerId, isReady);
    }

    public bool AreBothPlayersReady(int gameId)
    {
        var game = _gameRepository.GetGameById(gameId);
        return game.IsPlayer1Ready && game.IsPlayer2Ready;
    }
}