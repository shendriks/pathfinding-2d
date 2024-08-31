namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public class Grid
{
    public required Cell[,] Cells { get; init; }

    private Grid() { }

    public static Grid CreateFromArray(int[,] map)
    {
        var cells = new Cell[map.GetLength(1), map.GetLength(0)];
        for (var y = 0; y < map.GetLength(0); y++) {
            for (var x = 0; x < map.GetLength(1); x++) {
                cells[x, y] = new Cell(x, y, isWalkable: map[y, x] == 0);
            }
        }

        return new Grid { Cells = cells };
    }

    private bool IsInsideBounds(Point position)
    {
        var (x, y) = position;
        return x >= 0 && x < Cells.GetLength(0) && y >= 0 && y < Cells.GetLength(1);
    }

    /// <summary>
    /// Try get the cell at the given position
    /// </summary>
    /// <param name="pos">The position to get the cell at from the grid</param>
    /// <param name="cell">The cell at the position if it exists</param>
    /// <returns>true on success, false on failure</returns>
    public bool TryGetCellAtPosition(Point pos, out Cell cell)
    {
        if (!IsInsideBounds(pos)) {
            cell = new Cell(0, 0, false);
            return false;
        }

        cell = Cells[pos.X, pos.Y];
        return true;
    }
}