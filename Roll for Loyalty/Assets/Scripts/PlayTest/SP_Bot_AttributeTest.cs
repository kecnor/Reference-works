using NUnit.Framework;
using UnityEngine;
using System;

public class SP_BotAttributeTests
{
    private GameObject botGO;
    private SP_Bot bot;

    [SetUp]
    public void SetUp()
    {
        botGO = new GameObject("Bot");
        bot = botGO.AddComponent<SP_Bot>();
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(botGO);
    }


    [Test]
    public void SP_Bot_Attribute_Constructor_SetsTypeTest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(Attributes.Strength, attribute.Type);
    }

    [Test]
    public void SP_Bot_Attribute_Constructor_SetsBaseValueTest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.BaseValue);
    }

    [Test]
    public void SP_Bot_Attribute_Constructor_SetsParentTest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreSame(bot, attribute.parent);
    }

    [Test]
    public void SP_Bot_Attribute_Value_IsNotNullTest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.IsNotNull(attribute.Value);
    }

    [Test]
    public void SP_Bot_Attribute_ModifiedValue_EqualsBaseValuetest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.ModifiedValue);
    }

    [Test]
    public void SP_Bot_Attribute_TypeSetterTest()
    {
        var attribute = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        attribute.Type = Attributes.Intellect;
        Assert.AreEqual(Attributes.Intellect, attribute.Type);
    }

    [Test]
    public void SP_Bot_Attribute_MultipleInstances_AreIndependentTest()
    {
        var attribute1 = new SP_Bot_Attribute(bot, Attributes.Strength, 10);
        var attribute2 = new SP_Bot_Attribute(bot, Attributes.Agility, 20);
        Assert.AreNotEqual(attribute1.Value.BaseValue, attribute2.Value.BaseValue);
    }

    [Test]
    public void SP_Bot_RandomClassTest()
    {
        Array values = Enum.GetValues(typeof(Classes));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomClass(), values);
        }
    }

    [Test]
    public void SP_Bot_RandomRaceTest()
    {
        Array values = Enum.GetValues(typeof(Races));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomRace(), values);
        }
    }

    [Test]
    public void SP_Bot_RandomAttitudeTest()
    {
        Array values = Enum.GetValues(typeof(Attitude));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomAttidute(), values);
        }
    }

    [Test]
    public void SP_Bot_SetReputation_CreatSizeArrayTest()
    {
        bot.SetReputation(5);
        Assert.AreEqual(5, bot.Reputations.Length);
    }

    [Test]
    public void SP_Bot_SetReputation_ZeroSize_CreatesEmptyArrayTest()
    {
        bot.SetReputation(0);
        Assert.AreEqual(0, bot.Reputations.Length);
    }

    [Test]
    public void SP_Bot_FightingTest()
    {
        Assert.IsFalse(bot.Fighting);
    }

    [Test]
    public void SP_Bot_Fighting_SetTest()
    {
        bot.Fighting = true;
        Assert.IsTrue(bot.Fighting);
    }

    [Test]
    public void SP_Bot_CheatedTest()
    {
        Assert.IsFalse(bot.Cheated);
    }

    [Test]
    public void SP_Bot_ActiveTest()
    {
        bot.Active = true;
        Assert.IsTrue(bot.Active);
        bot.Active = false;
        Assert.IsFalse(bot.Active);
    }

    [Test]
    public void SP_Bot_WouldInterfierTest()
    {
        Assert.IsFalse(bot.WouldInterfier);
    }
}