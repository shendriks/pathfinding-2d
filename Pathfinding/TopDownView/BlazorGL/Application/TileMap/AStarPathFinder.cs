using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMap;

public class AStarPathfinder(Grid grid)
{
    private List<Cell> _openSet = [];
    private bool _needsInit = true;

    /// <summary>Evaluates one step of the algorithm</summary>
    /// <returns>True if done (i.e. a path was found or there is no path), false otherwise</returns>
    public bool Step()
    {
        if (_needsInit) {
            grid.Start.CostFromStart = 0;
            grid.Start.CostToTarget = GetDistance(grid.Start, grid.Target);
            _openSet = [grid.Start];
            _needsInit = false;
        }

        if (_openSet.Count == 0) return true;

        var current = _openSet.OrderBy(c => c.CostToTarget).First();
        if (current == grid.Target) {
            ReconstructPath(current);
            return true;
        }

        _openSet.Remove(current);
        current.IsInOpenSet = false;

        var tentativeCost = current.CostFromStart + 1;
        foreach (var n in Neighbors(current).Where(n => tentativeCost < n.CostFromStart)) {
            n.Parent = current;
            n.CostFromStart = tentativeCost;
            n.CostToTarget = tentativeCost + GetDistance(n, grid.Target);
            if (_openSet.Contains(n)) continue;

            _openSet.Add(n);
            n.WasInspected = true;
            n.IsInOpenSet = true;
        }

        return false;
    }

    public void Reset()
    {
        grid.Reset();
        _needsInit = true;
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

    private IEnumerable<Cell> Neighbors(Cell cell)
    {
        int[] dx = [-1, 1, 0, 0];
        int[] dy = [0, 0, -1, 1];

        for (var i = 0; i < 4; i++) {
            var checkX = cell.X + dx[i];
            var checkY = cell.Y + dy[i];

            if (grid.TryGetCellAtPosition(new Point(checkX, checkY), out var neighbor) && neighbor.IsWalkable) {
                yield return neighbor;
            }
        }
    }
}