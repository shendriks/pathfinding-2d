using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public class Grid : IEnumerable<Cell>
{
    private readonly Cell[,] _cells;
    private Point _startPosition;
    private Point _targetPosition;

    public Cell? this[int x, int y] => IsInsideBounds(new Point(x, y)) ? _cells[x, y] : null;

    public Point StartPosition {
        get => _startPosition;
        set {
            if (_startPosition == value) return;
            if (!IsInsideBounds(value)) return;
            if (!_cells[value.X, value.Y].Type.Equals(CellType.Empty)) return;
            GridChanged?.Invoke();
            _startPosition = value;
        }
    }
    public Point TargetPosition {
        get => _targetPosition;
        set {
            if (_targetPosition == value) return;
            if (!IsInsideBounds(value)) return;
            if (!_cells[value.X, value.Y].Type.Equals(CellType.Empty)) return;
            GridChanged?.Invoke();
            _targetPosition = value;
        }
    }
    public Cell Start => _cells[StartPosition.X, StartPosition.Y];
    public Cell Target => _cells[TargetPosition.X, TargetPosition.Y];

    public event Action? GridChanged;

    private Grid(Cell[,] cells)
    {
        _cells = cells;
        foreach (var cell in this) {
            cell.Grid = this;
            cell.CellTypeChanged += () => GridChanged?.Invoke();
        }
    }

    public static Grid CreateFromArray(char[,] map)
    {
        if (map.Length == 0) {
            throw new ArgumentException("map must contain at least 1 element");
        }

        var cells = new Cell[map.GetLength(1), map.GetLength(0)];
        for (var y = 0; y < map.GetLength(0); y++) {
            for (var x = 0; x < map.GetLength(1); x++) {
                cells[x, y] = new Cell(x, y, CellTypeFromChar(map[y, x]));
            }
        }

        return new Grid(cells);
    }

    private static CellType CellTypeFromChar(char c)
    {
        return c switch {
            ' ' => CellType.Empty,
            'B' => CellType.Block,
            'L' => CellType.Ladder,
            'H' => CellType.HangingBar,
            _ => throw new ArgumentException($"Invalid cell type: {c}")
        };
    }

    private bool IsInsideBounds(Point position)
    {
        var (x, y) = position;
        return x >= 0 && x < _cells.GetLength(0) && y >= 0 && y < _cells.GetLength(1);
    }

    /// <summary>Try get the cell at the given position.</summary>
    /// <param name="position">The position to get the cell at from the grid</param>
    /// <param name="cell">The cell at the position if it exists</param>
    /// <returns>true on success, false on failure</returns>
    public bool TryGetCellAtPosition(Point position, out Cell cell)
    {
        if (!IsInsideBounds(position)) {
            cell = Cell.None;
            return false;
        }

        cell = _cells[position.X, position.Y];
        return true;
    }

    public void Reset()
    {
        foreach (var cell in _cells) {
            cell.Reset();
        }
    }

    public IEnumerator<Cell> GetEnumerator()
    {
        for (var x = 0; x < _cells.GetLength(0); x++) {
            for (var y = 0; y < _cells.GetLength(1); y++) {
                yield return _cells[x, y];
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}