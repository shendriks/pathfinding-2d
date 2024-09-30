using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using IDrawable = Pathfinding2D.TopDownView.BlazorGL.Application.Supportive.Interfaces.IDrawable;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.TileMap;

public class GridRenderer(Grid grid) : IDrawable
{
    private Texture2D? _textureCellFree;
    private Texture2D? _textureCellBlocked;
    private Texture2D? _textureCellStart;
    private Texture2D? _textureCellTarget;
    private SpriteFont? _font;

    public bool IsShowParent { get; set; }

    public void LoadContent(ContentManager cm)
    {
        _textureCellFree = cm.Load<Texture2D>("Textures/CellFree");
        _textureCellBlocked = cm.Load<Texture2D>("Textures/CellBlocked");
        _textureCellStart = cm.Load<Texture2D>("Textures/CellStart");
        _textureCellTarget = cm.Load<Texture2D>("Textures/CellTarget");
        _font = cm.Load<SpriteFont>("Fonts/Font1");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        foreach (var cell in grid) {
            var position = new Vector2(cell.X, cell.Y) * Constant.TileSize;

            // draw cell
            var texture = GetCellTexture(cell);
            if (texture != null) {
                spriteBatch.Draw(
                    texture: texture,
                    position: position,
                    sourceRectangle: null,
                    color: GetCellColor(cell),
                    rotation: 0,
                    origin: Vector2.Zero,
                    scale: Vector2.One,
                    effects: SpriteEffects.None,
                    layerDepth: 1
                );
            }

            // draw numbers
            if (cell.CostFromStart < float.PositiveInfinity) {
                DrawNumber(spriteBatch, cell.CostFromStart, position + new Vector2(2, 1));
            }

            if (cell.CostToTarget < float.PositiveInfinity) {
                DrawNumber(spriteBatch, cell.CostToTarget, position + new Vector2(2, 12));
            }

            if (IsShowParent) {
                DrawLineToParentCell(spriteBatch, cell, position);
            }
        }
    }

    private static void DrawLineToParentCell(SpriteBatch spriteBatch, Cell cell, Vector2 position)
    {
        if (cell.WasInspected) {
            spriteBatch.DrawPoint(
                position: position + new Vector2(Constant.HalfTileSize, Constant.HalfTileSize - 1),
                color: Color.DarkSlateGray,
                size: cell.IsOnPath ? 10 : 6,
                layerDepth: 0.5f
            );
        }

        if (cell.Parent == null) return;

        var parentPosition = new Vector2(cell.Parent.X, cell.Parent.Y) * Constant.TileSize;
        spriteBatch.DrawLine(
            point1: position + Vector2.One * Constant.HalfTileSize,
            point2: parentPosition + Vector2.One * Constant.HalfTileSize,
            color: Color.DarkSlateGray,
            thickness: cell.IsOnPath ? 6 : 2,
            layerDepth: 0.5f
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
            layerDepth: 0
        );
    }

    private Texture2D? GetCellTexture(Cell cell)
    {
        if (!cell.IsWalkable) return _textureCellBlocked;
        if (cell == grid.Start) return _textureCellStart;
        return cell == grid.Target ? _textureCellTarget : _textureCellFree;
    }

    private static Color GetCellColor(Cell cell)
    {
        if (cell.IsOnPath) return Color.LightGreen;
        if (cell.IsInOpenSet) return Color.Orange;
        return cell.Parent != null ? Color.Yellow : Color.White;
    }
}