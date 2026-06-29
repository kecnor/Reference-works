using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public abstract class MP_Tile : NetworkBehaviour, IPointerDownHandler
{
    #region Variables
    private NetworkVariable<bool> northDoor = new();
    private NetworkVariable<bool> eastDoor = new();
    private NetworkVariable<bool> southDoor = new();
    private NetworkVariable<bool> westDoor = new();
    private NetworkVariable<Vector3> postion = new NetworkVariable<Vector3>();
    private NetworkVariable<TileTypes> tileType = new();
    private NetworkVariable<int> movementCost = new NetworkVariable<int>(1);

    //Getters & Setter
    public bool NorthDoor { get { return northDoor.Value; } set { northDoor.Value = value; } }
    public bool EastDoor { get { return eastDoor.Value; } set { eastDoor.Value = value; } }
    public bool SouthDoor { get { return southDoor.Value; } set { southDoor.Value = value; } }
    public bool WestDoor { get { return westDoor.Value; } set { westDoor.Value = value; } }
    public Vector3 Position { get { return postion.Value; } }
    public TileTypes TileType { get { return tileType.Value; } }
    public int MovementCost { get { return movementCost.Value; } set { movementCost.Value = value; } }
    #endregion
    #region Constructor
    public void SetTile(Vector3 position, TileTypes tileType)
    {
        if (!IsServer) return;

        transform.position = position;
        this.tileType.Value = tileType;

        northDoor.Value = false;
        eastDoor.Value = false;
        southDoor.Value = false;
        westDoor.Value = false;

        movementCost.Value = 1;
    }

    public void SetTile(Vector3 position, TileTypes tileType, bool[] openDoors)
    {
        if (!IsServer) return;

        northDoor.Value = openDoors[0];
        eastDoor.Value = openDoors[1];
        southDoor.Value = openDoors[2];
        westDoor.Value = openDoors[3];

        transform.position = position;
        this.tileType.Value = tileType;
        if (this.tileType.Value != TileTypes.Hallway)
        {
            movementCost.Value = 1;
        }
        else
        {
            movementCost.Value = 0;
        }
    }
    public override void OnNetworkSpawn()
    {
        northDoor.OnValueChanged += OnDoorChanged;
        eastDoor.OnValueChanged += OnDoorChanged;
        southDoor.OnValueChanged += OnDoorChanged;
        westDoor.OnValueChanged += OnDoorChanged;

        RefreshVisuals();
    }

    public override void OnNetworkDespawn()
    {
        northDoor.OnValueChanged -= OnDoorChanged;
        eastDoor.OnValueChanged -= OnDoorChanged;
        southDoor.OnValueChanged -= OnDoorChanged;
        westDoor.OnValueChanged -= OnDoorChanged;
    }
    #endregion
    #region Functions
    public abstract void ActivateTile();

    private void OnDoorChanged(bool oldValue, bool newValue)
    {
        RefreshVisuals();
    }

    private void RefreshVisuals()
    {
        foreach (Transform child in transform)
        {
            MP_VisualiseTilePieces visual = child.GetComponent<MP_VisualiseTilePieces>();

            if (visual != null)
            {
                visual.Init(this);
            }
        }
    }

    //Returns the door open or not
    public bool GetDoorOpen(string doorname)
    {
        return doorname switch
        {
            "northDoor" => northDoor.Value,
            "eastDoor" => eastDoor.Value,
            "southDoor" => southDoor.Value,
            "westDoor" => westDoor.Value,
            _ => false
        };
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        CallTileClickedRpc();
    }

    [Rpc(SendTo.Server)]
    private void CallTileClickedRpc()
    {
        GameObject obj = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerTileScript");
        MP_TileScript tileScript = obj.GetComponent<MP_TileScript>();
        tileScript.TileClicked(transform.position);
    }
    #endregion
}