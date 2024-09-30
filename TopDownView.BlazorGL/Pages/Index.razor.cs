using Microsoft.JSInterop;
using Microsoft.Xna.Framework;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Pathfinding2D.TopDownView.BlazorGL.Pages;

public partial class Index
{
    private Game? _game;

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender) {
            JsRuntime.InvokeAsync<object>("initRenderJS", DotNetObjectReference.Create(this));
        }
    }

    [JSInvokable]
    public void TickDotNet()
    {
        // init game
        if (_game == null) {
            _game = new PathfindingGame();
            _game.Run();
        }

        // run gameloop
        _game.Tick();
    }
}