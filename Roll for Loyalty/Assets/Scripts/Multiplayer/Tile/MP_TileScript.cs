
using UnityEngine;
using Unity.Netcode;

public class MP_TileScript : NetworkBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private Directions directions;
    [SerializeField] private MP_MoveCharacter moveCharacter;
    [SerializeField] private MP_Characters characters;
    [SerializeField] private WriteLog writeLog;
    //Network Object
    [SerializeField] private NetworkObject[] tiles;

    private MP_Tile neighbourTile;
    Vector3 position;
    #endregion
    #region Constructor
    public void CreateTile(Vector3 position)
    {
        if (IsServer)
        {
            this.position = position;
            TileTypes tileType = RandomTileType();
            MP_Tile newTile = GetTileByType(tileType);
            NetworkObject newTileNetworkObject = newTile.GetComponent<NetworkObject>();
            newTileNetworkObject.Spawn();
            newTile.SetTile(position, tileType);
            newTile.ActivateTile();
            CreateDoors(newTile);

            gameBoard.addBoardPiece(newTile.transform.position, newTile.gameObject);
            WriteLogRpc($"A new {newTile.TileType} discovered at ({newTile.transform.position.x}, {newTile.transform.position.y})");
        }
    }

    public void CreateTile(Vector3 position,TileTypes tileType , bool[] openDoors)
    {
        if (IsServer)
        {
            this.position = position;
            MP_Tile newTile = GetTileByType(tileType);
            NetworkObject newTileNetworkObject = newTile.GetComponent<NetworkObject>();
            newTileNetworkObject.Spawn();
            newTile.SetTile(position, tileType, openDoors);
            newTile.ActivateTile();

            gameBoard.addBoardPiece(newTile.transform.position, newTile.gameObject);
        }
    }
    #endregion
    #region Functions
    //Activate the appropriate child
    private MP_Tile GetTileByType(TileTypes tileType)
    {
        GameObject newTileObject;
        switch (tileType)
        {
            case TileTypes.Hallway:
                newTileObject  = Instantiate(tiles[0].gameObject, position, Quaternion.identity);
                newTileObject.name = "Tile";
                MP_HallwayTile hallwayTile = newTileObject.GetComponent<MP_HallwayTile>();
                return hallwayTile;
            case TileTypes.EmptyRoom:
                newTileObject = Instantiate(tiles[1].gameObject, position, Quaternion.identity);
                newTileObject.name = "Tile";
                MP_EmptyTile emptyTile = newTileObject.GetComponent<MP_EmptyTile>();
                return emptyTile;
            case TileTypes.EventRoom:
                newTileObject = Instantiate(tiles[2].gameObject, position, Quaternion.identity);
                newTileObject.name = "Tile";
                MP_EventTile eventTile = newTileObject.GetComponent<MP_EventTile>();
                return eventTile;
            case TileTypes.TreasureRoom:
                newTileObject = Instantiate(tiles[3].gameObject, position, Quaternion.identity);
                newTileObject.name = "Tile";
                MP_TreasureTile treasureTile = newTileObject.GetComponent<MP_TreasureTile>();
                return treasureTile;
            case TileTypes.MonsterRoom:
                newTileObject = Instantiate(tiles[4].gameObject, position, Quaternion.identity);
                newTileObject.name = "Tile";
                MP_MonsterTile monsterTile = newTileObject.GetComponent<MP_MonsterTile>();
                return monsterTile;
        }
        return null;
    }

    //Creating doors for the tile
    private void CreateDoors(MP_Tile tile)
    {
        Vector3 tilePosition = tile.transform.position;

        foreach ((int x, int y) koordinate in directions.koordinates)
        {
            Vector3 newPosition = new Vector3(
                tilePosition.x + koordinate.x,
                tilePosition.y + koordinate.y,
                0
            );

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
    private MP_Tile Neighbour(Vector3 newPosition)
    {
        if (gameBoard.onPosition(newPosition))
        {
            GameObject obj = gameBoard.GetGameObject(newPosition);

            if (obj != null && obj.CompareTag("Tile"))
            {
                return obj.GetComponent<MP_Tile>();
            }
        }

        return null;
    }

    private bool CreateDoor(string neighbourDoorname)
    {
        if (neighbourTile == null)
        {
            int rnd = Random.Range(0, 100);
            return rnd < 60;
        }

        return neighbourTile.GetDoorOpen(neighbourDoorname);
    }

    //Choosing a tileType
    private TileTypes RandomTileType()
    {
        return (TileTypes)Random.Range(0, 5);
    }

    //Clicked ont the give tile
    public void TileClicked(Vector3 position)
    {
        if (moveCharacter.CanMoveCharacter(position))
        {
            MoveCharacterRpc(position, characters.GetActiveCharacter());
        }
    }

    //MoveCharacter to position
    [Rpc(SendTo.ClientsAndHost)]
    private void MoveCharacterRpc(Vector3 position, NetworkObjectReference characterRef)
    {
        if (characterRef.TryGet(out NetworkObject character))
        {
            character.transform.position = position;
        }
    }

    //Write in the log for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void WriteLogRpc(string message)
    {
        writeLog.WriteNewLog(message);
    }
    #endregion
}