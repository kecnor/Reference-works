using System;

[Serializable]
public class SP_Bot_Attribute
{
    #region Variables
    [NonSerialized] public SP_Bot parent;
    private Attributes type;
    private ModifiableInt value;

    //Getters & Setters
    public Attributes Type { get { return type; } set { type = value; } }
    public ModifiableInt Value { get { return value; } }
    public int BaseValue { get { return value.BaseValue; } set { this.value.BaseValue = value; } }
    public int ModifiedValue { get { return value.ModifiedValue; }}
    #endregion
    #region Constructor
    public SP_Bot_Attribute(SP_Bot parent, Attributes type, int baseValue)
    {
        this.parent = parent;
        this.type = type;
        value = new ModifiableInt(AttributeModified, baseValue);
    }
    #endregion
    #region Function
    public void AttributeModified()
    {
        parent.AttributeModified(this);   
    }
    #endregion
}
