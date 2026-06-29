using System;

[Serializable]
public class Inventory
{
    #region Variables
    public InventorySlot[] slots;
    //Getter
    public InventorySlot[] Slots { get { return slots; } }
    #endregion
    #region Constructor
    public Inventory(int size)
    {
        slots = new InventorySlot[size];
        for (int i = 0; i < size; i++)
        {
            slots[i] = new InventorySlot();
        }
    }
    #endregion
    #region Functions
    //Clear the Inventory slots
    public void Clear()
    {
        foreach (InventorySlot slot in slots)
        {
            slot.RemoveItem();
        }
    }
    #endregion
}