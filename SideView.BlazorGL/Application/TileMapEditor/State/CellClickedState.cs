using MonoGame.Extended.Input;
using Pathfinding2D.SideView.BlazorGL.Application.TileMap;

namespace Pathfinding2D.SideView.BlazorGL.Application.TileMapEditor.State;

public abstract class CellClickedState : IState
{
    private bool _isFirstCall = true;

    protected abstract CellType NewCellType { get; }

    public void Handle(IContext context)
    {
        if (_isFirstCall) {
            context.CurrentCell.Type = NewCellType;
            _isFirstCall = false;
        }

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

            context.CurrentCell.Type = NewCellType;
            return;
        }

        context.TransitionTo(new ButtonReleasedState());
    }
}