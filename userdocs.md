## User documentation

There are three ways users can interact with the game.

**#1: command-line interface**
This is the `CLI` project contained in this repository. It should be run from the command line with no arguments. It will interactively let users play the game in hot-seat mode. The program does not explain the rules, so it is recommended that players first familiarize themselves with the game's rules using the rulebook(rules.md) provided in this repository.

Whenever the program asks for a player's input, it will show the player the game board from their point of view: it shows them everyone's colonies, and they can see their own hand, but only the card counts from other players. The player whose input is required is indicated by the player number included in the program's output. When the game ends, the program outputs players' scored and terminates.

**#2: Python-interoperable program**
This is the `PythonCLI` project contained in this repository. It should be run from the command line with 4 arguments. These arguments will be paths to Python scripts, which will be queried by the program for gameplay decisions.

Example invocation: `dotnet PythonCLI.dll Script1.py Script2.py Script3.py Script4.py`
*Note: the scripts do not have to be different. It is acceptable to use the same script as multiple parameters.*

There is also an optional 5th parameter. This should be provided when the user Python scripts require the use of external libraries. The IronPyton runtime is not able to find these on its own, so in order to use these in your scripts, you must provide a path to the libraries in this argument. For example, if the Python standard libraries are located at `C:\Python27\Lib` on my PC and I want to say `import json` in my scripts, an invocation could look like this: `dotnet PythonCLI.dll Script1.py Script2.py Script3.py Script4.py C:\Python27\Lib`

There are set semantics for how the game calls your Python code. Your script **must** contain a function named `processState` taking one string parameter and returning an integer. This function will be called by the game. It will be given a string containing a JSON representation of the current game state. The user script should then use this information to choose an action from the options provided inside the JSON, and return its number. The number returned should be the chosen action's position in the action list. For example if I want to choose the third action in the list, I would return `2`, because the list is zero-indexed. Refer to `gamestatejson.md` in this repository for information on what the JSON document contains.

If you are looking for examples of Python AI scripts, refer to the `SimpleIntelligence` and `GreedySimpleIntelligence` projects contained in this repository.

**#3: game server**
This is the `Server` project contained in this repository. Run the server application with no command-line arguments. The server will then run on `localhost:4141`. It will await TCP connections, and upon establising a connection, will immediately send the initial game state to the client. This transmission consists of a 4 byte integer, specifying how long the payload will be, and then a payload whose byte length is specified earlier. This payload is a JSON string detailing the current game state. Refer to `gamestatejson.md` in this repository for information on what the JSON document contains. As a response, the client will send a 4 byte integer containing the number of the action they chose to take. For example, if the desired action is 4th in the action list, the client would send back 4 bytes, 3 of them zeroes and one containing the integer `3`, because the list is zero-indexed.

Interrupting this connection at any point will cause the server to forget all data related to your session, thus discarding your game progress. When the game ends, the server will send a special end-of-game JSON also described in `gamestatejson.md` and then close the connection.

As a demonstration, this repository also contains a `SampleWebClient` project. This project acts as a client connecting to the aforementioned server. It is a proof-of-concept, simply retrieving game state JSON strings, deserializing them, writing them to the standard output and waiting for the user to choose and action. This action will then be sent back to the server.

Even though only a simple proof-of-concept client is provided, the server API is designed in such a way that it would allow for example not just a slim client, but a whole graphical application to be present at the client machine. This would for example allow users to play the game on their machine, but having the server resolve all the game logic. The graphical client would only need to build the board from the provided JSON, get the player's response and return it to the server.
