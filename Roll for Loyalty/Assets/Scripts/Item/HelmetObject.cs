using UnityEngine;

[CreateAssetMenu(fileName = "New Helmet Objetc", menuName = "Inventory System/Items/Helmet")]
public class HelmetObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.Helmet;
    }
}