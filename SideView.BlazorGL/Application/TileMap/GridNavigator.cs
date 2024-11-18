namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public class GridNavigator(Grid grid)
{
    private int _x;
    private int _y;

    public GridNavigator StartAt(Cell cell)
    {
        _x = cell.X;
        _y = cell.Y;
        return this;
    }
    
    public GridNavigator Up => NeighborAt(0, -1);
    public GridNavigator Down => NeighborAt(0, 1);
    public GridNavigator Left => NeighborAt(-1, 0);
    public GridNavigator Right => NeighborAt(1, 0);
    public GridNavigator DownRight => NeighborAt(1, 1);
    public GridNavigator DownLeft => NeighborAt(-1, 1);
    public GridNavigator UpRight => NeighborAt(1, -1);
    public GridNavigator UpLeft => NeighborAt(-1, -1);

    public GridNavigator NeighborAt(int deltaX, int deltaY)
    {
        _x += deltaX;
        _y += deltaY;
        return this;
    }

    public Cell? Cell => grid[_x, _y];
}
