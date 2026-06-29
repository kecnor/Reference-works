using Unity.Netcode;
using UnityEngine;

public class MP_VisualiseTilePieces : NetworkBehaviour
{
    #region Variables
    private static Sprite wall;
    private static Sprite[] floors = new Sprite[4];
    private MP_Tile tile;
    private static bool loaded = false;
    #endregion
    #region Functions
    //Creating the visuals for the tile
    public void Init(MP_Tile tile)
    {
        this.tile = tile;
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        LoadResources();
        if (tile.TileType.Equals(TileTypes.Hallway))
        {
            SpawnHallwaySprite();
        }
        else
        {
            SpawnTileSprite();
        }
    }

    //Getting the sprites
    private void LoadResources()
    {
        if (loaded) return;

        wall = Resources.Load<Sprite>("Wall");

        for (int i = 0; i < 4; i++)
        {
            floors[i] = Resources.Load<Sprite>($"Floor_0{i + 1}");
        }

        loaded = true;
    }

    //Give the tile piece's sprite
    private void SpawnTileSprite()
    {
        int x = Mathf.RoundToInt((transform.position.x - transform.parent.position.x) * 10f);
        int y = Mathf.RoundToInt((transform.position.y - transform.parent.position.y) * 10f);

        switch ((x, y))
        {
            case (-1, 9):
            case (1, 9):
                CheckDoor(tile.NorthDoor);
                break;

            case (9, 1):
            case (9, -1):
                CheckDoor(tile.EastDoor);
                break;

            case (-1, -9):
            case (1, -9):
                CheckDoor(tile.SouthDoor);
                break;

            case (-9, 1):
            case (-9, -1):
                CheckDoor(tile.WestDoor);
                break;

            default:
                if (x > -9 && x < 9 && y > -9 && y < 9)
                {
                    AttachSprite(RandomFloor());
                }
                else
                {
                    AttachSprite(wall);
                }
                break;
        }
    }

    private void SpawnHallwaySprite()
    {
        int x = Mathf.RoundToInt((transform.position.x - transform.parent.position.x) * 10f);
        int y = Mathf.RoundToInt((transform.position.y - transform.parent.position.y) * 10f);

        if (Mathf.Abs(x) == 3 && Mathf.Abs(y) == 3)
        {
            AttachSprite(wall);
        }
        if (Mathf.Abs(x) == 1 && Mathf.Abs(y) == 1)
        {
            AttachSprite(RandomFloor());
        }
        if (tile.NorthDoor)
        {
            if (Mathf.Abs(x) == 1 && y >= 3)
            {
                AttachSprite(RandomFloor());
            }
            else if (Mathf.Abs(x) == 3 && y >= 3)
            {
                AttachSprite(wall);
            }
        }
        else
        {
            if (Mathf.Abs(x) == 1 && y == 3)
            {
                AttachSprite(wall);
            }
        }
        if (tile.EastDoor)
        {
            if (x >= 3 && Mathf.Abs(y) == 1)
            {
                AttachSprite(RandomFloor());
            }
            else if (x >= 3 && Mathf.Abs(y) == 3)
            {
                AttachSprite(wall);
            }
        }
        else
        {
            if (x == 3 && Mathf.Abs(y) == 1)
            {
                AttachSprite(wall);
            }
        }
        if (tile.SouthDoor)
        {
            if (Mathf.Abs(x) == 1 && y <= -3)
            {
                AttachSprite(RandomFloor());
            }
            else if (Mathf.Abs(x) == 3 && y <= -3)
            {
                AttachSprite(wall);
            }
        }
        else
        {
            if (Mathf.Abs(x) == 1 && y == -3)
            {
                AttachSprite(wall);
            }
        }
        if (tile.WestDoor)
        {
            if (x <= -3 && Mathf.Abs(y) == 1)
            {
                AttachSprite(RandomFloor());
            }
            else if (x < -3 && Mathf.Abs(y) == 3)
            {
                AttachSprite(wall);
            }
        }
        else
        {
            if (x == -3 && Mathf.Abs(y) == 1)
            {
                AttachSprite(wall);
            }
        }
    }

    //Decide if the piece is a door
    private void CheckDoor(bool open)
    {
        if (open)
        {
            AttachSprite(RandomFloor());
        }
        else
        {
            AttachSprite(wall);
        }
    }

    //Return random floor sprite
    private Sprite RandomFloor()
    {
        return floors[Random.Range(0, floors.Length)];
    }

    //Attach the given sprite to the tile's piece
    private void AttachSprite(Sprite sprite)
    {
        GetComponent<SpriteRenderer>().sprite = sprite;
    }
    #endregion
}