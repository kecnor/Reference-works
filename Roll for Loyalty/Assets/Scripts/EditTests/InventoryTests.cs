using NUnit.Framework;
using System;

public class InventoryTests
{
    private Item MakeItem(int id, ItemType type = ItemType.MainHandWeapon)
    {
        return new Item { name = "TestSword", id = id, type = type, buffs = new ItemBuff[0] };
    }

    [Test]
    public void InventorySlot_Constructor_CreatesEmptySlotTest()
    {
        var slot = new InventorySlot();
        Assert.AreEqual(-1, slot.Item.ID);
        Assert.AreEqual(0, slot.Amount);
    }

    [Test]
    public void InventorySlot_Constructor_SetsItemAndAmountTest()
    {
        var slot = new InventorySlot(MakeItem(1), 5);
        Assert.AreEqual(1, slot.Item.ID);
        Assert.AreEqual(5, slot.Amount);
    }

    [Test]
    public void InventorySlot_UpdateSlotTest()
    {
        var slot = new InventorySlot();
        slot.UpdateSlot(MakeItem(1), 5);
        Assert.AreEqual(1, slot.Item.ID);
        Assert.AreEqual(5, slot.Amount);
    }

    [Test]
    public void InventorySlot_RemoveItemTest()
    {
        var slot = new InventorySlot(MakeItem(1), 5);
        slot.RemoveItem();
        Assert.AreEqual(-1, slot.Item.ID);
        Assert.AreEqual(0, slot.Amount);
    }

    [Test]
    public void InventorySlot_AddAmountTest()
    {
        var slot = new InventorySlot(MakeItem(1), 5);
        slot.AddAmount(10);
        Assert.AreEqual(15, slot.Amount);
    }

    [Test]
    public void InventorySlot_AddAmount_NegativeValueTest()
    {
        var slot = new InventorySlot(MakeItem(1), 10);
        slot.AddAmount(-5);
        Assert.AreEqual(5, slot.Amount);
    }

    [Test]
    public void InventorySlot_CanPlaceInSlot_NullItemTest()
    {
        var slot = new InventorySlot();
        slot.AllowedItems = new[] { ItemType.MainHandWeapon };
        Assert.IsTrue(slot.CanPlaceInSlot(null));
    }

    [Test]
    public void InventorySlot_CanPlaceInSlot_EmptyAllowedItemsTest()
    {
        var slot = new InventorySlot();
        Assert.AreEqual(0, slot.AllowedItems.Length);
        Assert.IsTrue(slot.CanPlaceInSlot(null));
    }

    [Test]
    public void InventorySlot_UpdateSlot_OverwritesItemTest()
    {
        var slot = new InventorySlot(MakeItem(1), 5);
        slot.UpdateSlot(MakeItem(2), 10);
        Assert.AreEqual(2, slot.Item.ID);
        Assert.AreEqual(10, slot.Amount);
    }

    [Test]
    public void InventorySlot_RemoveItem_ThenUpdateSlotTest()
    {
        var slot = new InventorySlot(MakeItem(1), 5);
        slot.RemoveItem();
        slot.UpdateSlot(MakeItem(2), 10);
        Assert.AreEqual(2, slot.Item.ID);
        Assert.AreEqual(10, slot.Amount);
    }

    [Test]
    public void Inventory_Constructor_CreatesCorrectNumberOfSlotsTest()
    {
        var inventory = new Inventory(5);
        Assert.AreEqual(5, inventory.Slots.Length);
    }

    [Test]
    public void Inventory_Constructor_AllSlotsAreEmptyTest()
    {
        var inv = new Inventory(5);
        foreach (var slot in inv.Slots)
        { 
            Assert.AreEqual(-1, slot.Item.ID);
        }
    }

    [Test]
    public void Inventory_ClearTest()
    {
        var inv = new Inventory(3);
        inv.Slots[0].UpdateSlot(MakeItem(1), 1);
        inv.Slots[1].UpdateSlot(MakeItem(2), 1);

        inv.Clear();
        foreach (var slot in inv.Slots)
        { 
            Assert.AreEqual(-1, slot.Item.ID);
        }
    }

    [Test]
    public void Inventory_Clear_OnAlreadyEmptyInventory_DoesNotThrowTest()
    {
        var inv = new Inventory(5);
        Assert.DoesNotThrow(() => inv.Clear());
    }

    [Test]
    public void Inventory_ZeroSize_CreatesEmptySlotArrayTest()
    {
        var inv = new Inventory(0);
        Assert.AreEqual(0, inv.Slots.Length);
    }

    [Test]
    public void Inventory_SlotsAreIndependentTest()
    {
        var inv = new Inventory(5);
        inv.Slots[0].UpdateSlot(MakeItem(1), 5);

        Assert.AreEqual(1, inv.Slots[0].Item.ID);
        Assert.AreEqual(-1, inv.Slots[1].Item.ID);
        Assert.AreEqual(-1, inv.Slots[2].Item.ID);
    }
}