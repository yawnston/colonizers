import sys
sys.path.insert(0, r"C:\Users\danie\Desktop\Skola\Colonizers")
from AICore.AICore import *

from random import seed, randint

class RandomAI(AIBase):
    def __init__(self):
        super().__init__()

    def messageCallback(self, gameState):
        return str(self.pickRandomAction(gameState)) # important to return string, not number

    def pickRandomAction(self, gameState):
        actionCount = len(gameState["Actions"])
        return randint(0, actionCount - 1)

if __name__ == "__main__":
    if len(sys.argv) != 2:
        raise Exception('AI Script must have 1 argument - name of named pipe')
    #seed(42) # Seed AI for reproducibility
    ai = RandomAI()
    ai.run(sys.argv[1])