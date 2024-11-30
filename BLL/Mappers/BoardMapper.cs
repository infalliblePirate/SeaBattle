using Newtonsoft.Json;

using SeaBattle.Models;
using SeaBattle.Common;
using SeaBattle.Dtos;

namespace SeaBattle.Mappers;

public class BoardMapper {

    public static BoardModel ToBoardModel(BoardDto dto) {
        return new BoardModel {
            Grid = dto.Grid,
            Ships = dto.Ships.Select(s => ShipMapper.ToShipModel(s)).ToList(),
        };
    }

    public static string Serialize(BoardModel model) {
        var dto = new BoardDto(model);
        return JsonConvert.SerializeObject(dto);
    }

    public static BoardModel Deserialize(string json) {
        var dto = JsonConvert.DeserializeObject<BoardDto>(json);
        return BoardMapper.ToBoardModel(dto);
    }
}