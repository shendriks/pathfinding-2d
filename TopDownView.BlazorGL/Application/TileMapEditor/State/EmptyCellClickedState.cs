using MonoGame.Extended.Input;

namespace Pathfinding2D.TopDownView.BlazorGL.Application.TileMapEditor.State;

public class EmptyCellClickedState : IState
{
    public void Handle(IContext context)
    {
        if (context.MouseState.IsButtonDown(MouseButton.Left)) {
            if (!context.MouseState.PositionChanged) {
                return;
            }

            if (context.CurrentCell == context.PreviousCell) {
                return;
            }

            if (context.CurrentCell.Position.Equals(context.Grid.StartPosition)
                || context.CurrentCell.Position.Equals(context.Grid.TargetPosition)
               ) {
                return;
            }

            context.CurrentCell.IsWalkable = false;
            return;
        }

        context.TransitionTo(new ButtonReleasedState());
    }
}