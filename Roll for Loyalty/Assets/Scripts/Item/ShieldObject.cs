using UnityEngine;

[CreateAssetMenu(fileName = "New Shield Objetc", menuName = "Inventory System/Items/Shield")]
public class ShieldObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Shield;
    }
}