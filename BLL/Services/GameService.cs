using Newtonsoft.Json;

using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
using SeaBattle.Mappers;
using SeaBattle.Common;

namespace SeaBattle.Services;

public class GameService {
    private readonly IGameRepository _gameRepository;

    public GameService(IGameRepository gameRepository) {
        _gameRepository = gameRepository;
    }

    public GameModel CreateGame(int joinedPlayerId) {
        // TODO: mb create a model and then
        // if (SessionService.ActiveUser?.Id == null) {
        //     throw new InvalidOperationException("No active user session found.");
        // }

        // int joinedPlayerId = SessionService.ActiveUser.Id;
        var gameEntity = new GameEntity { 
                User1Id = joinedPlayerId,
                Player1BoardSerialized = JsonConvert.SerializeObject(new BoardModel()),
                Player2BoardSerialized = JsonConvert.SerializeObject(new BoardModel()),
                IsPlayer1Turn = true
            };

        try {
            int gameId = _gameRepository.AddGame(gameEntity);
            gameEntity.Id = gameId;
            return GameMapper.ToGameModel(gameEntity);
        } catch (Exception ex) {
            Console.Error.WriteLine($"Error creating game: {ex.Message}");
            return null;
        }
    }

    public GameModel GetGameById(int id) {
        var gameEntity = _gameRepository.GetGameById(id); 
        return GameMapper.ToGameModel(gameEntity);
    }

    // public async Task<bool> PlaceShipAsync(ShipModel ship) {
    //     // todo: ensure only 10 ships can be placed
    //     var game = SessionService.ActiveGame;
    //     var activePlayerBoard = game.User1Id == SessionService.ActiveUser.Id
    //         ? game.Player1Board
    //         : game.Player2Board;
    //     bool isPlaced = activePlayerBoard.PlaceShip(ship);
    //     if (isPlaced) {
    //         // var dto = new GameDto(game); 
    //         _gameRepository.UpdateGameAsync(GameMapper.ToGameEntity(game));
    //     }
    //     return isPlaced;
    // }

    // public async void PrintBoardAsync() {
    //     var game = SessionService.ActiveGame;
    //     var activePlayerBoard = game.User1Id == SessionService.ActiveUser.Id
    //         ? game.Player1Board
    //         : game.Player2Board;
    //     activePlayerBoard.PrintBoard();
    // }

    // // TODO: remove, temporary
    // public async Task<CellState[,]> GetBoardStateAsync(int gameId) {
    //     var game = SessionService.ActiveGame;
    //     var activePlayerBoard = game.User1Id == SessionService.ActiveUser.Id
    //         ? game.Player1Board
    //         : game.Player2Board;
    //     return activePlayerBoard.Grid;
    // }

}