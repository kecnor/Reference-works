using UnityEngine;

[CreateAssetMenu(fileName = "New Default Objetc", menuName = "Inventory System/Items/Default")]
public class DefaultObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Default;
    }
}