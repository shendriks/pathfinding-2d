using MonoGame.Extended.Input;

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMapEditor.State;

public class BlockedCellClickedState : IState
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

            context.CurrentCell.IsWalkable = true;

            return;
        }

        context.TransitionTo(new ButtonReleasedState());
    }
}