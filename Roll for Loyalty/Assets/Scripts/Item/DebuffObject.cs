using UnityEngine;

[CreateAssetMenu(fileName = "New Debuff Objetc", menuName = "Inventory System/Items/Debuff")]
public class DebuffObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Debuff;
    }
}