using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Input;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL;

public class PathfindingGame : Game
{
    private readonly Color _backgroundColor = new(35, 36, 37);

    private SpriteBatch _spriteBatch;
    private Application.Pathfinding _pathfinding;

    public PathfindingGame()
    {
        _ = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _pathfinding = new Application.Pathfinding();
        _pathfinding.LoadContent(Content);
    }

    protected override void UnloadContent()
    {
        Content.Unload();
    }

    protected override void Update(GameTime gameTime)
    {
        MouseExtended.Update();
        _pathfinding.Update(gameTime);
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(_backgroundColor);

        _spriteBatch.Begin(
            samplerState: SamplerState.PointClamp,
            sortMode: SpriteSortMode.BackToFront
        );
        _pathfinding.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}