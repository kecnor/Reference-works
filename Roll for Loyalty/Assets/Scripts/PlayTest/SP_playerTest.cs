using NUnit.Framework;
using UnityEngine;


public class SP_PlayerTests
{
    private GameObject CreateBarePlayerObject()
    {
        GameObject go = new GameObject("TestPlayer");
        go.AddComponent<SP_Player>();
        return go;
    }

    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(fieldName, System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    private T GetPrivateField<T>(object obj, string fieldName)
    {
        var field = obj.GetType().GetField(fieldName,
            System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        return (T)field?.GetValue(obj);
    }

    [Test]
    public void GetClass_ValidStrings_ReturnCorrectEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var method = typeof(SP_Player).GetMethod("GetClass", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        Assert.AreEqual(Classes.Fighter, method.Invoke(player, new object[] { "Fighter" }));
        Assert.AreEqual(Classes.Rouge, method.Invoke(player, new object[] { "Rouge" }));
        Assert.AreEqual(Classes.Ranger, method.Invoke(player, new object[] { "Ranger" }));
        Assert.AreEqual(Classes.Wizard, method.Invoke(player, new object[] { "Wizard" }));

        Object.Destroy(go);
    }


    [Test]
    public void GetClass_InvalidString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var method = typeof(SP_Player).GetMethod("GetClass",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Classes result = (Classes)method.Invoke(player, new object[] { "InvalidClass" });
        Assert.IsTrue(System.Enum.IsDefined(typeof(Classes), result));

        Object.Destroy(go);
    }

    [Test]
    public void GetRace_ValidStrings_ReturnCorrectEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var method = typeof(SP_Player).GetMethod("GetRace", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Assert.AreEqual(Races.Human, method.Invoke(player, new object[] { "Human" }));
        Assert.AreEqual(Races.Dwarf, method.Invoke(player, new object[] { "Dwarf" }));
        Assert.AreEqual(Races.Elf, method.Invoke(player, new object[] { "Elf" }));

        Object.Destroy(go);
    }

    [Test]
    public void GetRace_RandomString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var method = typeof(SP_Player).GetMethod("GetRace",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Races result = (Races)method.Invoke(player, new object[] { "Random" });
        Assert.IsTrue(System.Enum.IsDefined(typeof(Races), result));

        Object.Destroy(go);
    }

    [Test]
    public void RandomClass_Always_ReturnsValidClassesEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        for (int i = 0; i < 10; i++)
        {
            Classes result = player.RandomClass();
            Assert.IsTrue(System.Enum.IsDefined(typeof(Classes), result));
        }

        Object.Destroy(go);
    }

    [Test]
    public void RandomRace_Always_ReturnsValidRacesEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        for (int i = 0; i < 10; i++)
        {
            Races result = player.RandomRace();
            Assert.IsTrue(System.Enum.IsDefined(typeof(Races), result));
        }

        Object.Destroy(go);
    }

    [Test]
    public void SetReputation_CreatesCorrectSizedArrayOfZerosTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        player.SetReputation(5);

        Assert.AreEqual(5, player.Reputations.Length);
        foreach (int rep in player.Reputations)
        {
            Assert.AreEqual(0, rep);
        }

        Object.Destroy(go);
    }

    [Test]
    public void SetReputation_Zero_CreatesEmptyArrayTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        Assert.DoesNotThrow(() => player.SetReputation(0));
        Assert.AreEqual(0, player.Reputations.Length);

        Object.Destroy(go);
    }

    [Test]
    public void GetBestAttribute_ReturnsCorrectAttributePerClassTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var method = typeof(SP_Player).GetMethod("GetBestAttribute", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

        SetPrivateField(player, "playerClass", Classes.Fighter);
        Assert.AreEqual(Attributes.Strength, method.Invoke(player, null));

        SetPrivateField(player, "playerClass", Classes.Ranger);
        Assert.AreEqual(Attributes.Agility, method.Invoke(player, null));

        SetPrivateField(player, "playerClass", Classes.Rouge);
        Assert.AreEqual(Attributes.Agility, method.Invoke(player, null));

        SetPrivateField(player, "playerClass", Classes.Wizard);
        Assert.AreEqual(Attributes.Intellect, method.Invoke(player, null));

        Object.Destroy(go);
    }
    [Test]
    public void Level_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        player.Level = 5;
        Assert.AreEqual(5, player.Level);

        player.Level = 1;
        Assert.AreEqual(1, player.Level);

        Object.Destroy(go);
    }

    [Test]
    public void Active_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        player.Active = true;
        Assert.IsTrue(player.Active);

        player.Active = false;
        Assert.IsFalse(player.Active);

        Object.Destroy(go);
    }

    [Test]
    public void Fighting_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        Assert.IsFalse(player.Fighting);

        player.Fighting = true;
        Assert.IsTrue(player.Fighting);

        Object.Destroy(go);
    }

    [Test]
    public void DrawableCards_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        player.DrawableCards = 5;
        Assert.AreEqual(5, player.DrawableCards);

        Object.Destroy(go);
    }

    [Test]
    public void ReputationID_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        player.ReputationID = 3;
        Assert.AreEqual(3, player.ReputationID);

        Object.Destroy(go);
    }

    [Test]
    public void SP_Attribute_Constructor_SetsTypeAndBaseValueTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        SP_Attribute attr = new SP_Attribute(player, Attributes.Strength, 10);

        Assert.AreEqual(Attributes.Strength, attr.Type);
        Assert.AreEqual(10, attr.Value.ModifiedValue);

        Object.Destroy(go);
    }

    [Test]
    public void AttributeList_Returns_AllSixAttributesTest()
    {
        GameObject go = CreateBarePlayerObject();
        SP_Player player = go.GetComponent<SP_Player>();

        var attributes = new SP_Attribute[6];
        attributes[0] = new SP_Attribute(player, Attributes.Health, 6);
        attributes[1] = new SP_Attribute(player, Attributes.Strength, 10);
        attributes[2] = new SP_Attribute(player, Attributes.Agility, 10);
        attributes[3] = new SP_Attribute(player, Attributes.Intellect, 10);
        attributes[4] = new SP_Attribute(player, Attributes.Charisma, 10);
        attributes[5] = new SP_Attribute(player, Attributes.Stamina, 5);
        SetPrivateField(player, "attributes", attributes);

        Assert.AreEqual(6, player.AttributeList.Length);
        Assert.AreEqual(Attributes.Health, player.AttributeList[0].Type);
        Assert.AreEqual(Attributes.Stamina, player.AttributeList[5].Type);

        Object.Destroy(go);
    }
}