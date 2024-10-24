using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Pathfinding2D.SideView.BlazorGL.Application.Form;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap.Neighbor;
using Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor;

namespace Pathfinding2D.SideView.BlazorGL.Application;

public class Pathfinding
{
    private const double StepDelay = 50;

    // (B)lock, (H)anging Bar, (L)adder
    private readonly char[,] _map = {
        { ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B', 'B', 'B' },
        { ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'H', 'H', 'L' },
        { ' ', ' ', ' ', ' ', 'B', 'B', 'B', ' ', ' ', 'L' },
        { 'B', 'B', ' ', ' ', ' ', 'B', 'B', ' ', ' ', 'L' },
        { ' ', ' ', ' ', ' ', ' ', 'B', 'B', 'B', 'B', 'L' },
        { ' ', ' ', ' ', 'B', 'B', 'B', 'H', 'H', 'H', 'L' },
        { ' ', ' ', ' ', ' ', ' ', 'B', ' ', ' ', 'B', 'L' },
        { 'B', 'B', ' ', ' ', ' ', 'B', 'B', ' ', 'B', 'L' },
        { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'B', 'L' },
        { ' ', ' ', 'B', 'B', ' ', ' ', ' ', ' ', 'B', 'L' },
        { ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', 'L' },
        { 'B', 'B', 'B', 'B', 'B', 'B', 'B', 'B', 'B', 'B' }
    };
    private readonly GridRenderer _gridRenderer;
    private readonly GridInputContext _gridInputContext;
    private readonly ControlPanel _controlPanel;
    private readonly AStarPathfinder _pathFinder;
    private readonly Grid _grid;

    private IEnumerator _findPathCoroutine;
    private bool _isPathfindingFinished;
    private double _lastStepOccurredAt;
    private bool _isPlaying;
    private bool _hasJumpCapability;

    public Pathfinding()
    {
        _grid = new Grid(_map) {
            StartPosition = new Point(0, 10),
            TargetPosition = new Point(5, 1)
        };
        _grid.GridChanged += Reset;

        _gridRenderer = new GridRenderer(_grid);
        _gridInputContext = new GridInputContext(_grid);
        _pathFinder = new AStarPathfinder(_grid, new NeighborFinder());
        _findPathCoroutine = _pathFinder.FindPathCoroutine(_hasJumpCapability);

        _controlPanel = new ControlPanel(new Point(0, _map.GetLength(0) * Constant.TileSize));
        _controlPanel.PlayButtonToggled += isSwitchedOn => { _isPlaying = isSwitchedOn; };
        _controlPanel.StepButtonClicked += Step;
        _controlPanel.ResetButtonClicked += Reset;
        _controlPanel.ShowParentButtonToggled += isSwitchedOn => { _gridRenderer.IsShowParent = isSwitchedOn; };
        _controlPanel.JumpingButtonToggled += hasJumpCapability => {
            _hasJumpCapability = hasJumpCapability;
            Reset();
        };
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
        if (_isPathfindingFinished) return;
        _isPathfindingFinished = !_findPathCoroutine.MoveNext();
    }

    private void Reset()
    {
        _grid.Reset();
        _findPathCoroutine = _pathFinder.FindPathCoroutine(_hasJumpCapability);
        _isPathfindingFinished = false;
    }
}