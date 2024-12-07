using SeaBattle.Models;
using SeaBattle.Common;
using SeaBattle.Entities;

namespace SeaBattle.Mappers;

public class CellMapper 
{
    public static CellEntity ToCellEntity(CellModel model) 
    {
        return new CellEntity 
        {
            Id = model.Id,
            GameId = model.GameId,
            UserId = model.UserId,
            Row = model.Row,
            Col = model.Col,
            State = model.State.ToString()
        };
    }

    public static CellModel ToCellModel(CellEntity entity) 
    {
        return new CellModel 
        {
            Id = entity.Id,
            GameId = entity.GameId,
            UserId = entity.UserId,
            Row = entity.Row,
            Col = entity.Col,
            State = Enum.Parse<CellState>(entity.State)
        };
    }
}