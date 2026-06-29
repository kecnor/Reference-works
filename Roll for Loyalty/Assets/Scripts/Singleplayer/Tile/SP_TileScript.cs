using System;
using UnityEngine;

public class SP_TileScript : MonoBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private SpawnGameObject spawnObject;
    [SerializeField] private Directions directions;
    [SerializeField] private SP_MoveCharacter moveCharacter;
    [SerializeField] private WriteLog writeLog;

    [SerializeField] private GameObject gameField;
    //Prefabs
    [SerializeField] private GameObject[] tiles;

    SP_Tile neighbourTile;
    Vector3 position;
    #endregion
    #region Constructor
    public void CreateTile(Vector3 position)
    {
        this.position = position;
        TileTypes tileType = RandomTileType();
        SP_Tile newTile = GetTileByType(tileType);
        newTile.SetTile(position, tileType);
        newTile.ActivateTile();
        CreateDoors(newTile);
        ShowTile(newTile.gameObject);
        gameBoard.addBoardPiece(newTile.Position, newTile.gameObject);

        writeLog.WriteNewLog($"A new {newTile.TileType} discovered at ({newTile.transform.position.x}, {newTile.transform.position.y})");
    }

    public void CreateTile(Vector3 position, TileTypes tileType, bool[] openDoors)
    {
        this.position = position;;
        SP_Tile newTile = GetTileByType(tileType);
        newTile.SetTile(position, tileType, openDoors);
        ShowTile(newTile.gameObject);
        gameBoard.addBoardPiece(newTile.Position, newTile.gameObject);
        writeLog.WriteNewLog($"A new {newTile.TileType} discovered at ({newTile.transform.position.x}, {newTile.transform.position.y})");
    }
    #endregion
    #region Functions
    //Activate the appropriate child
    private SP_Tile GetTileByType(TileTypes tileType)
    {
        GameObject newTileObject;
        switch (tileType)
        {
            case TileTypes.Hallway:
                newTileObject = spawnObject.SpawnNewObject(tiles[0], position, "Tile", gameField);
                SP_HallwayTile hallwayTile = newTileObject.GetComponent<SP_HallwayTile>();
                return hallwayTile;
            case TileTypes.EmptyRoom:
                newTileObject = spawnObject.SpawnNewObject(tiles[1], position, "Tile", gameField);
                SP_EmptyTile emptyTile = newTileObject.GetComponent<SP_EmptyTile>();
                return emptyTile;
            case TileTypes.EventRoom:
                newTileObject = spawnObject.SpawnNewObject(tiles[2], position, "Tile", gameField);
                SP_EventTile eventTile = newTileObject.GetComponent<SP_EventTile>();
                return eventTile;
            case TileTypes.TreasureRoom:
                newTileObject = spawnObject.SpawnNewObject(tiles[3], position, "Tile", gameField);
                SP_TreasureTile treasureTile = newTileObject.GetComponent<SP_TreasureTile>();
                return treasureTile;
            case TileTypes.MonsterRoom:
                newTileObject = spawnObject.SpawnNewObject(tiles[4], position, "Tile", gameField);
                SP_MonsterTile monsterTile = newTileObject.GetComponent<SP_MonsterTile>();
                return monsterTile;
        }
        return null;
    }

    //Creating doors for the tile
    private void CreateDoors(SP_Tile tile)
    {
        foreach ((int x, int y) koordinate in directions.koordinates)
        {
            neighbourTile = null;
            Vector3 newPosition = new Vector3(position.x + koordinate.x, position.y + koordinate.y, 0);
            neighbourTile = Neighbour(newPosition);

            switch (koordinate)
            {
                case (0, 2):
                    tile.NorthDoor = CreateDoor("southDoor");
                    break;
                case (2, 0):
                    tile.EastDoor = CreateDoor("westDoor");
                    break;
                case (0, -2):
                    tile.SouthDoor = CreateDoor("northDoor");
                    break;
                case (-2, 0):
                    tile.WestDoor = CreateDoor("eastDoor");
                    break;
            }
        }
    }

    //Retuns the tile's neighboud on position, if it has
    private SP_Tile Neighbour(Vector3 newPosition)
    {
        if (gameBoard.onPosition(newPosition))
        {
            if (gameBoard.GetGameObject(newPosition).CompareTag("Tile"))
            {
                return gameBoard.GetGameObject(newPosition).GetComponent<SP_Tile>();
            }
        }
        return null;
    }


    private bool CreateDoor(string neighbourDoorname)
    {
        if (neighbourTile == null)
        {
            int rnd = UnityEngine.Random.Range(0, 100);
            return rnd < 60;
        }
        else
        {
            return neighbourTile.GetDoorOpen(neighbourDoorname);
        }
    }

    //Choosing a tileType
    private TileTypes RandomTileType()
    {
        return (TileTypes)Enum.ToObject(typeof(TileTypes), UnityEngine.Random.Range(0, 5));
    }

    //Creating visual for the tile
    private void ShowTile(GameObject tile)
    {
        foreach (Transform child in tile.transform)
        {
            child.GetComponent<VisualiseTilePieces>().SpawnPieces();
        }
    }

    //Clicked ont the give tile
    public void TileClicked(Vector3 position)
    {
        if (moveCharacter.CanMoveCharacter(position))
        {
        }
    }
    #endregion
}