from AICore import *
from HeuristicIntelligence import HeuristicAI

import sys
from random import seed, randint, choice
from math import sqrt, log

class MCTSTreeNode:
    def __init__(self, action = None, parent = None, previousPlayer = None):
        self.action = action
        self.parent = parent
        self.children = []
        self.value = 0
        self.visitCount = 0
        self.availabilityCount = 1
        self.previousPlayer = previousPlayer
        
    def selectChild(self, possibleActions, explorationFactor = 0.7): # factor of 0.7 is from the ISMCTS paper
        possibleChildren = [child for child in self.children if child.action in possibleActions]
        # child with highest UCB1 score
        bestChild = max(possibleChildren, key = lambda c: c.getUCBScore(explorationFactor))
        # updating this here instead of during backprop is more convenient
        for child in possibleChildren:
            child.availabilityCount += 1
        
        return bestChild

    def getUCBScore(self, explorationFactor):
        return float(self.value) / float(self.visitCount) + explorationFactor * sqrt(log(self.availabilityCount) / float(self.visitCount))

    def hasUnvisitedActions(self, possibleActions):
        return len(self.getUnvisitedActions(possibleActions)) != 0
    
    def getUnvisitedActions(self, possibleActions):
        visitedActions = [child.action for child in self.children]
        return [action for action in possibleActions if action not in visitedActions]

    def addChild(self, action, previousPlayer):
        newChild = MCTSTreeNode(action = action, parent = self, previousPlayer = previousPlayer)
        self.children.append(newChild)
        return newChild

class ICMTS_AI(AIBase):
    def __init__(self):
        super().__init__()

    def messageCallback(self, gameState):
        return str(self.pickAction(gameState)) # important to return string, not number

    def pickAction(self, actualGameState):
        # Small optimization when we don't have a choice
        if len(actualGameState["Actions"]) == 1:
            return 0;

        rootNode = MCTSTreeNode()
        for i in range(200):
            if i % 10 == 0:
                print("ISMCTS simulating games - iteration " + str(i));

            currentNode = rootNode

            # DETERMINIZATION
            gameState = self.determinize()

            # SELECTION
            # continue while node is nonterminal and all its actions
            # have been explored
            while (not gameState["GameOver"]) and (not currentNode.hasUnvisitedActions(getActionStrings(gameState))):
                currentNode = currentNode.selectChild(getActionStrings(gameState))
                gameState = self.simulate(gameState["BoardState"], currentNode.action)

            # EXPANSION
            if not gameState["GameOver"]:
                unvisitedActions = currentNode.getUnvisitedActions(getActionStrings(gameState))
                actionToVisit = choice(unvisitedActions)
                currentNode = currentNode.addChild(actionToVisit, gameState["BoardState"]["PlayerTurn"])
                gameState = self.simulate(gameState["BoardState"], actionToVisit)

            # SIMULATION
            while not gameState["GameOver"]:
                gameState = self.simulateActionHeuristically(gameState)

            # BACKPROPAGATION
            while currentNode != None:
                currentNode.visitCount += 1
                if currentNode.previousPlayer != None:
                    currentNode.value += self.evaluatePlayerRanking(getPlayerRanking(gameState, currentNode.previousPlayer))
                currentNode = currentNode.parent

        # choose most visited node
        chosenActionString = max(rootNode.children, key=lambda c: c.visitCount).action
        print("ISMCTS choosing action: " + chosenActionString)
        return indexOfActionByString(actualGameState, chosenActionString)

    # Simulate a single action during playout randomly
    def simulateActionRandomly(self, gameState):
        return self.simulate(gameState["BoardState"], choice(getActionStrings(gameState)))

    # Simulate a single action during playout using the heuristic AI
    def simulateActionHeuristically(self, gameState):
        ai = HeuristicAI()
        chosenActionIndex = ai.pickAction(gameState)
        return self.simulate(gameState["BoardState"], getActionString(gameState["Actions"][chosenActionIndex]))

    # Convert player ranking to a scale of 1 to 0
    def evaluatePlayerRanking(self, ranking):
        if ranking == 1:
            return 1
        if ranking == 2:
            return 0.2
        if ranking == 3:
            return 0.1
        return 0

if __name__ == "__main__":
    if len(sys.argv) != 2:
        raise Exception('AI Script must have 1 argument - name of named pipe')
    seed(15) # Seed AI for reproducibility
    ai = ICMTS_AI()
    ai.run(sys.argv[1])
