namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public static class Program
{
    public static void Main(string[] _)
    {
        int[,] map = {
            { 0, 0, 1, 0, 0, 0, 1, 0, 1, 0 },
            { 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
            { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
            { 0, 1, 0, 0, 0, 0, 1, 0, 1, 1 },
            { 1, 1, 1, 1, 0, 1, 1, 0, 1, 0 },
            { 0, 0, 0, 0, 0, 1, 1, 0, 1, 0 },
            { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
            { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

        var grid = Grid.CreateFromArray(map);

        var pathFinder = new AStarPathfinder();
        var path = pathFinder.FindPath(new Point(1, 1), new Point(4, 2), grid);
        if (path.Count == 0) {
            System.Console.WriteLine("No path found!");
            return;
        }

        System.Console.OutputEncoding = System.Text.Encoding.UTF8;
        System.Console.WriteLine("Path found:");
        System.Console.WriteLine(StringFromPath(path));
    }

    private static string StringFromPath(IEnumerable<Cell> path)
    {
        var steps = path.Select(cell => $"[{cell.X},{cell.Y}]").ToList();
        return string.Join("\u2192", steps);
    }
}