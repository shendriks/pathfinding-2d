using BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMap;
using BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMapEditor.State;
using MonoGame.Extended.Input;

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMapEditor;

public interface IContext
{
    MouseStateExtended MouseState { get; }
    Cell CurrentCell { get; }
    Cell? PreviousCell { get; }
    Grid Grid { get; }
    void TransitionTo(IState state);
}