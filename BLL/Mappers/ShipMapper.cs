using SeaBattle.Models;
using SeaBattle.Entities;
using SeaBattle.Dtos;

namespace SeaBattle.Mappers;

public class ShipMapper {
    // public static ShipDto ToShipDto(ShipModel model) {
    //     if (model == null) throw new ArgumentNullException(nameof(model));

    //     return new ShipDto {
    //        NDecks = model.NDecks,
    //        IsHorizontal = model.IsHorizontal,
    //        Coords = model.Coords,
    //     };
    // }

    // public ShipEntity ToShipEntity(ShipModel model) {
    //     if (model == null) throw new ArgumentNullException(nameof(model));
    //     var dto = new ShipDto(model);

    //     return new ShipEntity {
    //        NDecks = model.NDecks,
    //        IsHorizontal = model.IsHorizontal,
    //        Coords = model.Coords;
    //     };
    // }

    public static ShipModel ToShipModel(ShipDto dto) {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        
        return new ShipModel ( dto.NDecks, dto.IsHorizontal, dto.Coords );
    }
}