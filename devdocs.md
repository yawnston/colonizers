## Developer documentation

The whole project consists of 4 parts:

 - Core game pipeline (Game)
 - User CLI (CLI)
 - CLI for Python interoperability (PythonCLI)
 - Game server (Server)

Outside use of this project is done through the last 3, their APIs are described in `userdocs.md`, also contained in this repository.

Also contained in this project are two sample Python AI scripts for use with the PythonCLI (SimpleIntelligence and GreedySimpleIntelligence) and a proof-of-concept slim game client that communicates with the game server.

**Core game pipeline**

The API for the game pipeline is simple: IGameAction goes in, GameState goes out. In practical terms, this means that the pipeline receives a board state and an action to perform with it, and it outputs a game state (a board state + a list of actions that can be performed on this board state).

The pipeline is build using the mediator pattern (embodied by the use of MediatR) and it makes heavy use of dependency injection. The used DI framework is the ASP.NET DI framework, available as Microsoft.Extensions.DependencyInjection on the NuGet store.

The pipeline starts with the Resolver class, which takes the command it receives and passes it to a mediator. This finds the proper CommandHandler, which will modify the board state object accordingly. This CommandHandler will then call an appropriate ActionGetter, which is a class that accepts a board state and returns a list of actions that can be performed on this board state.

The pipeline project also contains the serialization functionality, which is used when converting the board states to JSON. JSON.NET is used for all JSON-related operations, available as Newtonsoft.Json on the NuGet store.

New instances of a game are built using the BoardFactory and GameFactory factories.

Typical usage of this pipeline consists of building a GameState object and inserting it into the pipeline. Then the pipeline returns a new GameState and this is performed in a loop, until the pipeline returns a GameState object with GameOver set to `true`. This means that the game has ended, and the GameState object's GameEndInfo property contains information on how the game ended.

**User CLI**

A simple command-line abstraction over the pipeline API. It is a loop over the pipeline, giving user input to the pipeline and in return showing the user the game state in a nice, presentable way.

**CLI for Python interoperability**

In some ways, it's similar to the User CLI. It's an abstraction over the game pipeline, only this time it calls Python functions to resolve which actions to give back to the pipeline for execution.

As the use case is expected to be repeated calling for AI / machine learning purposes, the Python code is not interpreted, but compiled. Each input script is compiled at the start of the application's lifetime, this ensures faster execution times. IronPython is used for this.

**Game server**

A simple client-server approach is used. Clients connect to the game server (implemented via C# Socket), and the server immediately initializes a game session for that client and sends the client the first game state (JSON, see `boardstatejson.md` in this repository for more details). The server then waits for the client to respond with an integer indicating which action they wish to take from the provided list. It passes this to the game pipeline, and the cycle continues.

If for any reason an error is encountered during the communication process (e.g. client disconnect), this error is logged to standard output on the server and the connection is closed.

When the server detects that the game is over (the pipeline returns a specially marked GameState object), after this is sent to the client, the connection is closed.
