using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;

public class NeighborFinder(GridNavigator navigator) : INeighborFinder
{
    // +-+-+-+-+-+-+
    // |1|2|3|4|5| |
    // +-+-+-+-+-+-+
    // |0| | | |6| |
    // +-+-+-+-+-+-+
    // |^| | | |7| |
    // +-+-+-+-+-+-+
    private static readonly Point[] JumpRightTrajectoryDeltas = [new(0, -1), new(0, -2), new(1, -2), new(2, -2), new(3, -2), new(4, -2), new(4, -1), new(4, 0)];

    // +-+-+-+-+-+-+
    // | |5|4|3|2|1|
    // +-+-+-+-+-+-+
    // | |6| | | |0|
    // +-+-+-+-+-+-+
    // | |7| | | |^|
    // +-+-+-+-+-+-+
    private static readonly Point[] JumpLeftTrajectoryDeltas = [new(0, -1), new(0, -2), new(-1, -2), new(-2, -2), new(-3, -2), new(-4, -2), new(-4, -1), new(-4, 0)];

    public IEnumerable<CellCostPair> FindNeighbors(
        Cell sourceCell,
        bool hasJumpCapability,
        INeighborFinder.GetDistanceDelegate dist
    )
    {
        foreach (var cell in FindCellsWithoutJumping(sourceCell, dist)) {
            yield return cell;
        }

        if (!hasJumpCapability || navigator.StartAt(sourceCell).Down.Cell is not { IsBlock: true }) {
            yield break;
        }

        foreach (var cell in FindCellsWithJumping(sourceCell, dist)) {
            yield return cell;
        }
    }

    private IEnumerable<CellCostPair> FindCellsWithoutJumping(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist
    )
    {
        var down = navigator.StartAt(cell).Down.Cell; 
        if (down is { IsBlock: false }) {
            var cost = dist(cell, down);
            yield return new CellCostPair(down, cost);
        }

        var up = navigator.StartAt(cell).Up.Cell;
        if (cell is { IsLadder: true } && up is { IsBlock: false }) {
            var cost = dist(cell, up);
            yield return new CellCostPair(up, cost);
        }

        var canMoveSideways = down is { IsEmpty: false } || cell.IsLadder || cell.IsHangingBar;
        if (!canMoveSideways) yield break;

        var right = navigator.StartAt(cell).Right.Cell;
        if (right is { IsBlock: false }) {
            var cost = dist(cell, right);
            yield return new CellCostPair(right, cost);
        }

        var left = navigator.StartAt(cell).Left.Cell;
        if (left is { IsBlock: false }) {
            var cost = dist(cell, left);
            yield return new CellCostPair(left, cost);
        }
    }

    private IEnumerable<CellCostPair> FindCellsWithJumping(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist
    )
    {
        // If we're ...
        if (navigator.StartAt(cell).DownRight.Cell is { IsEmpty: true }        // a) standing at the edge of a pit, or
            || navigator.StartAt(cell).Right.Right.Cell is { IsBlock: true }   // b) there is an obstacle, or
            || navigator.StartAt(cell).Right.UpRight.Cell is { IsBlock: true } // c) there's an elevated platform
        ) {
            // ... then try jumping to the right
            foreach (var cellCostPair in FindCellsOnJumpPath(cell, dist, JumpRightTrajectoryDeltas)) {
                yield return cellCostPair;
            }
        }

        if (navigator.StartAt(cell).DownLeft.Cell is { IsEmpty: true }
            || navigator.StartAt(cell).Left.Left.Cell is { IsBlock: true }
            || navigator.StartAt(cell).Left.UpLeft.Cell is { IsBlock: true }
        ) {
            foreach (var cellCostPair in FindCellsOnJumpPath(cell, dist, JumpLeftTrajectoryDeltas)) {
                yield return cellCostPair;
            }
        }
    }

    private IEnumerable<CellCostPair> FindCellsOnJumpPath(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist,
        IEnumerable<Point> deltaDirection
    )
    {
        var cost = 0f;
        var previousCell = cell;
        foreach (var delta in deltaDirection) {
            var potentialNeighbor = navigator.StartAt(cell).MoveBy(delta.X, delta.Y).Cell;
            if (potentialNeighbor is null or { IsBlock: true }) {
                // If we are outside the grid or there is a block in the path of the jump,
                // we stop and try the opposite direction.
                yield break;
            }

            cost += dist(previousCell, potentialNeighbor);

            yield return new CellCostPair(potentialNeighbor, cost);

            previousCell = potentialNeighbor;
        }
    }
}