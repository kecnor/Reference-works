using UnityEngine;

public class SpawnGameObject : MonoBehaviour
{
    #region Variable
    [SerializeField] private ToggleVisibility toggleVisibility;
    #endregion

    //Creating gameObject in the game
    public GameObject SpawnNewObject(GameObject newObject, Vector3 position, string name)
    {
        GameObject spawningObjetc = CreateObject(newObject, position, name);
        return spawningObjetc;
    }

    //Setting the gameObject parent
    public GameObject SpawnNewObject(GameObject newObject, Vector3 position, string name, GameObject parent)
    {
        GameObject spawningObjetc = CreateObject(newObject, position, name);
        spawningObjetc.transform.SetParent(parent.transform);
        return spawningObjetc;
    }

    //Turn visible the gameObject
    private GameObject CreateObject(GameObject newObject, Vector3 position, string name)
    {
        GameObject spawningObjetc = Instantiate(newObject, position, Quaternion.identity);
        spawningObjetc.name = name;
        toggleVisibility.AppearObject(spawningObjetc);
        return spawningObjetc;
    }
}