using NUnit.Framework;

namespace BlogCodeExamples.Pathfinding.TopDownView.Console.Tests;

[TestFixture]
[TestOf(typeof(Grid))]
public class GridTest
{
    [Test]
    public void TestCreateFromArray()
    {
        var grid = Grid.CreateFromArray(new[,] {
            {0, 1, 0},
            {0, 0, 1},
            {0, 1, 1}
        });

        Assert.That(grid.Cells, Is.EqualTo(new Cell[,] {
            { new(0, 0, true), new(0, 1, true), new(0, 2, true) },
            { new(1, 0, false), new(1, 1, true), new(1, 2, false) },
            { new(2, 0, true), new(2, 1, false), new(2, 2, false) }
        }).Using<Cell>((a, b) => Equals(a.Position, b.Position) && a.IsWalkable == b.IsWalkable));
    }
}