using System.Collections.Generic;
using UnityEngine;

public class MP_MoveCharacter : MonoBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private MP_Characters characters;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Directions directions;
    [SerializeField] private MP_Cheating cheat;
    //GameObjects
    private GameObject startingTile;
    private GameObject targetTile;
    private GameObject endTarget;
    private GameObject character;

    private Dictionary<GameObject, GameObject> nextTileToGoal = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, int> costToReachTile = new Dictionary<GameObject, int>();
    private PriorityQueue<GameObject> frontier = new PriorityQueue<GameObject>();
    private int movement;
    #endregion
    #region Functions
    //Move player
    public bool CanMoveCharacter(Vector3 endTargetPosition)
    {
        character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            startingTile = gameBoard.GetGameObject(character.transform.position);
            endTarget = gameBoard.GetGameObject(endTargetPosition);
            targetTile = GetTargetTile(endTarget);

            ClearPathfindingData();
            costToReachTile[startingTile] = 0;
            frontier.Enqueue(startingTile, 0);

            MP_Player player = character.GetComponent<MP_Player>();
            movement = player.Movement;

            int fastestWay = CalculateFastestPath();
            if (endTarget.CompareTag("Plus") && fastestWay != -1)
            {
                fastestWay += 1;
            }
            if (fastestWay != -1 && movement >= fastestWay || cheat.Cheat)
            {
                if (movement < fastestWay)
                {
                    player.Cheated = true;
                    player.Movement = 0;
                }
                else
                {
                    player.Movement -= fastestWay;
                }
                return true;
            }
        }
        else if (character.CompareTag("MP_Bot"))
        {
            return true;
        }
        return false;
    }

    private void ClearPathfindingData()
    {
        nextTileToGoal.Clear();
        costToReachTile.Clear();
        frontier = new PriorityQueue<GameObject>();
    }

    //Calculate the fastest way to the target
    private int CalculateFastestPath()
    {
        while (frontier.Count > 0)
        {
            GameObject currentTile = frontier.Dequeue();
            if (currentTile == targetTile)
            {
                return costToReachTile[currentTile];
            }
            foreach (GameObject neighbor in GetNeighbors(currentTile))
            {
                MP_Tile neighborTile = neighbor.GetComponent<MP_Tile>();

                int newCost = costToReachTile[currentTile] + neighborTile.MovementCost;
                if (!costToReachTile.ContainsKey(neighbor) || newCost < costToReachTile[neighbor])
                {
                    costToReachTile[neighbor] = newCost;
                    nextTileToGoal[neighbor] = currentTile;
                    frontier.Enqueue(neighbor, newCost);
                }
            }
        }
        return -1;
    }

    //Provides the neighbouring tiles
    private List<GameObject> GetNeighbors(GameObject tileObject)
    {
        List<GameObject> neighbors = new List<GameObject>();
        MP_Tile tile = tileObject.GetComponent<MP_Tile>();

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
    private bool CanMoveFromTile(MP_Tile tile, (int x, int y) koordinate)
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

    //Return the targeted tile
    private GameObject GetTargetTile(GameObject targetObject)
    {
        if (targetObject.CompareTag("Tile"))
        {
            return targetObject;
        }
        if (targetObject.CompareTag("Plus"))
        {
            return GetAdjacentTileToPlus(targetObject);
        }
        return null;
    }

    //Provides the neighbouring tiles
    private GameObject GetAdjacentTileToPlus(GameObject plus)
    {
        Vector3 plusPosition = plus.transform.position;
        foreach ((int x, int y) koordinate in directions.koordinates)
        {
            Vector3 checkPosition = new Vector3(plusPosition.x + koordinate.x, plusPosition.y + koordinate.y, 0);
            if (!gameBoard.onPosition(checkPosition))
            {
                continue;
            }

            GameObject obj = gameBoard.GetGameObject(checkPosition);
            if (!obj.CompareTag("Tile"))
            {
                continue;
            }
            return obj;
        }
        return null;
    }
    #endregion
}