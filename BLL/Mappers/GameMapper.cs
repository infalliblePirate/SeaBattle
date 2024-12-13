using SeaBattle.Entities;
using SeaBattle.Models;

namespace SeaBattle.Mappers;

public class GameMapper {
    public static GameModel ToGameModel(GameEntity entity)
    {
        if (entity == null) throw new ArgumentNullException(nameof(entity));

        return new GameModel {
            Id = entity.Id,
            User1Id = entity.User1Id,
            User2Id = entity.User2Id,
            WinnerId = entity.WinnerId,
            Score = entity.Score,
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
            IsPlayer1Turn = model.IsPlayer1Turn
        };
    }
    public static List<GameModel> ToGameModelList(IEnumerable<GameEntity> entities)
    {
        if (entities == null) throw new ArgumentNullException(nameof(entities));

        return entities.Select(ToGameModel).ToList();
    }

 
}