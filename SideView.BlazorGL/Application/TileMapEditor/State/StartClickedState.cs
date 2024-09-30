using MonoGame.Extended.Input;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor.State;

public class StartClickedState : IState
{
    public void Handle(IContext context)
    {
        if (context.MouseState.IsButtonDown(MouseButton.Left)) {
            if (!context.MouseState.PositionChanged) {
                return;
            }

            if (context.CurrentCell == context.PreviousCell || !context.CurrentCell.IsEmpty) {
                return;
            }

            context.Grid.StartPosition = context.CurrentCell.Position;
            return;
        }

        context.TransitionTo(new ButtonReleasedState());
    }
}