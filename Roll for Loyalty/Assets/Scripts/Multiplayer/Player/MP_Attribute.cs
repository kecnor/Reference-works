using System;

[Serializable]
public class MP_Attribute
{
    #region Variables
    [NonSerialized]public MP_Player parent;
    private Attributes type;
    private ModifiableInt value;

    //Getters & Setter
    public Attributes Type { get { return type; } set { type = value; } }
    public ModifiableInt Value { get { return value; } }
    #endregion
    #region Constructor
    public MP_Attribute(MP_Player parent, Attributes type, int baseValue)
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