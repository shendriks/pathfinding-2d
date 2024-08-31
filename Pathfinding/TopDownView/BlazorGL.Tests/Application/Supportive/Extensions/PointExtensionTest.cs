using System.Collections.Generic;
using BlogCodeExamples.Pathfinding.TopDownView.BlazorGL.Application.Supportive.Extensions;
using Microsoft.Xna.Framework;
using NUnit.Framework;

namespace BlazorGL.Tests.Application.Supportive.Extensions;

[TestFixture]
[TestOf(typeof(PointExtension))]
public class PointExtensionTest
{

    [Test, Sequential]
    public void TestDivBy(
        [Values(3, 2, 4, 10)] int divBy,
        [ValueSource(nameof(PointProvider))] Point expected
    )
    {
        var point = new Point(9, 27);

        var actual = point.DivBy(divBy);

        Assert.That(actual, Is.EqualTo(expected));
    }

    public static IEnumerable<Point> PointProvider()
    {
        yield return new Point(3, 9); // divBy 3
        yield return new Point(4, 13); // divBy 2
        yield return new Point(2, 6); // divBy 4
        yield return new Point(0, 2); // divBy 10
    }
}