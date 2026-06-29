using NUnit.Framework;
using UnityEngine;

public class MP_PlayerTests
{
    private GameObject CreateBarePlayerObject()
    {
        GameObject go = new GameObject("TestMP_Player");
        go.AddComponent<MP_Player>();
        return go;
    }

    private void SetPrivateField(object obj, string fieldName, object value)
    {
        var field = obj.GetType().GetField(
            fieldName,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        field?.SetValue(obj, value);
    }

    private object InvokePrivate(object obj, string methodName, object[] args = null)
    {
        var method = obj.GetType().GetMethod(
            methodName,
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);
        return method?.Invoke(obj, args);
    }

    [Test]
    public void GetClass_ValidStrings_ReturnCorrectEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Assert.AreEqual(Classes.Fighter,
            InvokePrivate(player, "GetClass", new object[] { "Fighter" }));
        Assert.AreEqual(Classes.Rouge,
            InvokePrivate(player, "GetClass", new object[] { "Rouge" }));
        Assert.AreEqual(Classes.Ranger,
            InvokePrivate(player, "GetClass", new object[] { "Ranger" }));
        Assert.AreEqual(Classes.Wizard,
            InvokePrivate(player, "GetClass", new object[] { "Wizard" }));

        Object.Destroy(go);
    }

    [Test]
    public void GetClass_InvalidString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Classes result = (Classes)InvokePrivate(player, "GetClass", new object[] { "InvalidClass" });

        Assert.IsTrue(System.Enum.IsDefined(typeof(Classes), result));

        Object.Destroy(go);
    }

    [Test]
    public void GetClass_RandomString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Classes result = (Classes)InvokePrivate(player, "GetClass", new object[] { "Random" });

        Assert.IsTrue(System.Enum.IsDefined(typeof(Classes), result));

        Object.Destroy(go);
    }

    [Test]
    public void GetRace_ValidStrings_ReturnCorrectEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Assert.AreEqual(Races.Human, InvokePrivate(player, "GetRace", new object[] { "Human" }));
        Assert.AreEqual(Races.Dwarf, InvokePrivate(player, "GetRace", new object[] { "Dwarf" }));
        Assert.AreEqual(Races.Elf, InvokePrivate(player, "GetRace", new object[] { "Elf" }));

        Object.Destroy(go);
    }

    [Test]
    public void GetRace_RandomString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Races result = (Races)InvokePrivate(player, "GetRace", new object[] { "Random" });

        Assert.IsTrue(System.Enum.IsDefined(typeof(Races), result));

        Object.Destroy(go);
    }

    [Test]
    public void GetRace_InvalidString_ReturnsValidEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Races result = (Races)InvokePrivate(player, "GetRace", new object[] { "InvalidRace" });

        Assert.IsTrue(System.Enum.IsDefined(typeof(Races), result));

        Object.Destroy(go);
    }
    [Test]
    public void RandomClass_Always_ReturnsValidClassesEnumTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

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
        MP_Player player = go.GetComponent<MP_Player>();

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
        MP_Player player = go.GetComponent<MP_Player>();

        player.SetReputation(5);

        Assert.AreEqual(5, player.Reputations.Length);
        foreach (int reputation in player.Reputations)
        { 
            Assert.AreEqual(0, reputation);
        }

        Object.Destroy(go);
    }

    [Test]
    public void SetReputation_Zero_CreatesEmptyArrayTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        Assert.DoesNotThrow(() => player.SetReputation(0));
        Assert.AreEqual(0, player.Reputations.Length);

        Object.Destroy(go);
    }

    [Test]
    public void GetBestAttribute_ReturnsCorrectAttributePerClassTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        SetPrivateField(player, "playerClass", new Unity.Netcode.NetworkVariable<Classes>(Classes.Fighter));
        Assert.AreEqual(Attributes.Strength, InvokePrivate(player, "GetBestAttribute"));

        SetPrivateField(player, "playerClass", new Unity.Netcode.NetworkVariable<Classes>(Classes.Ranger));
        Assert.AreEqual(Attributes.Agility, InvokePrivate(player, "GetBestAttribute"));

        SetPrivateField(player, "playerClass", new Unity.Netcode.NetworkVariable<Classes>(Classes.Rouge));
        Assert.AreEqual(Attributes.Agility, InvokePrivate(player, "GetBestAttribute"));

        SetPrivateField(player, "playerClass", new Unity.Netcode.NetworkVariable<Classes>(Classes.Wizard));
        Assert.AreEqual(Attributes.Intellect, InvokePrivate(player, "GetBestAttribute"));

        Object.Destroy(go);
    }

    [Test]
    public void Cheated_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        player.Cheated = true;
        Assert.IsTrue(player.Cheated);

        player.Cheated = false;
        Assert.IsFalse(player.Cheated);

        Object.Destroy(go);
    }

    [Test]
    public void Reputations_GetterSetter_WorksCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        int[] reps = new int[] { 1, -1, 3 };
        player.Reputations = reps;

        Assert.AreEqual(reps, player.Reputations);
        Assert.AreEqual(3, player.Reputations.Length);

        Object.Destroy(go);
    }

    [Test]
    public void MP_Attribute_Constructor_SetsTypeAndBaseValueTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        MP_Attribute attr = new MP_Attribute(player, Attributes.Strength, 10);

        Assert.AreEqual(Attributes.Strength, attr.Type);
        Assert.AreEqual(10, attr.Value.ModifiedValue);

        Object.Destroy(go);
    }

    [Test]
    public void MP_Attribute_Parent_IsSetCorrectlyTest()
    {
        GameObject go = CreateBarePlayerObject();
        MP_Player player = go.GetComponent<MP_Player>();

        MP_Attribute attr = new MP_Attribute(player, Attributes.Health, 6);

        Assert.AreEqual(player, attr.parent);

        Object.Destroy(go);
    }
}