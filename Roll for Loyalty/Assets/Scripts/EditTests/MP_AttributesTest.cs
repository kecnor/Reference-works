using NUnit.Framework;

public class MP_AttributeTests
{

    [Test]
    public void MP_Attribute_Constructor_SetsTypeTest()
    {
        var attribute = new MP_Attribute(null, Attributes.Strength, 10);
        Assert.AreEqual(Attributes.Strength, attribute.Type);
    }

    [Test]
    public void MP_Attribute_Constructor_SetsBaseValueTest()
    {
        var attribute = new MP_Attribute(null, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.BaseValue);
    }

    [Test]
    public void MP_Attribute_Constructor_ParentTest()
    {
        Assert.DoesNotThrow(() => new MP_Attribute(null, Attributes.Strength, 10));
    }

    [Test]
    public void MP_Attribute_ValueTest()
    {
        var attribute = new MP_Attribute(null, Attributes.Strength, 10);
        Assert.IsNotNull(attribute.Value);
    }


    [Test]
    public void MP_Attribute_ModifiedValueTest()
    {
        var attribute = new MP_Attribute(null, Attributes.Strength, 10);
        Assert.AreEqual(10, attribute.Value.ModifiedValue);
    }

    [Test]
    public void MP_Attribute_TypeSetter_ChangeTest()
    {
        var attribute = new MP_Attribute(null, Attributes.Strength, 10);
        attribute.Type = Attributes.Intellect;
        Assert.AreEqual(Attributes.Intellect, attribute.Type);
    }

    [Test]
    public void MP_Attribute_MultipleInstances_IndependentTest()
    {
        var attribute1 = new MP_Attribute(null, Attributes.Strength, 10);
        var attribute2 = new MP_Attribute(null, Attributes.Agility, 20);
        Assert.AreNotEqual(attribute1.Value.BaseValue, attribute2.Value.BaseValue);
    }

    [Test]
    public void MP_Attribute_DifferentTypes_StoredTest()
    {
        var attribute1 = new MP_Attribute(null, Attributes.Strength, 10);
        var attribute2 = new MP_Attribute(null, Attributes.Stamina, 10);
        Assert.AreNotEqual(attribute1.Type, attribute2.Type);
    }

    [Test]
    public void ModifiableInt_InitializesWitBaseValueTest()
    {
        var modifiableInt = new ModifiableInt(null, 10);
        Assert.AreEqual(10, modifiableInt.BaseValue);
    }

    [Test]
    public void ModifiableInt_BaseValue_SetDirectlyTest()
    {
        var modifiableInt = new ModifiableInt(null, 10);
        modifiableInt.BaseValue = 20;
        Assert.AreEqual(20, modifiableInt.BaseValue);
    }
}