using SeaBattle.Common;
using SeaBattle.Models;

namespace SeaBattle.Dtos;

public class ShipDto {
    public int NDecks { get; set; }
    public bool IsHorizontal { get; set; }
    public Vector2D Coords { get; set; } 

    public ShipDto(ShipModel model) {
        NDecks = model.NDecks;
        IsHorizontal = model.IsHorizontal;
        Coords = model.Coords;
    }
}