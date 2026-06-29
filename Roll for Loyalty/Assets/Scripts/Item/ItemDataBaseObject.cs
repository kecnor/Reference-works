using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Item Database", menuName = "Inventory System/Items/Database")]
public class ItemDataBaseObject : ScriptableObject
{
    #region Variables
    public ItemObject[] itemObjects;
    private Dictionary<int, ItemObject> idWithItems = new Dictionary<int, ItemObject>();
    #endregion
    #region Constructor
    private void OnEnable()
    {
        BuildDatabase();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildDatabase();
    }
#endif
    #endregion
    #region Functions
    // Assign item IDs for both gameplay and the Editor
    public void BuildDatabase()
    {
        if (itemObjects != null)
        {
            idWithItems.Clear();
            for (int i = 0; i < itemObjects.Length; i++)
            {
                itemObjects[i].data.id = i;
                idWithItems[i] = itemObjects[i];
            }
        }
    }

    //Find item by it's id
    public ItemObject GetItem(int id)
    {
        return idWithItems[id];
    }

    //Get a random item from the items list
    public ItemObject GetRandomItem()
    {
        return itemObjects[Random.Range(1, itemObjects.Length)];
    }
    #endregion
}