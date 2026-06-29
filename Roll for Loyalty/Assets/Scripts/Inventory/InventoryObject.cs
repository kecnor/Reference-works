using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory", menuName = "Inventory System/Inventory")]
public class InventoryObject : ScriptableObject
{
    #region Variables
    public ItemDataBaseObject database;
    public Inventory Container;
    public InterfaceType type;
    public string savePath;

    //Getters
    public InventorySlot[] GetSlots { get{ return Container.Slots; } }
    public int EmptySlotCount
    {
        get
        {
            int counter = 0;
            foreach (InventorySlot slot in GetSlots)
            {
                if (slot.Item.ID < 0)
                {
                    counter++;
                }
            }
            return counter;
        }
    }
    #endregion
    #region Functions
    //Checking if the item is Stackable and if is exist in the inventory, then increase the amount or add the item to the inventory 
    public bool AddItem(Item item, int amount)
    {
        if (EmptySlotCount <= 0)
        {
            return false;
        }

        InventorySlot slot = FindItemOnInventory(item);
        if (!database.GetItem(item.ID).stackable || slot == null)
        {
            SetFirstEmptySlot(item, amount);
        }
        else 
        {
            slot.AddAmount(amount);
        }
        return true;
    }

    //Search for the first Empty slot in the inventory then add the item to that slot
    public InventorySlot SetFirstEmptySlot(Item item, int amount)
    {
        foreach (InventorySlot currentItem in GetSlots)
        {
            if (currentItem.Item.ID <= -1)
            {
                currentItem.UpdateSlot(item, amount);
                return currentItem;
            }
        }
        return null;
    }

    //Swap the tow given slots items even if one of the item is an placeholder item
    public void SwapItems(InventorySlot item1, InventorySlot item2)
    {
        if (item1.Item.ID != -1 || item2.Item.ID != -1)
        {
            if (item2.CanPlaceInSlot(item1.ItemObject) && item1.CanPlaceInSlot(item2.ItemObject))
            {
                InventorySlot temp = new InventorySlot(item2.Item, item2.Amount);
                item2.UpdateSlot(item1.Item, item1.Amount);
                item1.UpdateSlot(temp.Item, temp.Amount);
            }
        }
    }

    //Repleace the item with an empty item in the item's slot
    public void RemoveItem(Item item)
    {
        foreach (InventorySlot currentItem in GetSlots)
        {
            if (currentItem.Item == item)
            {
                currentItem.UpdateSlot(new Item(), 0);
            }
        }
    }

    //Use the slot's item if usable
    public bool UseItem(InventorySlot slot, out ItemBuff[] buffs)
    {
        buffs = null;
        if (slot.Item.type == ItemType.Potion || slot.Item.type == ItemType.Debuff)
        {
            buffs = slot.Item.buffs;
            if (slot.Amount == 1)
            {
                slot.RemoveItem();
            }
            else
            { 
                slot.AddAmount(-1);
            }
            return true;
        }
        return false;
    }

    //Search for the given item's slot in the inventory
    private InventorySlot FindItemOnInventory(Item item)
    {
        foreach (InventorySlot slot in GetSlots)
        {
            if (slot.Item.ID == item.ID)
            {
                return slot;
            }
        }
        return null;
    }
    #endregion
}