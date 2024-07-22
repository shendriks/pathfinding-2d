namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public class Cell(int x, int y, bool isWalkable)
{
    public Point Position { get; } = new(x, y);
    public int X => Position.X;
    public int Y => Position.Y;
    public bool IsWalkable { get; } = isWalkable;
    public float CostToTarget { get; set; } = float.PositiveInfinity;
    public float CostFromStart { get; set; } = float.PositiveInfinity;
    public Cell? Parent { get; set; }

    public override string ToString()
    {
        return $"{nameof(Position)}: {Position}, {nameof(IsWalkable)}: {IsWalkable}, " +
               $"{nameof(CostToTarget)}: {CostToTarget}, {nameof(CostFromStart)}: {CostFromStart}";
    }
}