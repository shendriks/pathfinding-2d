using System.Collections.Generic;
using NUnit.Framework;

namespace BlogCodeExamples.Pathfinding.TopDownView.Console.Tests;

[TestFixture]
[TestOf(typeof(AStarPathfinder))]
public class AStarPathfinderTest
{
    [Test]
    public void TestValidStartAndTargetPositions()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        });
        var start = new Point(0, 0);
        var target = new Point(4, 4);

        var pathfinder = new AStarPathfinder();
        var path = pathfinder.FindPath(start, target, grid);

        Assert.That(path.Count, Is.GreaterThan(0));
        Assert.That(path[0].Position, Is.EqualTo(start));
        Assert.That(path[^1].Position, Is.EqualTo(target));
    }

    [Test]
    public void TestInvalidStartPosition()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        });
        var start = new Point(-1, 0);
        var target = new Point(4, 4);

        var pathfinder = new AStarPathfinder();
        var path = pathfinder.FindPath(start, target, grid);

        Assert.That(path, Is.Empty);
    }

    [Test]
    public void TestInvalidTargetPosition()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 },
            { 0, 0, 0, 0, 0 }
        });
        var start = new Point(0, 0);
        var target = new Point(5, 4);

        var pathfinder = new AStarPathfinder();
        var path = pathfinder.FindPath(start, target, grid);

        Assert.That(path, Is.Empty);
    }

    [Test]
    public void TestNoPathFound()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { 0, 0, 1, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 1, 0, 0 },
            { 0, 0, 1, 0, 0 }
        });
        var start = new Point(0, 0);
        var target = new Point(4, 4);

        var pathfinder = new AStarPathfinder();
        var path = pathfinder.FindPath(start, target, grid);

        Assert.That(path, Is.Empty);
    }

    [Test]
    public void TestDefaultExample()
    {
        var grid = Grid.CreateFromArray(new[,] {
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
        });
        var start = new Point(1, 1);
        var target = new Point(4, 2);

        var pathfinder = new AStarPathfinder();
        var path = pathfinder.FindPath(start, target, grid);

        Assert.That(path.Count, Is.GreaterThan(0));
        Assert.That(path[0].Position, Is.EqualTo(start));
        Assert.That(path[^1].Position, Is.EqualTo(target));
        Assert.That(
            path.ConvertAll(p => p.Position),
            Is.EqualTo(new List<Point> {
                new(1, 1), new(1, 2), new(1, 3), new(1, 4), new(2, 4), new(3, 4),
                new(4, 4), new(5, 4), new(6, 4), new(7, 4), new(8, 4), new(8, 3),
                new(8, 2), new(7, 2), new(6, 2), new(5, 2), new(4, 2)
            }));
    }
}