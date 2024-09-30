using MonoGame.Extended.Input;
using Pathfinding2D.TopDownView.BlazorGL.Application.TileMap;
using Pathfinding2D.TopDownView.BlazorGL.Application.TileMapEditor.State;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.TileMapEditor;

public interface IContext
{
    MouseStateExtended MouseState { get; }
    Cell CurrentCell { get; }
    Cell? PreviousCell { get; }
    Grid Grid { get; }
    void TransitionTo(IState state);
}