using Unity.Netcode;
using UnityEngine;

public class SpawnNetworkObject : NetworkBehaviour
{
    #region Variable
    [SerializeField] private ToggleVisibility toggleVisibility;
    #endregion

    //Creating gameObject in the game
    public GameObject SpawnNewObject(NetworkObject newObject, Vector3 position, string name)
    {
        GameObject spawningObjetc = CreateObject(newObject, position, name);
        return spawningObjetc;
    }

    //Setting the gameObject parent
    public GameObject SpawnNewObject(NetworkObject newObject, Vector3 position, string name, NetworkObject parent)
    {
        GameObject spawningObjetc = CreateObject(newObject, position, name);
        spawningObjetc.GetComponent<NetworkObject>().TrySetParent(parent, true);
        return spawningObjetc;
    }

    //Turn visible the gameObject
    private GameObject CreateObject(NetworkObject newObject, Vector3 position, string name)
    {
        GameObject spawningObjetc = Instantiate(newObject.gameObject, position, Quaternion.identity);
        spawningObjetc.gameObject.name = name;
        toggleVisibility.AppearObject(spawningObjetc);
        return spawningObjetc;
    }
}