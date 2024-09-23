using Microsoft.Xna.Framework;

namespace BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.Supportive.Extensions;

public static class PointExtension
{
    public static Point DivBy(this Point left, int right)
    {
        return new Point(left.X / right, left.Y / right);
    }
}