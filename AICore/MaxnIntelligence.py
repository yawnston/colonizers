import sys
sys.path += '.'

from AICore import *

from random import seed, randint

class MaxnNode:
    def __init__(self, gameState, isTerminal = False):
        self.gameState = gameState
        self.isTerminal = gameState["GameOver"] or isTerminal
        self.children = []
        self.player = gameState["BoardState"]["PlayerTurn"]

    # Get heuristic evaluation for this node
    def evaluate(self):
        boardState = self.gameState["BoardState"]
        values = [0 for x in boardState["Players"]]
        for player in boardState["Players"]:
            value = 0

            for module in player["Colony"]:
                value += module["VictoryValue"]
            value += 0.6 * len(player["Hand"])
            value += 0.3 * player["Omnium"]
            if len(player["Colony"]) == 8:
                value += 4

            values[player["ID"] - 1] = value

        return values

class MaxnAI(AIBase):
    def __init__(self):
        super().__init__()

    DETERMINIZE_COUNT = 10
    SEARCH_DEPTH = 7

    def messageCallback(self, gameState):
        return str(self.pickAction(gameState)) # important to return string, not number

    def pickAction(self, actualGameState):
        # Small optimization when we don't have a choice
        if len(actualGameState["Actions"]) == 1:
            return 0

        # Counter for number of times the given action was the best
        actionValues = [0 for x in range(len(actualGameState["Actions"]))]
        for i in range(self.DETERMINIZE_COUNT):
            gameState = self.determinize()
            rootNode = MaxnNode(gameState, 0)
            payoffs = self.maxnPayoffs(rootNode, self.SEARCH_DEPTH)
            
            bestIndex = max(range(len(payoffs)), key=lambda i: payoffs[i])
            actionValues[bestIndex] += 1

        return max(range(len(actionValues)), key=lambda i: actionValues[i])

    def maxnPayoffs(self, node, depth):
        if node.isTerminal:
            return [node.evaluate()]

        for actionIndex in range(len(node.gameState["Actions"])):
            action = node.gameState["Actions"][actionIndex];
            newState = self.simulate(node.gameState["BoardState"], getActionString(action))
            node.children.append(MaxnNode(newState, depth == 1))

        pa = [self.maxn(child, depth - 1) for child in node.children]
        return pa

    def maxn(self, node, depth):
        return max(self.maxnPayoffs(node, depth), key=lambda p: p[node.player - 1])

if __name__ == "__main__":
    if len(sys.argv) != 2:
        raise Exception('AI Script must have 1 argument - name of named pipe')
    seed(99) # Seed AI for reproducibility
    ai = MaxnAI()
    ai.run(sys.argv[1])
