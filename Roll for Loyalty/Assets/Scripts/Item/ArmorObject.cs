using UnityEngine;

[CreateAssetMenu(fileName = "New Armor Objetc", menuName = "Inventory System/Items/Armor")]
public class ArmorObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Armor;
    }
}