using MonoGame.Extended.Input;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.TileMapEditor.State;

public class TargetClickedState : IState
{
    public void Handle(IContext context)
    {
        if (context.MouseState.IsButtonDown(MouseButton.Left)) {
            if (!context.MouseState.PositionChanged) {
                return;
            }

            if (context.CurrentCell == context.PreviousCell || !context.CurrentCell.IsWalkable) {
                return;
            }

            context.Grid.TargetPosition = context.CurrentCell.Position;
            return;
        }

        context.TransitionTo(new ButtonReleasedState());
    }
}