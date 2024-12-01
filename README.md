# A* Pathfinding in 2D Games

[![.NET](https://github.com/shendriks/blog-code-examples/actions/workflows/dotnet.yml/badge.svg)](https://github.com/shendriks/blog-code-examples/actions/workflows/dotnet.yml)
[![Lint](https://github.com/shendriks/blog-code-examples/actions/workflows/super-linter.yml/badge.svg)](https://github.com/shendriks/blog-code-examples/actions/workflows/super-linter.yml)

This is a collection of code examples from the blog series https://shendriks.dev/series/a-pathfinding-in-2d-games/

In here you'll find the following projects:

| Blog Post                                                                                                                                                                                 | Projects                                                                      | Branch           |
|-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------|-------------------------------------------------------------------------------|------------------|
| Part 1: [A* Pathfinding in 2D Games: The Basics for a simple Top-down Scenario](https://shendriks.dev/posts/2024-07-13-a-star-pathfinding-in-2d-games-the-basics-for-top-down-scenarios/) | <ul><li>`TopDownView.Console`</li><li>`TopDownView.Console.Tests`</li></ul>   | `main`           |
| Part 2: [A* Pathfinding in 2D Games: A simple Top-down Scenario implemented with MonoGame/KNI](https://shendriks.dev/posts/2024-08-24-a-star-pathfinding-top-down-blazorgl/)              | <ul><li>`TopDownView.BlazorGL`</li><li>`TopDownView.BlazorGL.Tests`</li></ul> | `main`           |
| Part 3: [A* Pathfinding in 2D Games: Side-View](https://shendriks.dev/posts/2024-09-04-a-star-pathfinding-side-view/)                                                                     | <ul><li>`SideView.BlazorGL`</li><li>`SideView.BlazorGL.Tests`</li></ul>       | `main`           |
| Part 4: [A* Pathfinding in 2D Games: Addendum about Cyclic Dependencies](https://shendriks.dev/posts/2024-12-01-a-star-pathfinding-supplemental/)                                         | <ul><li>`SideView.BlazorGL`</li><li>`SideView.BlazorGL.Tests`</li></ul>       | `grid-navigator` |

## A few notes about the implementation

### Code Duplication

If you compare the applications, you'll notice that all projects have
similar classes like `Cell`, `Grid` or `AStarPathfinder` with quite a bit of duplicated code.

Duplicated code is generally considered bad practice (see [the DRY principle](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself)).
However, it seems acceptable in this case because

1. These are separate projects, each with its own set of requirements (e.g. a console application
   is meant to be called once, while a MonoGame application is meant to be interactive and game-like, e.g. you can step through
   the algorithm).
2. Trying to avoid duplicated code by extracting superclasses into a separate project makes the relatively simple
   examples unnecessarily complicated.
3. The code is unlikely to change.

### About not unsubscribing from Events

In general, it's good practice to unsubscribe from events when you no longer need them.
In this example, however, we'll skip the step of unsubscribing for simplicity's sake, because it's just one level, if you will, and the
"game" is not progressing in the sense that a new level or area needs to be loaded while the current one is being unloaded, or
new game objects are being created or existing ones are being destroyed. The only time we would need to unsubscribe is when the
whole game ends, in which case everything will end up in data nirvana anyway.

### Testing

You'll find a few rudimentary tests in the test projects. The coverage is probably not super high. 
