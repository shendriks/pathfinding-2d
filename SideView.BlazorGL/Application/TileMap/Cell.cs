using System;
using Microsoft.Xna.Framework;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public class Cell(int x, int y, CellType type)
{
    private CellType _type = type;

    public Point Position { get; } = new(x, y);
    public int X => Position.X;
    public int Y => Position.Y;

    public CellType Type {
        get => _type;
        set {
            if (value == _type) return;
            CellTypeChanged?.Invoke();
            _type = value;
        }
    }

    public float CostToTarget { get; set; } = float.PositiveInfinity;
    public float CostFromStart { get; set; } = float.PositiveInfinity;
    public Cell? Parent { get; set; }
    public bool IsEmpty => Type == CellType.Empty;
    public bool IsBlock => Type == CellType.Block;
    public bool IsLadder => Type == CellType.Ladder;
    public bool IsHangingBar => Type == CellType.HangingBar;
    public bool IsOnPath { get; set; }
    public bool IsInOpenSet { get; set; }
    public bool WasInspected { get; set; }
    public bool IsCurrentlyBeingExamined { get; set; }

    public event Action? CellTypeChanged;

    public void Reset()
    {
        CostToTarget = float.PositiveInfinity;
        CostFromStart = float.PositiveInfinity;
        Parent = null;
        IsOnPath = false;
        IsInOpenSet = false;
        WasInspected = false;
        IsCurrentlyBeingExamined = false;
    }

    public override string ToString()
    {
        return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Type)}: {Type}";
    }
}