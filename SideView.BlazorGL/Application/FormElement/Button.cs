using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathfinding2D.SideView.BlazorGL.Application.Supportive.Interfaces;

namespace Pathfinding2D.SideView.BlazorGL.Application.FormElement;

public class Button(Texture2D texture, Point position) : IDrawableUpdateable
{
    private bool _isPressed;
    private Rectangle _worldBounds = new(position.X, position.Y, texture.Width, texture.Height);

    public event Action? Clicked;

    public void Update(GameTime _)
    {
        var mouseState = Mouse.GetState();
        if (!_isPressed && mouseState.LeftButton == ButtonState.Pressed && _worldBounds.Contains(mouseState.Position)) {
            _isPressed = true;
            Clicked?.Invoke();
        }
        if (mouseState.LeftButton == ButtonState.Released) {
            _isPressed = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: texture,
            position: new Vector2(position.X, position.Y),
            sourceRectangle: null,
            color: _isPressed ? Color.White : Color.LightGray
        );
    }
}