using System;
using NUnit.Framework;
using UnityEngine;
public class FlatModifier : IModifiers
{
    private readonly int _amount;
    public FlatModifier(int amount) => _amount = amount;
    public void AddValue(ref int baseValue) => baseValue += _amount;
}

public class DoubleModifier : IModifiers
{
    public void AddValue(ref int baseValue) => baseValue *= 2;
}

public class FakeItemObject : ItemObject
{
    public static FakeItemObject Create(string itemName, int id,
        ItemType type = ItemType.Default, ItemBuff[] buffs = null)

    {
        var obj = ScriptableObject.CreateInstance<FakeItemObject>();
        obj.name = itemName;
        obj.data.id = id;
        obj.data.type = type;
        obj.data.buffs = buffs ?? Array.Empty<ItemBuff>();
        obj.stackable = false;
        return obj;
    }
}

[TestFixture]
public class ModifiableIntTests
{
    [Test]
    public void ModifiableInt_Constructor_DefaultValueTest()
    {
        var modifiableInt = new ModifiableInt();
        Assert.AreEqual(0, modifiableInt.BaseValue);
        Assert.AreEqual(0, modifiableInt.TempValue);
        Assert.AreEqual(0, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_Constructor_WithBaseValueTest()
    {
        var modifiableInt = new ModifiableInt(baseValue: 10);
        Assert.AreEqual(10, modifiableInt.BaseValue);
        Assert.AreEqual(10, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_Constructor_NullCallback_DoesNotThrowTest()
    {
        Assert.DoesNotThrow(() => new ModifiableInt(null, 0));
    }

    [Test]
    public void ModifiableInt_BaseValue_Set_UpdatesModifiedValueTest()
    {
        var modifiableInt = new ModifiableInt();
        modifiableInt.BaseValue = 20;
        Assert.AreEqual(20, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_BaseValue_Set_NegativeValueTest()
    {
        var modifiableInt = new ModifiableInt(baseValue: 10);
        modifiableInt.BaseValue = -5;
        Assert.AreEqual(-5, modifiableInt.ModifiedValue);
    }


    [Test]
    public void ModifiableInt_TempValue_SetTest()
    {
        var modifiableInt = new ModifiableInt(baseValue: 10);
        modifiableInt.TempValue = 5;
        Assert.AreEqual(15, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_TempValue_Set_NegativeValueTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        modifiableInt.TempValue = -5;
        Assert.AreEqual(5, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_AddModifierTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        modifiableInt.AddModifier(new FlatModifier(5));
        Assert.AreEqual(15, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_AddModifier_MultipléTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        modifiableInt.AddModifier(new FlatModifier(10));
        modifiableInt.AddModifier(new FlatModifier(5));
        Assert.AreEqual(25, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_AddModifier_NegativeTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        modifiableInt.AddModifier(new FlatModifier(-5));
        Assert.AreEqual(5, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_AddModifier_WithTempAndBaseTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        modifiableInt.TempValue = 5;
        modifiableInt.AddModifier(new FlatModifier(10));
        Assert.AreEqual(25, modifiableInt.ModifiedValue);
    }


    [Test]
    public void ModifiableInt_RemoveModifierTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        var modifier = new FlatModifier(5);
        modifiableInt.AddModifier(modifier);
        modifiableInt.RemoveModifier(modifier);
        Assert.AreEqual(10, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ModifiableInt_Remove_OneModifierTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 10);
        var modifier1 = new FlatModifier(10);
        var modifier2 = new FlatModifier(5);
        modifiableInt.AddModifier(modifier1);
        modifiableInt.AddModifier(modifier2);
        modifiableInt.RemoveModifier(modifier1);
        Assert.AreEqual(15, modifiableInt.ModifiedValue);
    }
}

[TestFixture]
public class ItemBuffTests
{

    [Test]
    public void Itembuff_Constructor_GeneratesValueTest()
    {
        for (int i = 0; i < 10; i++)
        {
            var buff = new ItemBuff(max: 10, min: 1);
            Assert.That(buff.value, Is.InRange(1, 9));
        }
    }

    [Test]
    public void Itembuff_Constructor_SetsMaxAndMinTest()
    {
        var buff = new ItemBuff(max: 20, min: 5);
        Assert.AreEqual(20, buff.max);
        Assert.AreEqual(5, buff.min);
    }

    [Test]
    public void Itembuff_Constructor_SameMinMaxValueTest()
    {
        var buff = new ItemBuff(max: 5, min: 5);
        Assert.AreEqual(5, buff.value);
    }

    [Test]
    public void ItemBuff_UsedAsModifier_InModifiableIntTest()
    {
        var modifiableInt= new ModifiableInt(baseValue: 20);
        var buff = new ItemBuff(max: 5, min: 5);
        modifiableInt.AddModifier(buff);
        Assert.AreEqual(25, modifiableInt.ModifiedValue);
    }

    [Test]
    public void ItemBuff_Attribute_DefaultValueTest()
    {
        var buff = new ItemBuff(max: 1, min: 0);
        Assert.AreEqual(default(Attributes), buff.attribute);
    }

    [Test]
    public void ItemBuff_Attribute_CanBeSetTest()
    {
        var buff = new ItemBuff(max: 5, min: 1) { attribute = Attributes.Stamina };
        Assert.AreEqual(Attributes.Stamina, buff.attribute);
    }
}

[TestFixture]
public class ItemTests
{
    [Test]
    public void Item_DefaultConstructorTest()
    {
        var item = new Item();
        Assert.AreEqual("", item.Name);
        Assert.AreEqual(-1, item.ID);
    }

    [Test]
    public void Item_DefaultConstructor_BuffsTest()
    {
        var item = new Item();
        Assert.IsNull(item.Buffs);
    }

    [Test]
    public void Item_ItemObjectConstructor_CopiesNameAndIDTest()
    {
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.MainHandWeapon);
        var item = new Item(obj);

        Assert.AreEqual("TestSword", item.Name);
        Assert.AreEqual(1, item.ID);
    }

    [Test]
    public void Item_ItemObjectConstructor_CopiesTypeTest()
    {
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.MainHandWeapon);
        var item = new Item(obj);
        Assert.AreEqual(ItemType.MainHandWeapon, item.type);
    }

    [Test]
    public void Item_ItemObjectConstructor_CopiesBuffCountTest()
    {
        var buffs = new[]
        {
            new ItemBuff(max: 10, min: 5) { attribute = Attributes.Strength },
            new ItemBuff(max: 15, min: 10) { attribute = Attributes.Agility }
        };
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.MainHandWeapon, buffs);
        var item = new Item(obj);

        Assert.AreEqual(2, item.Buffs.Length);
    }

    [Test]
    public void Item_ItemObjectConstructor_NoBuffTest()
    {
        var obj = FakeItemObject.Create("EmptySword", 5);
        var item = new Item(obj);
        Assert.IsNotNull(item.Buffs);
        Assert.AreEqual(0, item.Buffs.Length);
    }

    [Test]
    public void Item_GetItemBuff_AttributeTest()
    {
        var buff = new ItemBuff(max: 5, min: 5) { attribute = Attributes.Agility };
        var obj = FakeItemObject.Create("TestSword", 2, ItemType.MainHandWeapon, new[] { buff });
        var item = new Item(obj);

        var result = item.GetItemBuff(Attributes.Agility);
        Assert.IsNotNull(result);
        Assert.AreEqual(Attributes.Agility, result.attribute);
    }

    [Test]
    public void Item_GetItemBuff_NoAttributeTest()
    {
        var buff = new ItemBuff(max: 5, min: 5) { attribute = Attributes.Strength };
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.MainHandWeapon, new[] { buff });
        var item = new Item(obj);

        var result = item.GetItemBuff(Attributes.Intellect);
        Assert.IsNull(result);
    }

    [Test]
    public void Item_GetItemBuff_MultipleBuffsTest()
    {
        var buffStrength = new ItemBuff(max: 8, min: 3) { attribute = Attributes.Strength };
        var buffAgility = new ItemBuff(max: 5, min: 5) { attribute = Attributes.Agility };
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.Accessories, new[] { buffStrength, buffAgility });
        var item = new Item(obj);

        var result = item.GetItemBuff(Attributes.Agility);
        Assert.IsNotNull(result);
        Assert.AreEqual(5, result.value);
    }

    [Test]
    public void Item_GetItemBuff_EmptyBuffArrayTest()
    {
        var obj = FakeItemObject.Create("TestSword", 1);
        var item = new Item(obj);

        Assert.IsNull(item.GetItemBuff(Attributes.Strength));
    }

    [Test]
    public void Item_ID_SetterTest()
    {
        var item = new Item();
        item.ID = 1;
        Assert.AreEqual(1, item.ID);
    }

    [Test]
    public void Item_CreateItem_ReturnsCorrectIDTest()
    {
        var obj = FakeItemObject.Create("TestSword", 1, ItemType.MainHandWeapon);
        var item = obj.CreateItem();

        Assert.AreEqual(1, item.ID);
        Assert.AreEqual(ItemType.MainHandWeapon, item.type);
    }
}