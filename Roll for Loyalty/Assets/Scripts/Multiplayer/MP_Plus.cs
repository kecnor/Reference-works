using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class MP_Plus : NetworkBehaviour
{
    #region Variables
    //Scripts
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private SpawnNetworkObject spawnObject;
    [SerializeField] private Directions directions;
    [SerializeField] private MP_TileScript tileScript;
    [SerializeField] private MP_MoveCharacter moveCharacter;
    [SerializeField] private MP_Characters characters;
    //Scenes
    [SerializeField] private GameObject gameField;
    //GameObjects & Prefabs
    [SerializeField] private NetworkObject plus;
    #endregion
    #region Functions

    //Destroying the plus insertObject and spawn tile gameObject in it's place
    public void PlusClicked(GameObject clickedPlus)
    {
        Vector3 position = clickedPlus.transform.position;
        if (moveCharacter.CanMoveCharacter(position))
        {
            MoveCharacterRpc(position, characters.GetActiveCharacter());
            gameBoard.removeBoardPiece(position);
            tileScript.CreateTile(position);
            CreateDirections(position);
            clickedPlus.GetComponent<NetworkObject>().Despawn();
            ZeroOutCharacterMovement(position);
            if (characters.GetActiveCharacter().CompareTag("MP_Player"))
            {
                characters.GetActiveCharacter().GetComponent<MP_Player>().UpdateStatsVisualClientRpc();
            }
        }
    }

    //Moving the character on the server
    [Rpc(SendTo.ClientsAndHost)]
    private void MoveCharacterRpc(Vector3 position, NetworkObjectReference characterRef)
    {
        if (characterRef.TryGet(out NetworkObject character))
        {
            character.transform.position = position;
        }
    }

    //Creating the tile's neighbooring pluses
    public void CreateDirections(Vector3 position)
    {
       foreach ((int x, int y) koordinate in directions.koordinates)
        {
            Vector3 newPosition = new Vector3(position.x + koordinate.x, position.y + koordinate.y, 0);
            if (!gameBoard.onPosition(newPosition))
            {
                switch (koordinate)
                {
                    case (0, 2):
                        if (gameBoard.GetGameObject(position).GetComponent<MP_Tile>().NorthDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (2, 0):
                        if (gameBoard.GetGameObject(position).GetComponent<MP_Tile>().EastDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (0, -2):
                        if (gameBoard.GetGameObject(position).GetComponent<MP_Tile>().SouthDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (-2, 0):
                        if (gameBoard.GetGameObject(position).GetComponent<MP_Tile>().WestDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                }
            }
        }
    }

    private void CreatePlus(Vector3 position)
    {
        GameObject newPlus = spawnObject.SpawnNewObject(plus, position, "Plus", gameField.GetComponent<NetworkObject>());
        newPlus.transform.position = position;
        newPlus.GetComponent<NetworkObject>().Spawn();
        gameBoard.addBoardPiece(position, newPlus);
    }

    //Depending on the tile's type, reset the character movment
    private void ZeroOutCharacterMovement(Vector3 position)
    {
        MP_Tile tile = gameBoard.GetGameObject(position).GetComponent<MP_Tile>();
        if (tile.TileType != TileTypes.Hallway && tile.TileType != TileTypes.EmptyRoom)
        {
            GameObject character = characters.GetActiveCharacter();
            if (character.CompareTag("MP_Player"))
            {
                character.GetComponent<MP_Player>().Movement = 0;
            }
            else if (character.CompareTag("MP_Bot"))
            {
                character.GetComponent<MP_Bot>().Movement = 0;
            }
        }
    }
    #endregion
}