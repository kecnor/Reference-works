using NUnit.Framework;
using UnityEngine;
using System;

public class SP_AttributeTests
{
    private GameObject playerGO;
    private SP_Player player;

    [SetUp]
    public void SetUp()
    {
        playerGO = new GameObject("Player");
        player = playerGO.AddComponent<SP_Player>();
    }

    [TearDown]
    public void TearDown()
    {
        GameObject.DestroyImmediate(playerGO);
    }

    [Test]
    public void SP_Attribute_Constructor_SetsTypeTest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        Assert.AreEqual(Attributes.Strength, attribute.Type);
    }

    [Test]
    public void SP_Attribute_Constructor_SetsBaseValueTest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.BaseValue);
    }

    [Test]
    public void SP_Attribute_Constructor_SetsParenttest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        Assert.AreSame(player, attribute.parent);
    }

    [Test]
    public void SP_Attribute_ValueNotNullTest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        Assert.IsNotNull(attribute.Value);
    }


    [Test]
    public void SP_Attribute_ModifiedValue_EqualsBaseValueTest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.ModifiedValue);
    }

    [Test]
    public void SP_Attribute_TypeSetter_ChangesTypeTest()
    {
        var attribute = new SP_Attribute(player, Attributes.Strength, 10);
        attribute.Type = Attributes.Intellect;
        Assert.AreEqual(Attributes.Intellect, attribute.Type);
    }

    [Test]
    public void SP_Player_RandomClassTest()
    {
        Array values = Enum.GetValues(typeof(Classes));
        for (int i = 0; i < 20; i++)
        {
            Classes result = player.RandomClass();
            Assert.Contains(result, values);
        }
    }

    [Test]
    public void SP_Player_RandomRaceTest()
    {
        Array values = Enum.GetValues(typeof(Races));
        for (int i = 0; i < 20; i++)
        {
            Races result = player.RandomRace();
            Assert.Contains(result, values);
        }
    }

    [Test]
    public void SP_Player_SetReputationTest()
    {
        player.SetReputation(5);
        Assert.AreEqual(5, player.Reputations.Length);
    }

    [Test]
    public void SP_Player_SetReputation_AllZeroTest()
    {
        player.SetReputation(5);
        foreach (int rep in player.Reputations)
        { 
            Assert.AreEqual(0, rep);
        }
    }

    [Test]
    public void SP_Player_FightingTest()
    {
        Assert.IsFalse(player.Fighting);
    }

    [Test]
    public void SP_Player_Fighting_CanBeSetTrueTest()
    {
        player.Fighting = true;
        Assert.IsTrue(player.Fighting);
    }

    [Test]
    public void SP_Player_ActiveTest()
    {
        player.Active = true;
        Assert.IsTrue(player.Active);
        player.Active = false;
        Assert.IsFalse(player.Active);
    }

    [Test]
    public void SP_Player_CheatedTest()
    {
        Assert.IsFalse(player.Cheated);
    }
}