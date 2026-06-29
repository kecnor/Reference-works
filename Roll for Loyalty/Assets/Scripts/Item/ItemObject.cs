using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory System/Item")]
public abstract class ItemObject : ScriptableObject
{
    #region Variables
    public Sprite display;
    public bool stackable;
    [TextArea(15, 20)] public string description;
    public Item data = new Item();
    #endregion
    #region Constructor
    public Item CreateItem()
    {
        Item newItem = new Item(this);
        return newItem;
    }
    #endregion
}