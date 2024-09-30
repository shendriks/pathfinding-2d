using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;

namespace Pathfinding2D.SideView.BlazorGL.Tests.Application.TileMap.Neighbor;

[TestFixture]
[TestOf(typeof(NeighborFinder))]
public class NeighborFinderTest
{
    [Test]
    public void TestFindNeighborsReturnsEmptyCollectionIfOnlyOneCell()
    {
        var finder = new NeighborFinder();
        var cell = new Cell(0, 0, CellType.Empty);

        var neighbors = finder.FindNeighbors(cell, false, GetDistance);
        var cellCostPairs = neighbors.ToArray();

        Assert.That(cellCostPairs, Is.Empty);
    }

    [TestCaseSource(nameof(GridNeighborNoJumpingProvider))]
    public void TestFindNeighborsWithoutJumping(Grid grid, Cell cell, CellCostPair[] expectedCellCostPairs)
    {
        var finder = new NeighborFinder();

        var neighbors = finder.FindNeighbors(cell, false, GetDistance);
        var actualCellCostPairs = neighbors.ToArray();

        Assert.That(actualCellCostPairs, Is.EquivalentTo(expectedCellCostPairs));
    }

    public static IEnumerable<object> GridNeighborNoJumpingProvider()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' }
        });
        yield return new object[] { grid, grid[1, 1]!, new[] { new CellCostPair(grid[1, 2]!, 1) } };

        grid = Grid.CreateFromArray(new[,] {
            { ' ', ' ', ' ' },
            { ' ', ' ', ' ' },
            { ' ', 'B', ' ' }
        });
        yield return new object[] {
            grid,
            grid[1, 1]!,
            new[] {
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[2, 1]!, 1)
            }
        };

        grid = Grid.CreateFromArray(new[,] {
            { ' ', 'L', ' ' },
            { ' ', 'L', ' ' },
            { ' ', 'L', ' ' }
        });
        yield return new object[] {
            grid,
            grid[1, 1]!,
            new[] {
                new CellCostPair(grid[1, 0]!, 1),
                new CellCostPair(grid[1, 2]!, 1),
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[2, 1]!, 1)
            }
        };

        grid = Grid.CreateFromArray(new[,] {
            { 'B', 'B', 'B' },
            { 'H', 'H', 'H' },
            { ' ', ' ', ' ' }
        });
        yield return new object[] {
            grid,
            grid[1, 1]!,
            new[] {
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[2, 1]!, 1),
                new CellCostPair(grid[1, 2]!, 1)
            }
        };
    }

    [TestCaseSource(nameof(GridNeighborProvider))]
    public void TestFindNeighborsWithJumping(Grid grid, Cell cell, CellCostPair[] expectedCellCostPairs)
    {
        var finder = new NeighborFinder();

        var neighbors = finder.FindNeighbors(cell, true, GetDistance);
        var actualCellCostPairs = neighbors.ToArray();

        Assert.That(actualCellCostPairs, Is.EqualTo(expectedCellCostPairs));
    }

    public static IEnumerable<object> GridNeighborProvider()
    {
        var grid = Grid.CreateFromArray(new[,] {
            { ' ', ' ', ' ' , ' ', ' ' },
            { ' ', ' ', 'B' , ' ', ' ' },
            { ' ', ' ', 'B' , ' ', ' ' },
            { 'B', 'B', 'B' , 'B', 'B' }
        });
        yield return new object[] {
            grid,
            grid[0, 2]!,
            new[] {
                new CellCostPair(grid[1, 2]!, 1),
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[0, 0]!, 2),
                new CellCostPair(grid[1, 0]!, 3),
                new CellCostPair(grid[2, 0]!, 4),
                new CellCostPair(grid[3, 0]!, 5),
                new CellCostPair(grid[4, 0]!, 6),
                new CellCostPair(grid[4, 1]!, 7),
                new CellCostPair(grid[4, 2]!, 8)
            }
        };

        grid = Grid.CreateFromArray(new[,] {
            { ' ', ' ', ' ' , ' ', ' ' },
            { ' ', ' ', 'B' , 'B', 'B' },
            { ' ', ' ', ' ' , ' ', ' ' },
            { 'B', 'B', 'B' , 'B', 'B' }
        });
        yield return new object[] {
            grid,
            grid[0, 2]!,
            new[] {
                new CellCostPair(grid[1, 2]!, 1),
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[0, 0]!, 2),
                new CellCostPair(grid[1, 0]!, 3),
                new CellCostPair(grid[2, 0]!, 4),
                new CellCostPair(grid[3, 0]!, 5),
                new CellCostPair(grid[4, 0]!, 6)
            }
        };

        grid = Grid.CreateFromArray(new[,] {
            { ' ', ' ', ' ' , ' ', ' ' },
            { ' ', ' ', ' ' , ' ', ' ' },
            { ' ', ' ', ' ' , ' ', ' ' },
            { 'B', ' ', ' ' , ' ', 'B' }
        });
        yield return new object[] {
            grid,
            grid[0, 2]!,
            new[] {
                new CellCostPair(grid[1, 2]!, 1),
                new CellCostPair(grid[0, 1]!, 1),
                new CellCostPair(grid[0, 0]!, 2),
                new CellCostPair(grid[1, 0]!, 3),
                new CellCostPair(grid[2, 0]!, 4),
                new CellCostPair(grid[3, 0]!, 5),
                new CellCostPair(grid[4, 0]!, 6),
                new CellCostPair(grid[4, 1]!, 7),
                new CellCostPair(grid[4, 2]!, 8)
            }
        };
    }

    private static float GetDistance(Cell a, Cell b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}