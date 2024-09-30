namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public record struct CellCostPair(Cell Cell, float Cost)
{
    public Cell Cell { get; private set; } = Cell;
    public float Cost { get; private set; } = Cost;
}