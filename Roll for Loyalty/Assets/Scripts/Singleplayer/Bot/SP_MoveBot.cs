using System.Collections.Generic;
using UnityEngine;

public class SP_MoveBot : MonoBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private SP_Characters characters;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Directions directions;
    [SerializeField] private SP_Plus plus;
    [SerializeField] private SP_Cheating cheat;

    private Dictionary<GameObject, int> movementToTile = new Dictionary<GameObject, int>();
    private Dictionary<GameObject, GameObject> previousTile = new Dictionary<GameObject, GameObject>();
    private PriorityQueue<GameObject> frontier = new PriorityQueue<GameObject>();
    private GameObject startingTile;
    private int movement;
    #endregion
    #region Functions
    public bool MoveBotTowardsPlus()
    {
        SP_Bot bot = characters.GetActiveCharacter().GetComponent<SP_Bot>();
        movement = bot.Movement;

        startingTile = gameBoard.GetGameObject(bot.transform.position);
        RunDijkstra(startingTile);

        GameObject[] pluses = GameObject.FindGameObjectsWithTag("Plus");
        List<(GameObject plus, GameObject entryTile, int totalCost)> reachablePluses = new List<(GameObject plusObject, GameObject entryTile, int totalCost)>();

        GameObject nearestPlus = null;
        GameObject nearestEntryTile = null;
        int fastestWay = int.MaxValue;

        foreach (GameObject plus in pluses)
        {
            List<GameObject> entryTiles = GetAdjacentTilesToPlus(plus);

            GameObject bestEntryTile = null;
            int bestfastestWay = fastestWay;

            foreach (GameObject entryTile in entryTiles)
            {
                if (movementToTile.ContainsKey(entryTile))
                {
                    int totalCost = movementToTile[entryTile] + 1;
                    if (totalCost < bestfastestWay)
                    {
                        bestfastestWay = totalCost;
                        bestEntryTile = entryTile;
                    }
                }
            }
            if (bestEntryTile != null)
            {
                if (bestfastestWay < fastestWay)
                {
                    fastestWay = bestfastestWay;
                    nearestPlus = plus;
                    nearestEntryTile = bestEntryTile;
                }
                if (bestfastestWay <= movement)
                {
                    reachablePluses.Add((plus, bestEntryTile, bestfastestWay));
                }
            }
        }
        if (reachablePluses.Count > 0)
        {
            if (cheat.Cheat && pluses.Length > 0)
            {
                int chanceToCheat = Mathf.FloorToInt(Random.Range(1, 5) / 4);
                if (chanceToCheat == 1)
                {
                    GameObject randomPlus = pluses[Random.Range(0, pluses.Length)];
                    foreach ((GameObject plus, GameObject entryTile, int totalCost) reachablePlus in reachablePluses)
                    {
                        if (reachablePlus.plus == randomPlus)
                        {
                            bot.Movement -= reachablePlus.totalCost;
                            bot.transform.position = randomPlus.transform.position;
                            plus.PlusClicked(randomPlus);
                            return true;
                        }
                    }
                    bot.Cheated = true;
                    bot.Movement = 0;
                    bot.transform.position = randomPlus.transform.position;
                    plus.PlusClicked(randomPlus);
                    return true;
                }
            }

            (GameObject plus, GameObject entryTile, int totalCost) chosenPlus = reachablePluses[Random.Range(0, reachablePluses.Count)];
            bot.Movement -= chosenPlus.totalCost;
            bot.transform.position = chosenPlus.plus.transform.position;
            plus.PlusClicked(chosenPlus.plus);
            return true;
        }
        else if (nearestPlus != null && nearestEntryTile != null)
        {
            GameObject bestTile = FastestTileOnPath(nearestEntryTile, movement);
            int moveCost = movementToTile[bestTile];
            bot.Movement -= moveCost;
            bot.transform.position = bestTile.transform.position;
            return true;
        }
        return false;
    }
    
    //Run Dijkstra
    private void RunDijkstra(GameObject startTile)
    {
        ClearPathfindingData();
        movementToTile[startTile] = 0;
        frontier.Enqueue(startTile, 0);

        while (frontier.Count > 0)
        {
            GameObject currentTile = frontier.Dequeue();

            if (!movementToTile.ContainsKey(currentTile))
            { 
                continue;
            }

            int currentCost = movementToTile[currentTile];
            foreach (GameObject neighbor in GetNeighbors(currentTile))
            {
                SP_Tile neighborTile = neighbor.GetComponent<SP_Tile>();
                int newCost = currentCost + neighborTile.MovementCost;
                if (!movementToTile.ContainsKey(neighbor) || newCost < movementToTile[neighbor])
                {
                    movementToTile[neighbor] = newCost;
                    previousTile[neighbor] = currentTile;
                    frontier.Enqueue(neighbor, newCost);
                }
            }
        }
    }

    private void ClearPathfindingData()
    {
        movementToTile.Clear();
        previousTile.Clear();
        frontier = new PriorityQueue<GameObject>();
    }

    //Provides the neighbouring tiles
    private List<GameObject> GetNeighbors(GameObject tileObject)
    {
        List<GameObject> neighbors = new List<GameObject>();
        SP_Tile tile = tileObject.GetComponent<SP_Tile>();

        Vector3 currentPosition = tileObject.transform.position;
        foreach ((int x, int y) koordinate in directions.koordinates)
        {
            Vector3 newPosition = new Vector3(currentPosition.x + koordinate.x, currentPosition.y + koordinate.y, 0);

            if (!gameBoard.onPosition(newPosition))
            { 
                continue;
            }
            GameObject neighborObject = gameBoard.GetGameObject(newPosition);
            if (!neighborObject.CompareTag("Tile"))
            { 
                continue;
            }
            if (!CanMoveFromTile(tile, koordinate))
            { 
                continue;
            }
            neighbors.Add(neighborObject);
        }
        return neighbors;
    }

    //Return if there is a door
    private bool CanMoveFromTile(SP_Tile tile, (int x, int y) koordinate)
    {
        switch (koordinate)
        {
            case (0, 2):
                return tile.NorthDoor;
            case (2, 0):
                return tile.EastDoor;
            case (0, -2):
                return tile.SouthDoor;
            case (-2, 0):
                return tile.WestDoor;
            default:
                return false;
        }
    }

    //Provides the neighbouring tiles
    private List<GameObject> GetAdjacentTilesToPlus(GameObject plus)
    {
        List<GameObject> adjacentTiles = new List<GameObject>();

        Vector3 plusPosition = plus.transform.position;
        foreach ((int x, int y) koordinate in directions.koordinates)
        {
            Vector3 tilePosition = new Vector3(plusPosition.x + koordinate.x, plusPosition.y + koordinate.y, 0);
            if (!gameBoard.onPosition(tilePosition))
            { 
                continue;
            }

            GameObject obj = gameBoard.GetGameObject(tilePosition);
            if (obj.CompareTag("Tile"))
            {
                adjacentTiles.Add(obj);
            }
        }
        return adjacentTiles;
    }

    //Return the minimum movement cost tile
    private GameObject FastestTileOnPath(GameObject targetTile, int max)
    {
        List<GameObject> path = ReconstructPath(targetTile);
        GameObject bestTile = startingTile;

        foreach (GameObject tile in path)
        {
            if (movementToTile.ContainsKey(tile) && movementToTile[tile] <= max)
            {
                bestTile = tile;
            }
        }
        return bestTile;
    }

    private List<GameObject> ReconstructPath(GameObject targetTile)
    {
        List<GameObject> path = new List<GameObject>();
        if (movementToTile.ContainsKey(targetTile))
        { 
            return path;
        }
        GameObject current = targetTile;
        path.Add(current);
        while (previousTile.ContainsKey(current))
        {
            current = previousTile[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }
    #endregion
}