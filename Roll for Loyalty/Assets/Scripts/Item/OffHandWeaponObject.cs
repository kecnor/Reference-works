using UnityEngine;

[CreateAssetMenu(fileName = "New OffHandWeapon Objetc", menuName = "Inventory System/Items/OffHandWeapon")]
public class OffHandWeaponObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.OffHandWeapon;
    }
}