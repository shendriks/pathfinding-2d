using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;

public class NeighborFinder : INeighborFinder
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

        if (!hasJumpCapability || sourceCell.Down is not { IsBlock: true }) {
            yield break;
        }

        foreach (var cell in FindCellsWithJumping(sourceCell, dist)) {
            yield return cell;
        }
    }

    private static IEnumerable<CellCostPair> FindCellsWithoutJumping(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist
    )
    {
        if (cell.Down is { IsBlock: false }) {
            var cost = dist(cell, cell.Down);
            yield return new CellCostPair(cell.Down, cost);
        }

        if (cell is { IsLadder: true, Up.IsBlock: false }) {
            var cost = dist(cell, cell.Up);
            yield return new CellCostPair(cell.Up, cost);
        }

        var canMoveSideways = cell.Down is { IsEmpty: false } || cell.IsLadder || cell.IsHangingBar;
        if (!canMoveSideways) yield break;

        if (cell.Right is { IsBlock: false }) {
            var cost = dist(cell, cell.Right);
            yield return new CellCostPair(cell.Right, cost);
        }

        if (cell.Left is { IsBlock: false }) {
            var cost = dist(cell, cell.Left);
            yield return new CellCostPair(cell.Left, cost);
        }
    }

    private static IEnumerable<CellCostPair> FindCellsWithJumping(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist
    )
    {
        // If we're ...
        if (cell.DownRight is { IsEmpty: true }         // a) standing at the edge of a pit, or
            || cell.Right?.Right is { IsBlock: true }   // b) there is an obstacle, or
            || cell.Right?.UpRight is { IsBlock: true } // c) there's an elevated platform
        ) {
            // ... then try jumping to the right
            foreach (var cellCostPair in FindCellsOnJumpPath(cell, dist, JumpRightTrajectoryDeltas)) {
                yield return cellCostPair;
            }
        }

        if (cell.DownLeft is { IsEmpty: true }
            || cell.Left?.Left is { IsBlock: true }
            || cell.Left?.UpLeft is { IsBlock: true }
        ) {
            foreach (var cellCostPair in FindCellsOnJumpPath(cell, dist, JumpLeftTrajectoryDeltas)) {
                yield return cellCostPair;
            }
        }
    }

    private static IEnumerable<CellCostPair> FindCellsOnJumpPath(
        Cell cell,
        INeighborFinder.GetDistanceDelegate dist,
        IEnumerable<Point> deltaDirection
    )
    {
        var cost = 0f;
        var previousCell = cell;
        foreach (var delta in deltaDirection) {
            var potentialNeighbor = cell.NeighborAt(delta.X, delta.Y);
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