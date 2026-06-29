using UnityEngine;

[CreateAssetMenu(fileName = "New MainHandWeapon Objetc", menuName = "Inventory System/Items/MainHandWeapon")]
public class MainHandWeaponObject : ItemObject
{
    public void Awake()
    {
        data.type = ItemType.MainHandWeapon;
    }
}