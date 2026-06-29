using UnityEngine;

[CreateAssetMenu(fileName = "New Potion Objetc", menuName = "Inventory System/Items/Potion")]
public class PotionObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Potion;
    }
}