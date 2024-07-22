namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public class AStarPathfinder
{
    public List<Cell> FindPath(Point startPos, Point targetPos, Grid grid)
    {
        // If startPos or targetPos is out of bounds, return an empty path
        if (!grid.TryGetCellAtPosition(startPos, out var start)) return [];
        if (!grid.TryGetCellAtPosition(targetPos, out var target)) return [];

        start.CostFromStart = 0;

        List<Cell> openSet = [start];
        while (openSet.Count > 0) {
            var current = openSet.OrderBy(c => c.CostToTarget).First();
            if (current == target) return ReconstructPath(current);

            openSet.Remove(current);

            var tentativeCost = current.CostFromStart + 1;
            foreach (var n in Neighbors(current, grid.Cells)
                         .Where(n => tentativeCost < n.CostFromStart)) {
                n.Parent = current;
                n.CostFromStart = tentativeCost;
                n.CostToTarget = tentativeCost + GetDistance(n, target);
                if (!openSet.Contains(n)) openSet.Add(n);
            }
        }

        return [];
    }

    private static List<Cell> ReconstructPath(Cell current)
    {
        List<Cell> path = [current];

        var walkingCell = current;
        while (walkingCell.Parent != null) {
            path.Insert(0, walkingCell.Parent);
            walkingCell = walkingCell.Parent;
        }

        return path;
    }

    private static float GetDistance(Cell a, Cell b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }

    private static List<Cell> Neighbors(Cell cell, Cell[,] grid)
    {
        List<Cell> neighbors = [];

        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];

        for (var i = 0; i < 4; i++) {
            var checkX = cell.X + dx[i];
            var checkY = cell.Y + dy[i];

            if (checkX >= 0
                && checkX < grid.GetLength(0)
                && checkY >= 0
                && checkY < grid.GetLength(1)
                && grid[checkX, checkY].IsWalkable
               ) {
                neighbors.Add(grid[checkX, checkY]);
            }
        }

        return neighbors;
    }
}