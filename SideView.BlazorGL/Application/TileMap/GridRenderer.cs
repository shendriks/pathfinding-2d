using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using IDrawable = Pathfinding2D.SideView.BlazorGL.Application.Supportive.Interfaces.IDrawable;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMap;

public class GridRenderer(Grid grid) : IDrawable
{
    private Texture2D? _textureCellEmpty;
    private Texture2D? _textureCellBlock;
    private Texture2D? _textureCellLadder;
    private Texture2D? _textureCellHangingBar;
    private Texture2D? _textureCellStart;
    private Texture2D? _textureCellTarget;
    private SpriteFont? _font;

    public bool IsShowParent { get; set; }

    public void LoadContent(ContentManager cm)
    {
        _textureCellEmpty = cm.Load<Texture2D>("Textures/CellEmpty");
        _textureCellBlock = cm.Load<Texture2D>("Textures/CellBlock");
        _textureCellLadder = cm.Load<Texture2D>("Textures/CellLadder");
        _textureCellHangingBar = cm.Load<Texture2D>("Textures/CellHangingBar");
        _textureCellStart = cm.Load<Texture2D>("Textures/CellStart");
        _textureCellTarget = cm.Load<Texture2D>("Textures/CellTarget");
        _font = cm.Load<SpriteFont>("Fonts/Font1");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var cell in grid) {
            var position = new Vector2(cell.X, cell.Y) * Constant.TileSize;

            // draw cell
            DrawCell(spriteBatch, cell, position);

            // draw numbers
            if (cell.CostFromStart < float.PositiveInfinity) {
                DrawNumber(spriteBatch, cell.CostFromStart, position + new Vector2(3, 3));
            }

            if (cell.CostToTarget < float.PositiveInfinity) {
                DrawNumber(spriteBatch, cell.CostToTarget, position + new Vector2(3, 14));
            }

            DrawPathSegment(spriteBatch, cell, position);

            if (IsShowParent) {
                DrawLineToParentCell(spriteBatch, cell, position);
            }

            DrawMouseHover(spriteBatch, position);
        }
    }

    private void DrawCell(SpriteBatch spriteBatch, Cell cell, Vector2 position)
    {
        var texture = GetCellTexture(cell);
        if (texture != null) {
            spriteBatch.Draw(
                texture: texture,
                position: position,
                sourceRectangle: null,
                color: Color.White,
                rotation: 0,
                origin: Vector2.Zero,
                scale: Vector2.One,
                effects: SpriteEffects.None,
                layerDepth: 1f
            );
        }

        if (!TryGetCellColor(cell, out var color)) {
            return;
        }

        spriteBatch.DrawRectangle(
            rectangle: new RectangleF(position.X, position.Y, Constant.TileSize, Constant.TileSize),
            color: color,
            thickness: 2f,
            layerDepth: 0.8f
        );

        color.A = 120;
        spriteBatch.FillRectangle(
            rectangle: new RectangleF(position.X, position.Y, Constant.TileSize, Constant.TileSize),
            color: color,
            layerDepth: 0.9f
        );
    }

    private static void DrawPathSegment(SpriteBatch spriteBatch, Cell cell, Vector2 position)
    {
        if (!cell.IsOnPath) return;

        if (cell.WasInspected) {
            spriteBatch.DrawPoint(
                position: position + new Vector2(Constant.HalfTileSize, Constant.HalfTileSize - 1),
                color: Color.Green,
                size: cell.IsOnPath ? 10 : 6,
                layerDepth: 0.5f
            );
        }

        if (cell.Parent == null) return;

        var parentPosition = new Vector2(cell.Parent.X, cell.Parent.Y) * Constant.TileSize;
        spriteBatch.DrawLine(
            point1: position + Vector2.One * Constant.HalfTileSize,
            point2: parentPosition + Vector2.One * Constant.HalfTileSize,
            color: Color.Green,
            thickness: cell.IsOnPath ? 6 : 2,
            layerDepth: 0.5f
        );
    }

    private static void DrawLineToParentCell(SpriteBatch spriteBatch, Cell cell, Vector2 position)
    {
        if (cell.WasInspected) {
            spriteBatch.DrawPoint(
                position: position + new Vector2(Constant.HalfTileSize, Constant.HalfTileSize - 1),
                color: Color.DarkSlateGray,
                size: cell.IsOnPath ? 10 : 6,
                layerDepth: 0.4f
            );
        }

        if (cell.Parent == null) return;

        var parentPosition = new Vector2(cell.Parent.X, cell.Parent.Y) * Constant.TileSize;
        spriteBatch.DrawLine(
            point1: position + Vector2.One * Constant.HalfTileSize,
            point2: parentPosition + Vector2.One * Constant.HalfTileSize,
            color: Color.DarkSlateGray,
            thickness: cell.IsOnPath ? 6 : 2,
            layerDepth: 0.4f
        );
    }

    private void DrawNumber(SpriteBatch spriteBatch, float number, Vector2 position)
    {
        if (_font == null) return;
        spriteBatch.DrawString(
            spriteFont: _font,
            text: $"{number}",
            position: position,
            color: Color.Black,
            rotation: 0,
            origin: Vector2.Zero,
            scale: Vector2.One,
            effects: SpriteEffects.None,
            layerDepth: 0.3f
        );
    }

    private static void DrawMouseHover(SpriteBatch spriteBatch, Vector2 position)
    {
        var mousePosition = Mouse.GetState().Position;
        var rect = new RectangleF(position.X, position.Y, Constant.TileSize, Constant.TileSize);
        if (!rect.Contains(mousePosition.ToVector2())) return;

        var color = Color.Black;
        color.A = 20;
        spriteBatch.FillRectangle(
            rectangle: rect,
            color: color,
            layerDepth: 0.2f
        );
    }

    private Texture2D? GetCellTexture(Cell cell)
    {
        if (cell == grid.Start) return _textureCellStart;
        if (cell == grid.Target) return _textureCellTarget;
        return cell.Type switch {
            CellType.Empty => _textureCellEmpty,
            CellType.Block => _textureCellBlock,
            CellType.Ladder => _textureCellLadder,
            CellType.HangingBar => _textureCellHangingBar,
            _ => _textureCellEmpty
        };
    }

    private static bool TryGetCellColor(Cell cell, out Color color)
    {
        if (cell.IsCurrentlyBeingExamined) {
            color = Color.Red;
            return true;
        }
        if (cell.IsOnPath) {
            color = Color.DarkGreen;
            return true;
        }
        if (cell.IsInOpenSet) {
            color = Color.OrangeRed;
            return true;
        }
        if (cell.Parent != null) {
            color = Color.Orange;
            return true;
        }

        color = Color.White;
        return false;
    }
}