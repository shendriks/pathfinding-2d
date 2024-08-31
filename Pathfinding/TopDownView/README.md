# Basic A* Pathfinding for a simple Top-down Scenario

These are code examples from the blog post
https://shendriks.dev/posts/2024-07-13-a-star-pathfinding-in-2d-games-the-basics-for-top-down-scenarios/

In here you'll find the following projects:
* The console application in the folder `Console`
* The KNI BlazorGL application in the folder `BlazorGL`
* Test projects are in the folders `Console.Tests` and `BlazorGL.Tests`

```text
.
├── BlazorGL
├── BlazorGL.Tests
├── Console
└── Console.Tests
```

There is another more detailed blog post about the implementation of the BlazorGL application at
https://shendriks.dev/posts/2024-08-24-a-star-pathfinding-top-down-blazorgl/

## A few notes about the implementation

### Code Duplication

If you compare the console application to the BlazorGL project, you'll notice that both projects have
similar classes like `Cell`, `Grid` or `AStarPathfinder` with quite a bit of duplicated code.

Duplicated code is generally considered bad practice (see [the DRY principle](https://en.wikipedia.org/wiki/Don%27t_repeat_yourself)).
However, it seems acceptable in this case because

1. These are two separate projects, each with its own set of requirements (e.g. the console application
is meant to be called once, while the BlazorGL is meant to be interactive and more game-like, e.g. you can step through
the algorithm).
2. Trying to avoid duplicate code by extracting superclasses into a separate project makes the relatively simple
examples unnecessarily complicated.
3. The code is unlikely to change.

### Not unsubscribing from Events

In general, it's good practice to unsubscribe from events when you no longer need them.
In this example, however, we'll skip the step of unsubscribing for simplicity's sake, because it's just one level, if you will, and the
"game" is not progressing in the sense that a new level or area needs to be loaded while the current one is being unloaded, or
new game objects are being created or existing ones are being destroyed. The only time we would need to unsubscribe is when the
whole game ends, in which case everything will end up in data nirvana anyway.

### Testing

You'll find a few rudimentary tests in here. The coverage is probably not too high. 
