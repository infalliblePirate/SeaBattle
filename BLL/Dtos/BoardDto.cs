using SeaBattle.Models;
using SeaBattle.Common;

namespace SeaBattle.Dtos;

public class BoardDto {
    public CellState[,] Grid { get; set; }
    public List<ShipDto> Ships { get; set; }

    public BoardDto(BoardModel model) {
        if (model == null) throw new ArgumentNullException(nameof(model));

        // for json serialization
        int rows = model.Grid.GetLength(0);
        int cols = model.Grid.GetLength(1);

        Grid = new CellState[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Grid[i, j] = model.Grid[i, j];  
            }
        }
        Ships = Ships = model.Ships.Select(ship => new ShipDto(ship)).ToList();
    }
}