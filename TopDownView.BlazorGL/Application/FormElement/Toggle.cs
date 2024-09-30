using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Pathfinding2D.TopDownView.BlazorGL.Application.Supportive.Interfaces;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.FormElement;

public class Toggle(Texture2D textureSwitchedOff, Texture2D textureSwitchedOn, Point position) : IDrawableUpdateable
{
    private bool _isPressed;
    private bool _isSwitchedOn;
    private Rectangle _worldBounds = new(position.X, position.Y, textureSwitchedOff.Width, textureSwitchedOff.Height);

    public event Action<bool>? Toggled;

    public void Update(GameTime _)
    {
        var mouseState = Mouse.GetState();
        if (!_isPressed && mouseState.LeftButton == ButtonState.Pressed && _worldBounds.Contains(mouseState.Position)) {
            _isPressed = true;
            _isSwitchedOn = !_isSwitchedOn;
            Toggled?.Invoke(_isSwitchedOn);
        }
        if (mouseState.LeftButton == ButtonState.Released) {
            _isPressed = false;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            texture: _isSwitchedOn ? textureSwitchedOn : textureSwitchedOff,
            position: new Vector2(position.X, position.Y),
            sourceRectangle: null,
            color: _isPressed ? Color.White : Color.LightGray
        );
    }
}