using SeaBattle.Models;
using SeaBattle.Data;
using SeaBattle.Entities;
using SeaBattle.Mappers;
using SeaBattle.Common;

namespace SeaBattle.Services;

public class CellService {
    private readonly ICellRepository _cellRepository;
    private readonly BoardProps _props;

    public CellService(ICellRepository cellRepository, BoardProps props)
    {
        _cellRepository = cellRepository;
        _props = props;
    }

    public CellService(ICellRepository cellRepository) : this(cellRepository, new BoardProps()) {}

    public List<CellModel> GetCellsForPlayer(int gameId, int playerId)
    {
        var cellEntities = _cellRepository.GetCellsForPlayer(gameId, playerId); 
        return cellEntities.Select(cell => CellMapper.ToCellModel(cell)).ToList();
    }

    public void CreateCell(int gameId, int userId, int row, int col, CellState state)
    {
        var cellEntity = new CellEntity 
        {
            GameId = gameId,
            UserId = userId,
            Row = row,
            Col = col,
            State = state.ToString()
        };

        // var cellEntity = CellMapper.ToCellEntity(cell);
        try
        {
            int cellId = _cellRepository.AddCell(cellEntity);
            cellEntity.Id = cellId;
        } catch (Exception ex){
            Console.Error.WriteLine($"Error creating cell: {ex.Message}");
        }
    }

    public void PlaceShip(int gameId, int playerId, ShipModel ship)
    {
        for (int i = 0; i < ship.NDecks; i++)
        {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            CreateCell(gameId, playerId, x, y, CellState.Ship);
        }

        BlockSurroundingCells(gameId, playerId, ship);
    }

    public bool CanPlaceShip(int gameId, int playerId, ShipModel ship)
    {
        for (int i = 0; i < ship.NDecks; i++)
        {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            if (!IsWithinBounds(x, y) || IsCellOccupied(x, y, gameId, playerId, CellState.Ship) || IsCellOccupied(x, y, gameId, playerId, CellState.Blocked))
            {
                return false;
            }
        }
        return true;
    }

    private bool IsWithinBounds(int x, int y)
    {
        return x >= 0 && x < _props.Size && y >= 0 && y < _props.Size;
    }

    private void BlockSurroundingCells(int gameId, int playerId, ShipModel ship) 
    {
        for (int i = 0; i < ship.NDecks; i++)
        {
            int x = ship.IsHorizontal ? ship.Coords.X + i : ship.Coords.X;
            int y = ship.IsHorizontal ? ship.Coords.Y : ship.Coords.Y + i;

            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++) 
                {
                    int nx = x + dx;
                    int ny = y + dy;
                    ChangeCellStateIfNoShip(nx, ny, gameId, playerId);
                }
            }
        }
    }

    private void ChangeCellStateIfNoShip(int x, int y, int gameId, int userId) 
    {
        if (IsWithinBounds(x, y) && !IsCellOccupied(x, y, gameId, userId, CellState.Ship))
        {
            CreateCell(gameId, userId, x, y, CellState.Blocked);
        }
    }

    public bool IsCellOccupied(int row, int col, int gameId, int userId, CellState state)
    {
        return _cellRepository.IsCellAlreadyStored(row, col, gameId, userId, state.ToString());
    }
}