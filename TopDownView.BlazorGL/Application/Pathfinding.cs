using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pathfinding2D.TopDownView.BlazorGL.Application.Form;
using Pathfinding2D.TopDownView.BlazorGL.Application.TileMap;
using Pathfinding2D.TopDownView.BlazorGL.Application.TileMapEditor;

namespace Pathfinding2D.TopDownView.BlazorGL.Application;

public class Pathfinding
{
    private const double StepDelay = 50;

    private readonly int[,] _map = {
        { 0, 0, 1, 0, 0, 0, 1, 0, 1, 0 },
        { 0, 0, 1, 0, 1, 1, 1, 0, 0, 0 },
        { 0, 0, 1, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 1, 1, 1, 1, 1, 1, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 1, 0, 0, 0, 0, 1, 0, 1, 1 },
        { 1, 1, 1, 1, 0, 1, 1, 0, 1, 0 },
        { 0, 0, 0, 0, 0, 1, 1, 0, 1, 0 },
        { 0, 1, 1, 1, 1, 1, 1, 1, 1, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
    };

    private readonly GridRenderer _gridRenderer;
    private readonly GridInputContext _gridInputContext;
    private readonly ControlPanel _controlPanel;
    private readonly AStarPathfinder _pathFinder;

    private bool _isPathfindingFinished;
    private double _lastStepOccurredAt;
    private bool _isPlaying;

    public Pathfinding()
    {
        var grid = Grid.CreateFromArray(_map);
        grid.StartPosition = new Point(1, 1);
        grid.TargetPosition = new Point(4, 2);
        grid.GridChanged += Reset;

        _gridRenderer = new GridRenderer(grid);
        _gridInputContext = new GridInputContext(grid);
        _pathFinder = new AStarPathfinder(grid);

        _controlPanel = new ControlPanel(new Point(8, _map.GetLength(0) * Constant.TileSize + Constant.HalfTileSize));
        _controlPanel.PlayButtonToggled += isSwitchedOn => { _isPlaying = isSwitchedOn; };
        _controlPanel.StepButtonClicked += Step;
        _controlPanel.ResetButtonClicked += Reset;
        _controlPanel.ShowParentButtonToggled += isSwitchedOn => { _gridRenderer.IsShowParent = isSwitchedOn; };
    }

    public void LoadContent(ContentManager cm)
    {
        _gridRenderer.LoadContent(cm);
        _controlPanel.LoadContent(cm);
    }

    public void Update(GameTime gameTime)
    {
        _gridInputContext.Update(gameTime);
        _controlPanel.Update(gameTime);

        if (!_isPlaying) return;
        if (_isPathfindingFinished) return;

        var timeSinceLastStep = gameTime.TotalGameTime.TotalMilliseconds - _lastStepOccurredAt;
        if (timeSinceLastStep < StepDelay) return;

        Step();
        _lastStepOccurredAt = gameTime.TotalGameTime.TotalMilliseconds;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _controlPanel.Draw(spriteBatch);
        _gridRenderer.Draw(spriteBatch);
    }

    private void Step()
    {
        _isPathfindingFinished = _pathFinder.Step();
    }

    private void Reset()
    {
        _pathFinder.Reset();
        _isPathfindingFinished = false;
    }
}