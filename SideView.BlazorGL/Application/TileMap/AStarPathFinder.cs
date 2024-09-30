using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public class AStarPathfinder(Grid grid, INeighborFinder neighborFinder)
{
    private List<Cell> _openSet = [];

    /// <summary>Returns an enumerator which iterates over single steps of the algorithm.</summary>
    public IEnumerator FindPathCoroutine(bool hasJumpCapability)
    {
        grid.Start.CostFromStart = 0;
        grid.Start.CostToTarget = GetDistance(grid.Start, grid.Target);
        _openSet = [grid.Start];

        while (_openSet.Count > 0) {
            var current = _openSet
                .OrderBy(c => c.CostToTarget)
                .ThenBy(c => GetDistance(c, grid.Target))
                .First();

            if (current == grid.Target) {
                ReconstructPath(current);
                yield break;
            }

            _openSet.Remove(current);
            current.IsInOpenSet = false;
            current.IsCurrentlyBeingExamined = true;
            yield return null;

            foreach (var neighbor in neighborFinder.FindNeighbors(current, hasJumpCapability, GetDistance)) {
                if (!IsNeighborWorthTrying(current, neighbor)) continue;
                _openSet.Add(neighbor.Cell);
                neighbor.Cell.WasInspected = true;
                neighbor.Cell.IsInOpenSet = true;
                yield return null;
            }

            current.IsCurrentlyBeingExamined = false;
        }
    }

    private bool IsNeighborWorthTrying(Cell current, CellCostPair neighbor)
    {
        var tentativeCost = current.CostFromStart + neighbor.Cost;
        if (tentativeCost >= neighbor.Cell.CostFromStart) return false;
        neighbor.Cell.Parent = current;
        neighbor.Cell.CostFromStart = tentativeCost;
        neighbor.Cell.CostToTarget = tentativeCost + GetDistance(neighbor.Cell, grid.Target);
        return !_openSet.Contains(neighbor.Cell);
    }

    private static void ReconstructPath(Cell current)
    {
        current.IsOnPath = true;

        var walkingCell = current;
        while (walkingCell.Parent != null) {
            walkingCell.Parent.IsOnPath = true;
            walkingCell = walkingCell.Parent;
        }
    }

    private static float GetDistance(Cell a, Cell b)
    {
        return Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);
    }
}