using UnityEngine;

public class SP_Plus : MonoBehaviour
{
    #region Variables
    //Scripts
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private SpawnGameObject spawnObject;
    [SerializeField] private Directions directions;
    [SerializeField] private SP_TileScript tileScript;
    [SerializeField] private SP_MoveCharacter moveCharacter;
    [SerializeField] private SP_Characters characters;
    //Scenes
    [SerializeField] private GameObject gameField;
    //GameObjects & Prefabs
    [SerializeField] private GameObject plus;
    #endregion
    #region Functions

    //Destroying the plus gameObject and spawn tile gameObject in it's place
    public void PlusClicked(GameObject clickedPlus)
    {
        Vector3 position = clickedPlus.transform.position;
        if (moveCharacter.CanMoveCharacter(position))
        { 
            gameBoard.removeBoardPiece(position);
            tileScript.CreateTile(position);
            CreateDirections(position);
            Destroy(clickedPlus);
            NullCharacterMovement(position);
            if (characters.GetActiveCharacter().CompareTag("SP_Player"))
            {
                characters.GetActiveCharacter().GetComponent<SP_Player>().UpdateStatsVisual();
            }
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
                        if (gameBoard.GetGameObject(position).GetComponent<SP_Tile>().NorthDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (2, 0):
                        if (gameBoard.GetGameObject(position).GetComponent<SP_Tile>().EastDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (0, -2):
                        if (gameBoard.GetGameObject(position).GetComponent<SP_Tile>().SouthDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                    case (-2, 0):
                        if (gameBoard.GetGameObject(position).GetComponent<SP_Tile>().WestDoor)
                        {
                            CreatePlus(newPosition);
                        }
                        break;
                }
            }
        }
    }

    public void CreatePlus(Vector3 position)
    {
        GameObject newPlus = spawnObject.SpawnNewObject(plus, position, "Plus", gameField);
        newPlus.transform.position = position;
        gameBoard.addBoardPiece(position, newPlus);
    }

    //Depending on the tile's type, reset the character movment
    private void NullCharacterMovement(Vector3 position)
    {
        SP_Tile tile = gameBoard.GetGameObject(position).GetComponent<SP_Tile>();
        if (tile.TileType != TileTypes.Hallway && tile.TileType != TileTypes.EmptyRoom)
        {
            GameObject character = characters.GetActiveCharacter();
            if (character.CompareTag("SP_Player"))
            {
                character.GetComponent<SP_Player>().Movement = 0;
            }
            else if (character.CompareTag("SP_Bot"))
            {
                character.GetComponent<SP_Bot>().Movement = 0;
            }
        }
    }
    #endregion
}