using System;

[Serializable]
public class SP_Attribute
{
    #region Variables
    [NonSerialized]public SP_Player parent;
    private Attributes type;
    private ModifiableInt value;

    //Getters & Setter
    public Attributes Type { get { return type; } set { type = value; } }
    public ModifiableInt Value { get { return value; } }
    #endregion
    #region Constructor
    public SP_Attribute(SP_Player parent, Attributes type, int baseValue)
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