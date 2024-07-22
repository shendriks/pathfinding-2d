namespace BlogCodeExamples.Pathfinding.TopDownView.Console;

public struct Point(int x, int y)
{
    public int X = x;
    public int Y = y;

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
    }
}