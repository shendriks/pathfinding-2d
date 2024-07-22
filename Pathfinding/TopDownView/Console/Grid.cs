namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public class Grid
{
    public required Cell[,] Cells { get; init; }

    private Grid()
    {
    }

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

    public bool TryGetCellAtPosition(Position pos, out Cell cell)
    {
        if (pos.X < 0 || pos.Y < 0 || pos.X >= Cells.GetLength(0) || pos.Y >= Cells.GetLength(1)) {
            cell = new Cell(0, 0, false);
            return false;
        }

        cell = Cells[pos.X, pos.Y];
        return true;
    }
}