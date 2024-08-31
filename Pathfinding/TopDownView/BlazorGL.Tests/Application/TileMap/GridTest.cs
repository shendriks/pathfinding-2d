using System;
using System.Diagnostics;
using System.Linq;
using BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMap;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace BlazorGL.Tests.Application.TileMap;

[TestFixture]
[TestOf(typeof(Cell))]
public class GridTest
{
    [Test]
    public void TestCreateFromArray()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { 0, 1, 0 },
            { 0, 0, 1 },
            { 0, 1, 1 }
        });

        Assert.That(grid[0, 0], Is.EqualTo(new Cell(0, 0, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[1, 0], Is.EqualTo(new Cell(1, 0, false)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[2, 0], Is.EqualTo(new Cell(2, 0, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[0, 1], Is.EqualTo(new Cell(0, 1, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[1, 1], Is.EqualTo(new Cell(1, 1, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[2, 1], Is.EqualTo(new Cell(2, 1, false)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[0, 2], Is.EqualTo(new Cell(0, 2, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[1, 2], Is.EqualTo(new Cell(1, 2, false)).Using<Cell>(AreCellsEqual));
        Assert.That(grid[2, 2], Is.EqualTo(new Cell(2, 2, false)).Using<Cell>(AreCellsEqual));
    }

    [Test]
    public void TestGridChanged()
    {
        var grid = CreateTestGrid();

        var gridChangeCount = 0;
        grid.GridChanged += () => gridChangeCount++;

        Assert.That(gridChangeCount, Is.EqualTo(0));
        grid[0, 0].IsWalkable = true;
        Assert.That(gridChangeCount, Is.EqualTo(0));
        grid[0, 0].IsWalkable = false;
        Assert.That(gridChangeCount, Is.EqualTo(1));
        grid[1, 1].IsWalkable = false;
        Assert.That(gridChangeCount, Is.EqualTo(2));
        grid[2, 2].IsWalkable = false;
        Assert.That(gridChangeCount, Is.EqualTo(3));
        grid[2, 2].IsWalkable = false;
        Assert.That(gridChangeCount, Is.EqualTo(3));
        grid[2, 2].IsWalkable = true;
        Assert.That(gridChangeCount, Is.EqualTo(4));
    }

    [Test]
    public void TestTryGetCellAtPositionSucceeds()
    {
        var grid = CreateTestGrid();

        var actualSuccess = grid.TryGetCellAtPosition(new Point(0, 0), out var actualCell);

        Assert.That(actualSuccess, Is.True);
        Assert.That(actualCell, Is.SameAs(grid[0, 0]));
    }

    [Test]
    public void TestTryGetCellAtPositionFails()
    {
        var grid = CreateTestGrid();

        var actualSuccess = grid.TryGetCellAtPosition(new Point(3, 4), out var actualCell);

        Assert.That(actualSuccess, Is.False);
        Assert.That(actualCell, Is.SameAs(Cell.None));
    }

    [Test]
    public void TestGridChanges()
    {
        var grid = CreateTestGrid();

        var actionCallCount = 0;
        grid.GridChanged += () => actionCallCount++;

        Assert.That(actionCallCount, Is.EqualTo(0));
        grid[0, 0].IsWalkable = true;
        Assert.That(actionCallCount, Is.EqualTo(0));
        grid[0, 0].IsWalkable = false;
        Assert.That(actionCallCount, Is.EqualTo(1));
        grid[1, 1].IsWalkable = false;
        Assert.That(actionCallCount, Is.EqualTo(2));
        grid[2, 2].IsWalkable = false;
        Assert.That(actionCallCount, Is.EqualTo(3));
        grid[2, 2].IsWalkable = false;
        Assert.That(actionCallCount, Is.EqualTo(3));
    }

    [Test]
    public void TestIndexerReturnsCorrectValue()
    {
        var grid = CreateTestGrid();

        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                Assert.That(grid[x, y], Is.EqualTo(new Cell(x, y, true)).Using<Cell>(AreCellsEqual));
            }
        }
    }

    [Test, Sequential]
    public void TestIndexerThrowsExceptionForInvalidIndexes(
        [Values(0, -1, 0, 3)] int x,
        [Values(-1, 0, 3, 0)] int y
    )
    {
        var grid = CreateTestGrid();
        Assert.Throws<IndexOutOfRangeException>(() => {
            _ = grid[x, y];
        });
    }

    private static bool AreCellsEqual(Cell a, Cell b)
    {
        return a.Position == b.Position && a.IsWalkable == b.IsWalkable;
    }

    private static Grid CreateTestGrid()
    {
        return Grid.CreateFromArray(new[,] {
            { 0, 0, 0 },
            { 0, 0, 0 },
            { 0, 0, 0 }
        });
    }

    [Test]
    public void TestEnumerator()
    {
        var grid = CreateTestGrid();

        var cells = grid.ToArray();
        Assert.That(cells.Length, Is.EqualTo(9));

        var i = 0;
        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++, i++) {
                Assert.That(cells[i], Is.EqualTo(new Cell(x, y, true)).Using<Cell>(AreCellsEqual));
            }
        }
    }

    [Test]
    public void TestReset()
    {
        var grid = CreateTestGrid();
        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                grid[x, y].CostFromStart = 23;
                grid[x, y].CostToTarget = 42;
                grid[x, y].Parent = new Cell(0, 0, false);
                grid[x, y].WasInspected = true;
                grid[x, y].IsOnPath = true;
                grid[x, y].IsInOpenSet = true;
            }
        }

        grid.Reset();

        for (var x = 0; x < 3; x++) {
            for (var y = 0; y < 3; y++) {
                var cell = grid[x, y];
                Assert.That(cell.CostFromStart, Is.EqualTo(float.PositiveInfinity));
                Assert.That(cell.CostToTarget, Is.EqualTo(float.PositiveInfinity));
                Assert.That(cell.Parent, Is.Null);
                Assert.That(cell.WasInspected, Is.False);
                Assert.That(cell.IsOnPath, Is.False);
                Assert.That(cell.IsInOpenSet, Is.False);
            }
        }
    }


    [Test]
    public void TestStartAndTarget()
    {
        var grid = CreateTestGrid();
        grid.StartPosition = new Point(1, 1);
        grid.TargetPosition = new Point(2, 2);

        Assert.That(grid.Start, Is.EqualTo(new Cell(1, 1, true)).Using<Cell>(AreCellsEqual));
        Assert.That(grid.Target, Is.EqualTo(new Cell(2, 2, true)).Using<Cell>(AreCellsEqual));
    }
}