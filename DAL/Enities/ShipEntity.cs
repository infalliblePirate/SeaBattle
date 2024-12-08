using SeaBattle.Common;

namespace SeaBattle.Entities;

public class ShipEntity 
{
    public int NDecks { get; set; }
    public int Health { get; set; }
    public bool IsHorizontal { get; set; }
    public Vector2D Coords { get; set; }

    public ShipEntity(int nDecks, bool isHorizontal, Vector2D coords) {
        NDecks = nDecks;
        IsHorizontal = isHorizontal;
        Coords = coords;
        Health = nDecks;
    }
}