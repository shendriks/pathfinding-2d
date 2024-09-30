using MonoGame.Extended.Input;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap;
using Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor.State;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor;

public interface IContext
{
    MouseStateExtended MouseState { get; }
    Cell CurrentCell { get; }
    Cell? PreviousCell { get; }
    Grid Grid { get; }
    void TransitionTo(IState state);
}