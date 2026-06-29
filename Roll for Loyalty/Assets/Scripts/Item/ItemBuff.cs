using System;

[Serializable]
public class ItemBuff : IModifiers
{
    #region Variables
    public Attributes attribute;
    public int value;
    public int max;
    public int min;
    #endregion
    #region Constructor
    public ItemBuff(int max, int min)
    {
        this.max = max;
        this.min = min;
        GenerateValue();
    }
    #endregion
    #region Functions
    //Generating the item's itembuff's value
    public void GenerateValue()
    {
        value = UnityEngine.Random.Range(min, max);
    }

    public void AddValue(ref int value)
    {
        value += this.value;
    }
    #endregion
}