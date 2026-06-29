using UnityEngine;

[CreateAssetMenu(fileName = "New Accessories Objetc", menuName = "Inventory System/Items/Accessories")]
public class AccessoriesObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Accessories;
    }
}