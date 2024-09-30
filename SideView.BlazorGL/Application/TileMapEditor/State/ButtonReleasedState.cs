using MonoGame.Extended.Input;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor.State;

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

        if (context.CurrentCell.IsEmpty) {
            context.TransitionTo(new EmptyCellClickedState());
            return;
        }

        if (context.CurrentCell.IsBlock) {
            context.TransitionTo(new BlockClickedState());
            return;
        }

        if (context.CurrentCell.IsLadder) {
            context.TransitionTo(new LadderClickedState());
            return;
        }

        if (context.CurrentCell.IsHangingBar) {
            context.TransitionTo(new HangingBarClickedState());
        }
    }
}