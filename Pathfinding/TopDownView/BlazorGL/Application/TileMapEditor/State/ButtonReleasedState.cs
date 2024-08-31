using MonoGame.Extended.Input;

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.TileMapEditor.State;

public class ButtonReleasedState : IState
{
    public void Handle(IContext context)
    {
        if (!context.MouseState.WasButtonPressed(MouseButton.Left)) {
            return;
        }

        if (context.Grid.StartPosition == context.CurrentCell.Position) {
            context.TransitionTo(new StartClickedState());
            return;
        }

        if (context.Grid.TargetPosition == context.CurrentCell.Position) {
            context.TransitionTo(new TargetClickedState());
            return;
        }

        if (context.CurrentCell.IsWalkable) {
            context.TransitionTo(new EmptyCellClickedState());
            if (context.Grid.StartPosition != context.CurrentCell.Position
                && context.Grid.TargetPosition != context.CurrentCell.Position
            ) {
                context.CurrentCell.IsWalkable = false;
            }

            return;
        }

        context.TransitionTo(new BlockedCellClickedState());
        context.CurrentCell.IsWalkable = true;
    }
}