## Game state JSON

The game server communicates with clients using JSON and the game engine communicates with the Python AI scripts using JSON. This document details the contents of these JSON files.

There are 2 types of JSON documents sent by the game engine. The regular game state JSON is the format that will be used for nearly all communication. The only exception is the last JSON sent by the game engine, which is a special JSON document containing information on how the game ended.

**Regular game state JSON**
This file will have the following contents:
 - GameOver: false
 - Actions: [*Action*]
 - Board: *Board*

***Action***

 - Type: *ActionType*

***ActionType***

A string representing the action's type. Will be one of the following:

 - BuildModule {M}
 - BuildNothing
 - ColonistPick {C}
 - DoNothing
 - DrawModules
 - KeepModule {M}
 - StealOmnium {T}
 - SwapHands {T}
 - TakeOmnium

Actions marked with {M} also contain an element Module: *Module* 

Actions marked with {C} also contain an element Colonist: *Colonist* 

Actions marked with {T} also contain an element Target: *Colonist* 


***Board***

 - CurrentPlayer: *CurrentPlayer*
 - Players: [*Player*] (does not include the current player)
 - PlayableColonists: [*ColonistString*]
 - PlayerTurn: Number of the current player
 - GamePhase: [*GamePhaseString*]

***CurrentPlayer***

 - Colonist: *ColonistString*
 - Omnium: non-negative integer
 - Hand: [*Module*]
 - Colony: [*Module*]
 - Number: non-negative integer

***Player***

 - Omnium: non-negative integer
 - Handsize: non-negative integer
 - Colony: [*Module*]
 - Number: non-negative integer

***ColonistString***

A string representing a colonist. Will be one of the following:

 - Ecologist
 - General
 - Miner
 - Opportunist
 - Spy
 - Visionary

***Module***

 - Type: *Color*
 - BuildCost: non-negative integer
 - VictoryValue: non-negative integer

***Color***

A string representing the color of a Module. Will be one of the following:

 - Green
 - Blue
 - Red
 - None

***GamePhaseString***

A string representing the current game phase. Will be one of the following:

 - ColonistPick
 - Draw
 - Discard
 - Power
 - Build

**Game end JSON**
This will be sent if the game has concluded. It contains the results of the game. Its structure is the following:

 - GameOver: true
 - Board: *Board*
 - GameEndInfo: [*PlayerEndInfo*]

***PlayerEndInfo***

 - Player: *Player*
 - Points: non-negative integer

