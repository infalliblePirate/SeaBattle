using SeaBattle.Common;

namespace SeaBattle.Models;

public class BoardModel {
    private readonly BoardProps _props;
    public CellState[,] Grid { get; set; }
    public List<ShipModel> Ships { get; set; }

    public BoardModel(BoardProps props) {
        _props = props;
        Grid = new CellState[_props.Size, _props.Size];
        Ships = new List<ShipModel>();
        for (int x = 0; x < _props.Size; x++) {
            for (int y = 0; y < _props.Size; y++) {
                Grid[x, y] = CellState.Empty;
            }
        }
    }

    public BoardModel() : this(new BoardProps()) { }

    public bool PlaceShip(ShipModel ship) {
        if (!CanPlaceShip(ship)) return false;

        for (int i = 0; i < ship.NDecks; i++) {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            Grid[x, y] = CellState.Ship;
        }

        BlockSurroundingCells(ship);
        Ships.Add(ship);
        return true;
    }

    private bool CanPlaceShip(ShipModel ship) {
        for (int i = 0; i < ship.NDecks; i++) {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            if (!IsWithinBounds(x, y) || Grid[x, y] != CellState.Empty) {
                return false;
            }
        }
        return true;
    }

    private void BlockSurroundingCells(ShipModel ship) {
        for (int i = 0; i < ship.NDecks; i++) {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            for (int dx = -1; dx <= 1; dx++) {
                for (int dy = -1; dy <= 1; dy++) {
                    int nx = x + dx;
                    int ny = y + dy;
                    ChangeCellStateIfNoShip(nx, ny, CellState.Blocked);
                }
            }
        }
    }

    private void ChangeCellStateIfNoShip(int x, int y, CellState state) {
        if (IsWithinBounds(x, y) && Grid[x, y] != CellState.Ship) {
            Grid[x, y] = state;
        }
    }

    private bool IsWithinBounds(int x, int y) {
        bool res = x >= 0 && x < _props.Size && y >= 0 && y < _props.Size; 
        return res;
    }

    public void PrintBoard() {
        for (int y = 0; y < _props.Size; y++) {
            for (int x = 0; x < _props.Size; x++) {
                char cell = Grid[x, y] switch {
                    CellState.Empty => '.',
                    CellState.Ship => 'S',
                    CellState.Blocked => 'B',
                    CellState.Hit => 'H',
                    CellState.Miss => 'M',
                    _ => '?'
                };
                Console.Write(cell + " ");
            }
            Console.WriteLine();
        }
    }
}

