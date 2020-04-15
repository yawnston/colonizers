import sys
sys.path.insert(0, r"C:\Users\danie\Desktop\Skola\Colonizers")
from AICore.AICore import *

import json
from random import seed, randint

# AI based on heuristics - simply a collection of rules of thumb used by humans
class HeuristicAI(AIBase):
    def __init__(self):
        super().__init__()

    def messageCallback(self, gameState):
        return str(self.pickAction(gameState)) # important to return string, not number

    def pickAction(self, gameState):
        phase = gameState["BoardState"]["GamePhase"]
    
        if phase == "ColonistPick":
            return self.processColonistPick(gameState)
        if phase == "Draw":
            return self.processDraw(gameState)
        if phase == "Discard":
            return self.processDiscard(gameState)
        if phase == "Power":
            return self.processPower(gameState)
        if phase == "Build":
            return self.processBuild(gameState)

        return -1 # should never get here

    def pickRandomAction(self, gameState):
        actionCount = len(gameState["Actions"])
        return randint(0, actionCount - 1)

    def processColonistPick(self, gameState):
        # pick color-related colonist if it makes a lot of omnium
        moduleCountsPerColor = getColonyModuleCountPerColor(gameState, gameState["BoardState"]["PlayerTurn"])
        bestColorCount = moduleCountsPerColor[0][1]
        colonistToPick = None
        if bestColorCount >= 3:
            if moduleCountsPerColor[0][0] == "Red":
                colonistToPick = "General"
            if moduleCountsPerColor[0][0] == "Green":
                colonistToPick = "Ecologist"
            if moduleCountsPerColor[0][0] == "Blue":
                colonistToPick = "Miner"
        colonistPickIndex = indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Colonist"] == colonistToPick)
        if colonistPickIndex != None:
            return colonistPickIndex

        # pick Visionary if 0-1 modules in hand
        if getHandSize(gameState, gameState["BoardState"]["PlayerTurn"]) <= 1:
            colonistToPick = "Visionary"
            colonistPickIndex = indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Colonist"] == colonistToPick)
            if colonistPickIndex != None:
                return colonistPickIndex

        # pick a random colonist if no other criteria are met
        return self.pickRandomAction(gameState)

    def processDraw(self, gameState):
        currentPlayer = getCurrentPlayer(gameState)
        handSize = len(currentPlayer["Hand"])
        omnium = currentPlayer["Omnium"]

        # if at 0 omnium or 4+ cards then extract
        if omnium == 0 or handSize >= 4:
            return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Type"] == "TakeOmnium")

        # if at 0 cards then draw
        if handSize == 0:
            return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Type"] == "DrawModules")

        # otherwise pick randomly
        return self.pickRandomAction(gameState)

    def processDiscard(self, gameState):
        player = getCurrentPlayer(gameState)
        omnium = player["Omnium"]
        # if anyone has 7 or 8 modules then keep the highest value affordable
        # module
        if self.doesAnyPlayerHaveManyModules(gameState):
            moduleToKeep = self.getMostValuableAffordableModule(gameState["BoardState"]["DiscardTempStorage"], omnium, lambda x: x["VictoryValue"])
            if moduleToKeep != None:
                return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToKeep["Name"])

        # if player has 5+ modules then keep the highest bonus value module
        if len(player["Hand"]) >= 5:
            moduleToKeep = self.getMostValuableAffordableModule(gameState["BoardState"]["DiscardTempStorage"], omnium, lambda x: x["VictoryValue"] - x["BuildCost"])
            if moduleToKeep != None:
                return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToKeep["Name"])

        # otherwise keep the module which synergizes with the most built
        # modules
        colorCounts = getColonyModuleCountPerColor(gameState, player["ID"])
        colorToPick = colorCounts[0][0]
        rightColorModules = [m for m in gameState["BoardState"]["DiscardTempStorage"] if m["Type"] == colorToPick]
        moduleToKeep = self.getMostValuableAffordableModule(rightColorModules, omnium, lambda x: x["VictoryValue"] - x["BuildCost"])
        if moduleToKeep != None:
            return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToKeep["Name"])

        # if such a module is not possible to keep then build randomly
        return self.pickRandomAction(gameState)

    def processPower(self, gameState):
        player = getCurrentPlayer(gameState)
        # if opportunist: find player with most omnium and randomly choose from
        # the information set
        if player["Colonist"]["Name"] == "Opportunist":
            pass # TODO: implement information sets

        # if spy: find player with more cards and randomly choose from the
        # information set, if such a player does not exist then pass
        if player["Colonist"]["Name"] == "Spy":
            pass

        # otherwise randomly choose power usage (should be only 1 choice
        # at this point anyway)
        return self.pickRandomAction(gameState)

    def processBuild(self, gameState):
        player = getCurrentPlayer(gameState)
        omnium = player["Omnium"]
        # if anyone has 7 or 8 modules then build the highest victory value
        # module possible
        if self.doesAnyPlayerHaveManyModules(gameState):
            moduleToBuild = self.getMostValuableAffordableModule(player["Hand"], omnium, lambda x: x["VictoryValue"])
            if moduleToBuild != None:
                return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToBuild["Name"])

        # if player has 5+ modules then build the highest bonus value module
        if len(player["Hand"]) >= 5:
            moduleToBuild = self.getMostValuableAffordableModule(player["Hand"], omnium, lambda x: x["VictoryValue"] - x["BuildCost"])
            if moduleToBuild != None:
                return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToBuild["Name"])

        # otherwise build the module which synergizes with the most built
        # modules
        colorCounts = getColonyModuleCountPerColor(gameState, player["ID"])
        colorToPick = colorCounts[0][0]
        rightColorModules = [m for m in player["Hand"] if m["Type"] == colorToPick]
        moduleToBuild = self.getMostValuableAffordableModule(rightColorModules, omnium, lambda x: x["VictoryValue"] - x["BuildCost"])
        if moduleToBuild != None:
            return indexOfFirstOrDefault(gameState["Actions"], lambda a: a["Module"] == moduleToBuild["Name"])

        # if such a module is not possible to build then build randomly
        return self.pickRandomAction(gameState)

    def doesAnyPlayerHaveManyModules(self, gameState):
        for player in gameState["BoardState"]["Players"]:
            if len(player["Colony"]) >= 7:
                return True
        return False

    def getMostValuableAffordableModule(self, modules, omnium, key):
        filteredModules = (m for m in modules if m["BuildCost"] <= omnium)
        sortedModules = sorted(filteredModules, key=key, reverse=True)
        if len(sortedModules) > 0:
            return sortedModules[0]
        return None

seed(97) # Seed AI for reproducibility
if len(sys.argv) != 2:
    raise Exception('AI Script must have 1 argument - name of named pipe')
ai = HeuristicAI()
ai.initPipe(sys.argv[1])
ai.messageLoop()


