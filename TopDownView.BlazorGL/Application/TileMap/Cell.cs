using System;
using Microsoft.Xna.Framework;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.TileMap;

public class Cell(int x, int y, bool isWalkable)
{
    private bool _isWalkable = isWalkable;

    public static Cell None { get; } = new(0, 0, false);
    public Point Position { get; } = new(x, y);
    public int X => Position.X;
    public int Y => Position.Y;
    public bool IsWalkable {
        get => _isWalkable;
        set {
            if (value == _isWalkable) return;
            _isWalkable = value;
            WalkabilityChanged?.Invoke();
        }
    }
    public float CostToTarget { get; set; } = float.PositiveInfinity;
    public float CostFromStart { get; set; } = float.PositiveInfinity;
    public Cell? Parent { get; set; }
    public bool IsOnPath { get; set; }
    public bool IsInOpenSet { get; set; }
    public bool WasInspected { get; set; }
    public event Action? WalkabilityChanged;

    public void Reset()
    {
        CostToTarget = float.PositiveInfinity;
        CostFromStart = float.PositiveInfinity;
        Parent = null;
        IsOnPath = false;
        IsInOpenSet = false;
        WasInspected = false;
    }

    public override string ToString()
    {
        return $"{nameof(Position)}: {Position}, {nameof(IsWalkable)}: {IsWalkable}, " +
               $"{nameof(CostToTarget)}: {CostToTarget}, {nameof(CostFromStart)}: {CostFromStart}";
    }
}