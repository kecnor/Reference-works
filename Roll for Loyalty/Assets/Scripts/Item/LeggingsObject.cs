using UnityEngine;

[CreateAssetMenu(fileName = "New Leggings Objetc", menuName = "Inventory System/Items/Leggings")]
public class LeggingsObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Leggings;
    }
}