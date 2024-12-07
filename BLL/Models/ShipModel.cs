using SeaBattle.Common;

namespace SeaBattle.Models;

public class ShipModel  {
    public int NDecks { get; set; }
    public int Health { get; set; }
    public bool IsHorizontal { get; set; }
    public Vector2D Coords { get; set; } // of the first deck

    public ShipModel(int nDecks, bool isHorizontal, Vector2D coords) {
        NDecks = nDecks;
        IsHorizontal = isHorizontal;
        Coords = coords;
        Health = nDecks;
    }

    public void Damage() {
        Health--;
    }

    public bool HasSunck() {
        return Health == 0;
    }
}