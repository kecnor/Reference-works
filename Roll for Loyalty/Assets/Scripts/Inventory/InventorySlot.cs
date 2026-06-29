using System;
using UnityEngine;

[Serializable]
public class InventorySlot
{
    #region Variables
    [NonSerialized] public InventoryObject parentInventory;
    [NonSerialized] public GameObject slotDisplay;
    [NonSerialized] public SlotUpdated OnBeforeUpdate;
    [NonSerialized] public SlotUpdated OnAfterUpdate;
    [NonSerialized] public SP_Cheating SP_cheating;
    private Item item;
    private int amount;
    public ItemType[] AllowedItems = new ItemType[0];

    //Getters & Setters
    public Item Item { get { return item; } set { item = value; } }
    public int Amount { get { return amount; } }
    public ItemObject ItemObject 
    {
        get
        {
            if (item.ID >= 0)
            {
                return parentInventory.database.GetItem(item.ID);
            }
            return null;
        }
    }
    #endregion
    #region Constructors
    public InventorySlot()
    {
        UpdateSlot(new Item(), 0);
    }

    public InventorySlot(Item item, int amount)
    {
        UpdateSlot(item, amount);
    }
    #endregion
    #region Functions
    //Updateing the given slot
    public void UpdateSlot(Item item, int amount)
    {
        if (OnBeforeUpdate != null)
        {
            OnBeforeUpdate.Invoke(this);
        }

        this.item = item;
        this.amount = amount;

        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    public void LoadSlot(Item item, int amount)
    {
        this.item = item;
        this.amount = amount;

        if (OnAfterUpdate != null)
        {
            OnAfterUpdate.Invoke(this);
        }
    }

    //Changeing the current item with a placeholder empty item
    public void RemoveItem()
    {
        UpdateSlot(new Item(), 0);
    }

    //Change an existing stackable item's amount variable 
    public void AddAmount(int value)
    {
        UpdateSlot(item, amount += value);
    }

    //Check if the item can be placed on the given slot
    public bool CanPlaceInSlot(ItemObject itemObjetc)
    {
        SP_cheating = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCheat").GetComponent<SP_Cheating>();
        if (SP_cheating.Cheat)
        {
            return true;
        }
        if (AllowedItems.Length <= 0 || itemObjetc == null || itemObjetc.data.ID < 0)
        {
            return true;
        }
        foreach (ItemType allowedType in AllowedItems)
        {
            if (itemObjetc.data.type == allowedType)
            {
                return true;
            }
        }
        return false;
    }
#endregion
}

public delegate void SlotUpdated(InventorySlot slot);