using System;

[Serializable]
public class MP_Bot_Attribute
{
    #region Variables
    [NonSerialized] public MP_Bot parent;
    private Attributes type;
    private ModifiableInt value;

    //Getters & Setters
    public Attributes Type { get { return type; } set { type = value; } }
    public ModifiableInt Value { get { return value; } }
    public int BaseValue { get { return value.BaseValue; } set { this.value.BaseValue = value; } }
    public int ModifiedValue { get { return value.ModifiedValue; }}
    #endregion
    #region Constructor
    public MP_Bot_Attribute(MP_Bot parent, Attributes type, int baseValue)
    {
        this.parent = parent;
        this.type = type;
        value = new ModifiableInt(AttributeModified, baseValue);
    }
    #endregion
    #region Functions
    public void AttributeModified()
    {
        parent.AttributeModified(this);   
    }
    #endregion
}