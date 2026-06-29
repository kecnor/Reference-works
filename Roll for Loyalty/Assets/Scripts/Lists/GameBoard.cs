using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    #region Variables
    private Dictionary<Vector3, GameObject> gameBoard;

    //Getter
    public Dictionary<Vector3, GameObject> GameBoardPieces { get { return gameBoard; } }
    #endregion
    #region Constructor
    void Awake()
    {
        gameBoard = new Dictionary<Vector3, GameObject>();
    }
    #endregion
    #region Functions

    //Add & remove spawned plus or tile to the list
    public void addBoardPiece(Vector3 position, GameObject piece)
    {
        gameBoard.Add(position, piece);
    }

    public void removeBoardPiece(Vector3 position)
    {
        gameBoard.Remove(position);
    }

    //Returns if there is a board piece on the given position
    public bool onPosition(Vector3 position)
    {
        return gameBoard.ContainsKey(position);
    }

    //Returns the board piece on the given position
    public GameObject GetGameObject(Vector3 position)
    {
        if (onPosition(position))
        { 
            return gameBoard[position];
        }
        return null;
    }

    //Returns a random tile's position
    public Vector3 GetRandomTilePosition()
    {
        List<Vector3> positions = new List<Vector3>(gameBoard.Keys);
        Vector3 randomTilePosition = positions[Random.Range(0, positions.Count)];
        while(gameBoard[randomTilePosition].CompareTag("Plus"))
        {
            randomTilePosition = positions[Random.Range(0, positions.Count)];
        }
        return randomTilePosition;
    }
    #endregion
}