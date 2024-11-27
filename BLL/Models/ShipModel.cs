namespace SeaBattle.Models;

public struct Vector2D {
    public int X { get; set; }
    public int Y { get; set; }

    public Vector2D(int x, int y) {
        X = x;
        Y = y;
    }
}

public class ShipModel  {
    public int NDecks { get; set; }
    public bool IsHorizontal { get; set; }
    public Vector2D Coords { get; set; } // of the first deck

    public ShipModel(int nDecks, bool IsHorizontal, Vector2D coords) {
        NDecks = nDecks;
        IsHorizontal = IsHorizontal;
        Coords = coords;
    }

    public void Damage() {
        NDecks--;
    }

    public bool HasSunck() {
        return NDecks == 0;
    }
}