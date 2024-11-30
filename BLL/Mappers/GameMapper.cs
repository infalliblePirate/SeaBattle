using Newtonsoft.Json;

using SeaBattle.Entities;
using SeaBattle.Models;
using SeaBattle.Dtos;

namespace SeaBattle.Mappers;

public class GameMapper {
    public static GameModel ToGameModel(GameEntity entity) {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new GameModel {
            Id = entity.Id,
            User1Id = entity.User1Id,
            User2Id = entity.User2Id,
            WinnerId = entity.WinnerId,
            Score = entity.Score,
            Player1Board = BoardMapper.Deserialize(entity.Player1BoardSerialized),
            Player2Board = BoardMapper.Deserialize(entity.Player2BoardSerialized),
            IsPlayer1Turn = entity.IsPlayer1Turn
        };
    }

    public static GameEntity ToGameEntity(GameModel model) {
        if (model == null) throw new ArgumentNullException(nameof(model));

        return new GameEntity {
            Id = model.Id,
            User1Id = model.User1Id,
            User2Id = model.User2Id,
            WinnerId = model.WinnerId,
            Score = model.Score,
            Player1BoardSerialized = BoardMapper.Serialize(model.Player1Board),
            Player2BoardSerialized = BoardMapper.Serialize(model.Player2Board),
            IsPlayer1Turn = model.IsPlayer1Turn
        };
    }


    // public GameEntity ToGameEntity(GameDto dto) {
    //     if (dto == null) throw new ArgumentNullException(nameof(dto));

    //     return new GameEntity {
    //         Id = dto.Id,
    //         User1Id = dto.User1Id,
    //         User2Id = dto.User2Id,
    //         WinnerId = dto.WinnerId,
    //         Score = dto.Score,
    //         Player1BoardSerialized = JsonConvert.SerializeObject(dto.Player1Board),
    //         Player2BoardSerialized = JsonConvert.SerializeObject(dto.Player2Board),
    //         IsPlayer1Turn = dto.IsPlayer1Turn
    //     };
    // }

 
}