using System.Collections.Generic;
using UnityEngine;

public delegate void ModifiedEvent();
[System.Serializable]
public class ModifiableInt
{
    #region Variables
    [SerializeField] private int baseValue;
    [SerializeField] private int tempValue;
    [SerializeField] private int modifiedValue;
    private List<IModifiers> modifiers = new List<IModifiers>();

    //Getters & Setters
    public int BaseValue { get { return baseValue; } set { baseValue = value; UpdateModifiedValue(); } }
    public int TempValue { get { return tempValue; } set { tempValue = value; UpdateModifiedValue(); } }
    public int ModifiedValue { get { return modifiedValue; } }
    #endregion
    #region Event
    public event ModifiedEvent ValueModified;
    #endregion
    #region Constructor
    public ModifiableInt(ModifiedEvent method = null, int baseValue = 0)
    {
        BaseValue = baseValue;
        modifiedValue = BaseValue;
        if (method != null)
        { 
            ValueModified += method;
        }
    }
    #endregion
    #region Functions
    //manage event subscription
    public void RegsiterModEvent(ModifiedEvent method)
    {
        ValueModified += method;
    }

    public void UnregsiterModEvent(ModifiedEvent method)
    {
        ValueModified -= method;
    }

    //Changeing the modifiers
    public void AddModifier(IModifiers modifier)
    {
        modifiers.Add(modifier);
        UpdateModifiedValue();
    }

    public void RemoveModifier(IModifiers modifier)
    {
        modifiers.Remove(modifier);
        UpdateModifiedValue();
    }

    //Updateing the Modified Value
    private void UpdateModifiedValue()
    {
        var valueToAdd = 0;
        for (int i = 0; i < modifiers.Count; i++)
        {
            modifiers[i].AddValue(ref valueToAdd);
        }
        modifiedValue = baseValue + valueToAdd + tempValue;
        if (ValueModified != null)
        { 
            ValueModified.Invoke();
        }
    }
    #endregion
}