using NUnit.Framework;
using UnityEngine;
using System;

public class MP_BotAttributeTests
{
    private GameObject botGO;
    private MP_Bot bot;

    [SetUp]
    public void SetUp()
    {
        botGO = new GameObject("MP_Bot");
        bot = botGO.AddComponent<MP_Bot>();
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(botGO);
    }

    [Test]
    public void MP_Bot_Attribute_Constructor_SetsTypeTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(Attributes.Strength, attribute.Type);
    }

    [Test]
    public void MP_Bot_Attribute_Constructor_SetsBaseValueTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.BaseValue);
    }

    [Test]
    public void MP_Bot_Attribute_Constructor_SetsParentTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreSame(bot, attribute.parent);
    }

    [Test]
    public void MP_Bot_Attribute_Value_IsNotNullTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.IsNotNull(attribute.Value);
    }

    [Test]
    public void MP_Bot_Attribute_ModifiedValue_EqualsBaseValueTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.ModifiedValue);
    }

    [Test]
    public void MP_Bot_Attribute_TypeSetter_ChangeTest()
    {
        var attribute = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        attribute.Type = Attributes.Intellect;
        Assert.AreEqual(Attributes.Intellect, attribute.Type);
    }

    [Test]
    public void MP_Bot_Attribute_MultipleInstances_IndependentTest()
    {
        var attribute1 = new MP_Bot_Attribute(bot, Attributes.Strength, 10);
        var attribute2 = new MP_Bot_Attribute(bot, Attributes.Agility, 20);
        Assert.AreNotEqual(attribute1.Value.BaseValue, attribute2.Value.BaseValue);
    }

    [Test]
    public void MP_Bot_RandomClass_ReturnsClassTest()
    {
        Array values = Enum.GetValues(typeof(Classes));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomClass(), values);
        }
    }

    [Test]
    public void MP_Bot_RandomRace_ReturnsRaceTest()
    {
        Array values = Enum.GetValues(typeof(Races));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomRace(), values);
        }
    }

    [Test]
    public void MP_Bot_RandomAttitude_ReturnsAttitudeTest()
    {
        Array values = Enum.GetValues(typeof(Attitude));
        for (int i = 0; i < 10; i++)
        { 
            Assert.Contains(bot.RandomAttidute(), values);
        }
    }

    [Test]
    public void MP_Bot_SetReputation_CreatesSizeArrayTest()
    {
        bot.SetReputation(5);
        Assert.AreEqual(5, bot.Reputations == null ? 0 : 5);
        Assert.AreEqual(5, GetReputations(bot).Length);
    }

    [Test]
    public void MP_Bot_SetReputation_CreatesEmptyArrayTest()
    {
        bot.SetReputation(0);
        Assert.AreEqual(0, GetReputations(bot).Length);
    }

    [Test]
    public void MP_Bot_SetReputation_DefaultAttitudeTest()
    {
        bot.SetReputation(3);
        int[] reputations = GetReputations(bot);
        int first = reputations[0];
        foreach (int reputation in reputations)
        { 
            Assert.AreEqual(first, reputation);
        }
    }

    private static int[] GetReputations(MP_Bot target)
    {
        var field = typeof(MP_Bot).GetField("reputations", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (int[])field.GetValue(target);
    }

    [Test]
    public void MP_Bot_FightingTest()
    {
        Assert.IsFalse(bot.Fighting);
    }

    [Test]
    public void MP_Bot_Fighting_SetToTrueTest()
    {
        bot.Fighting = true;
        Assert.IsTrue(bot.Fighting);
    }

    [Test]
    public void MP_Bot_Cheated_DefaultTest()
    {
        Assert.IsFalse(bot.Cheated);
    }

    [Test]
    public void MP_Bot_ActiveTest()
    {
        bot.Active = true;
        Assert.IsTrue(bot.Active);
        bot.Active = false;
        Assert.IsFalse(bot.Active);
    }

    [Test]
    public void MP_Bot_WouldInterfierTest()
    {
        Assert.IsFalse(bot.WouldInterfier);
    }
}