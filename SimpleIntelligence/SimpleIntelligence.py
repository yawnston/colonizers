import json

def processState(jsonString):
    gameState = json.loads(jsonString)
    print(json.dumps(gameState, indent=4, sort_keys=True))
    return 0