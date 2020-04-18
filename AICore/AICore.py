import time
import struct
import os
import sys
import json
from abc import ABC, abstractmethod

# Base class which provides communication with the game engine for AIs
# Make sure to have the root folder on your Python PATH
class AIBase(ABC):
    def __init__(self):
        super().__init__()

    def initPipe(self, pipeName):
        while True:
            try:
                # open named pipe from Windows pipe directory
                self.pipe = open('\\\\.\\pipe\\' + pipeName, 'r+b', 0)
                print('Pipe opened: ' + pipeName)
                return
            except Exception as e:
                print('Error while connecting to pipe, waiting: ')
                print(e)
                time.sleep(2)

    # Implement in descendants to communicate with game engine
    @abstractmethod
    def messageCallback(self, gameState):
        pass

    # Get a determinized version of the current game state
    def determinize(self):
        messageToSend = "determinize"
        self.writeMessage(messageToSend)
        return self.readMessage()

    # Simulate the given move and get the resulting game state
    def simulate(self, boardState, move):
        dto = SimulationDTO(boardState, move)
        messageToSend = json.dumps(dto.__dict__)
        self.writeMessage(messageToSend)
        return self.readMessage()

    # run the AI with the given pipe as the communication channel
    # to the game engine
    def run(self, pipeName):
        self.initPipe(pipeName)
        while True:
            gameState = self.readMessage()
            messageToSend = self.messageCallback(gameState)
            self.writeMessage(messageToSend)

    # read message from pipe
    # messages have a simple protocol of an int32, which specifies
    # message (ASCII) length in bytes
    def readMessage(self):
        lengthBytes = self.pipe.read(4)
        if not lengthBytes:
            print('Pipe closed, exiting AI')
            sys.exit()
        length = struct.unpack('I', lengthBytes)[0]
        receivedMessage = self.pipe.read(length).decode('ascii')               
        self.pipe.seek(0)
        gameState = json.loads(receivedMessage)
        self.fixColonistInfoDictKeys(gameState)
        return gameState

    # write message to pipe
    def writeMessage(self, message):
        self.pipe.write(struct.pack('I', len(message)))
        self.pipe.write(message.encode("ascii"))
        self.pipe.seek(0)

    # convert the keys in colonist into dict from str to int
    def fixColonistInfoDictKeys(self, gameState):
        for player in gameState["BoardState"]["Players"]:
            player["ColonistInformation"] = {int(k):v for k,v in player["ColonistInformation"].items()}

# DTO for requesting simulation from the game engine
class SimulationDTO:
    def __init__(self, boardState, action):
        self.BoardState = boardState
        self.Action = action

#
# Some utility functions for working with the game state
#
def getColonyModuleCountByColor(gameState, playerNumber, color):
    # player ID is unique -> we know this result contains one element
    playerColony = next((x["Colony"] for x in gameState["BoardState"]["Players"] if x["ID"] == playerNumber))
    rightColorModules = (1 for m in playerColony if m["Type"] == color)
    return sum(rightColorModules)

# Get sorted list of tuples (color, moduleCount) descending
def getColonyModuleCountPerColor(gameState, playerNumber):
    colors = ["Red", "Green", "Blue"] # Ignore modules w/o color - they do not provide any bonus
    return sorted(((c, getColonyModuleCountByColor(gameState, playerNumber, c)) for c in colors), key=lambda x: x[1], reverse=True)

# Find the first instance of a matching element in a list and return its index,
# or None if not present
def indexOfFirstOrDefault(iterable, condition):
    for i, item in enumerate(iterable):
        if condition(item):
            return i
    return None

# Get player on turn
def getCurrentPlayer(gameState):
    return getPlayerByNumber(gameState, gameState["BoardState"]["PlayerTurn"])

# Get all players except the player on turn
def getOtherPlayers(gameState):
    currentPlayer = getCurrentPlayer(gameState)
    return [p for p in gameState["BoardState"]["Players"] if p["ID"] != currentPlayer["ID"]]

# Return player by number
def getPlayerByNumber(gameState, playerNumber):
    return next(x for x in gameState["BoardState"]["Players"] if x["ID"] == playerNumber)

# Get hand size of given player
def getHandSize(gameState, playerNumber):
    player = getPlayerByNumber(gameState, playerNumber)
    return len(player["Hand"])

# Get string representations of all available actions
def getActionStrings(gameState):
    return [getActionString(a) for a in gameState["Actions"]]

# Get string representation of the action
def getActionString(action):
    name = action["Type"]
    if name == "BuildModule" or name == "KeepModule":
        name += (" " + action["Module"])
    elif name == "StealOmnium" or name == "SwapHands":
        name += (" " + action["Target"])
    elif name == "ColonistPick":
        name += (" " + action["Colonist"])
    return name

# Get index of action by its action string
def indexOfActionByString(gameState, actionString):
    strList = [getActionString(a) for a in gameState["Actions"]]
    return strList.index(actionString)

# Get end ranking for the given player
# From 1 (best) to 4 (worst)
def getPlayerRanking(gameState, playerNumber):
    playerEndInfo = next(p for p in gameState["GameEndInfo"]["Players"] if p["Player"]["ID"] == playerNumber)
    return playerEndInfo["Ranking"]