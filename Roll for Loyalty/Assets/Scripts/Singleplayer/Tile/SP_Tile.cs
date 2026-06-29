using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class SP_Tile: MonoBehaviour, IPointerDownHandler
{
    #region Variables
    private Dictionary<string, bool> doors;
    private Vector3 postion;
    private TileTypes tileType;
    private int movementCost;

    //Getters & Setter
    public bool NorthDoor { get { return doors["northDoor"]; } set { doors["northDoor"] = value; } }
    public bool EastDoor { get { return doors["eastDoor"]; } set { doors["eastDoor"] = value; } }
    public bool SouthDoor { get { return doors["southDoor"]; } set { doors["southDoor"] = value; } }
    public bool WestDoor { get { return doors["westDoor"]; } set { doors["westDoor"] = value; } }
    public Vector3 Position { get { return postion; } }
    public TileTypes TileType { get { return tileType; } }
    public int MovementCost { get { return movementCost; } set { movementCost = value; } }
    #endregion
    #region Constructor
    public void SetTile(Vector3 postion, TileTypes tileType)
    {
        doors = new Dictionary<string, bool>();
        doors.Add("northDoor", false);
        doors.Add("eastDoor", false);
        doors.Add("southDoor", false);
        doors.Add("westDoor", false);
        this.postion = postion;
        this.tileType = tileType;
        if (this.tileType != TileTypes.Hallway)
        {
            movementCost = 2;
        }
        else
        {
            movementCost = 1;
        }
    }

    public void SetTile(Vector3 postion, TileTypes tileType, bool[] openDoors)
    {
        doors = new Dictionary<string, bool>();
        doors.Add("northDoor", openDoors[0]);
        doors.Add("eastDoor", openDoors[1]);
        doors.Add("southDoor", openDoors[2]);
        doors.Add("westDoor", openDoors[3]);
        this.postion = postion;
        this.tileType = tileType;
        if (this.tileType != TileTypes.Hallway)
        {
            movementCost = 1;
        }
        else
        {
            movementCost = 0;
        }
    }
    #endregion
    #region Functions
    public abstract void ActivateTile();

    //Returns the door open or not
    public bool GetDoorOpen(string doorname)
    {
        return doors[doorname];
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject obj = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerTileScript");
        SP_TileScript tileScript = obj.GetComponent<SP_TileScript>();
        tileScript.TileClicked(postion);
    }
    #endregion
}