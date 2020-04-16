import time
import struct
import os
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
                print('Error while connecting to pipe, waiting: ' + e)
                time.sleep(2)

    # Implement in descendants to communicate with game engine
    @abstractmethod
    def messageCallback(self, gameState):
        pass

    def messageLoop(self):
        while True:
            # read message from pipe
            # messages have a simple protocol of an int32, which specifies
            # message (ASCII) length in bytes
            lengthBytes = self.pipe.read(4)
            if not lengthBytes:
                print('Pipe closed, exiting AI')
                break
            length = struct.unpack('I', lengthBytes)[0]
            receivedMessage = self.pipe.read(length).decode('ascii')               
            self.pipe.seek(0)

            # calculate response and send it back through the pipe
            gameState = json.loads(receivedMessage)
            self.fixColonistInfoDictKeys(gameState)
            mesageToSend = self.messageCallback(gameState)
            self.pipe.write(struct.pack('I', len(mesageToSend)))
            self.pipe.write(mesageToSend.encode("ascii"))
            self.pipe.seek(0)

    # convert the keys in colonist into dict from str to int
    def fixColonistInfoDictKeys(self, gameState):
        for player in gameState["BoardState"]["Players"]:
            player["ColonistInformation"] = {int(k):v for k,v in player["ColonistInformation"].items()}

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