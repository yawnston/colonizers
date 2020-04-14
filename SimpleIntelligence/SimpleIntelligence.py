import json
from random import randint

def processState(jsonString):
    gameState = json.loads(jsonString) # convert json string to dictionary

    # DEBUGGING PRINT
    #print(json.dumps(gameState, indent=4))

    phase = gameState["Board"]["GamePhase"]
    
    if phase == "ColonistPick":
        return processColonistPick(gameState)
    if phase == "Draw":
        return processDraw(gameState)
    if phase == "Discard":
        return processDiscard(gameState)
    if phase == "Power":
        return processPower(gameState)
    if phase == "Build":
        return processBuild(gameState)

    return -1 # should never get here

def processColonistPick(gameState):
    # pick a random colonist
    actionCount = len(gameState["Actions"])
    return randint(0, actionCount - 1)

def processDraw(gameState):
    # if at least 3 cards are in hand, take omnium
    if len(gameState["Board"]["CurrentPlayer"]["Hand"]) >= 3:
        for index, a in enumerate(gameState["Actions"]):
            if a["Type"] == "TakeOmnium":
                break
        return index

    # otherwise randomly decide if we take omnium or draw modules
    return randint(0, 1)

def processDiscard(gameState):
    # randomly choose a module to keep
    return randint(0, 1)

def processPower(gameState):
    # randomly choose power usage
    actionCount = len(gameState["Actions"])
    return randint(0, actionCount - 1)

def processBuild(gameState):
    # attempt to play the highest value module we can afford
    actionCount = len(gameState["Actions"])
    if actionCount == 1: # we cannot build anything -> we can't choose
        return 0
    #we know we can build something -> remove the BuildNothing action before picking
    realBuildActions = [i for i in gameState["Actions"] if i["Type"] != "BuildNothing"]

    bestBuilding = max(action["Module"]["VictoryValue"] for action in realBuildActions)

    for index, a in enumerate(gameState["Actions"]):
        if a["Module"]["VictoryValue"] == bestBuilding:
            break

    return index

import time
import struct
import os
from datetime import datetime

def initPipe():
    while True:
        try:
            f = open(r'\\.\pipe\\NPTest', 'r+b', 0)
            return f
        except:
            print('waiting for pipe')
            time.sleep(2)

f = initPipe()
print('pipe exists')

while True:
    n = struct.unpack('I', f.read(4))[0]    # Read str length
    s = f.read(n).decode('ascii')                          # Read str
    f.seek(0)                               # Important!!!
    #print('Python Read:', s)

    s = '0'
    f.write(struct.pack('I', len(s)))   # Write str length and str
    f.write(s.encode("ascii"))
    f.seek(0)                               # EDIT: This is also necessary