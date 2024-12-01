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
    
    public GridNavigator Up => MoveBy(0, -1);
    public GridNavigator Down => MoveBy(0, 1);
    public GridNavigator Left => MoveBy(-1, 0);
    public GridNavigator Right => MoveBy(1, 0);
    public GridNavigator DownRight => MoveBy(1, 1);
    public GridNavigator DownLeft => MoveBy(-1, 1);
    public GridNavigator UpRight => MoveBy(1, -1);
    public GridNavigator UpLeft => MoveBy(-1, -1);

    public GridNavigator MoveBy(int deltaX, int deltaY)
    {
        _x += deltaX;
        _y += deltaY;
        return this;
    }

    public Cell? Cell => grid[_x, _y];
}
