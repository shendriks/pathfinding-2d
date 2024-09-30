using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;
using Pathfinding2D.SideView.BlazorGL.Application.Supportive.Extensions;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap;
using Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor.State;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor;

/// <summary>
/// A simple implementation of a finite state machine to handle mouse input on the grid.
/// </summary>
public class GridInputContext(Grid grid) : IContext
{
    private IState _currentState = new ButtonReleasedState();

    public MouseStateExtended MouseState { get; private set; }
    public Cell CurrentCell { get; private set; } = Cell.None;
    public Cell? PreviousCell { get; private set; }
    public Grid Grid { get; } = grid;

    public void TransitionTo(IState state)
    {
        _currentState = state;
    }

    public void Update(GameTime _)
    {
        MouseState = MouseExtended.GetState();
        var gridPosition = MouseState.Position.DivBy(Constant.TileSize);

        if (!Grid.TryGetCellAtPosition(gridPosition, out var cell)) {
            return;
        }

        CurrentCell = cell;
        _currentState.Handle(this);
        PreviousCell = cell;
    }
}