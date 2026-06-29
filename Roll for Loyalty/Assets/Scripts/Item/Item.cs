using System;

[Serializable]
public class Item
{
    #region Variable
    public string name;
    public int id;
    public ItemType type;
    public ItemBuff[] buffs;

    //Getters & Setters
    public string Name { get { return name; } }
    public int ID { get { return id; } set { id = value; } }
    public ItemBuff[] Buffs { get { return buffs; } set { buffs = value; } }
    #endregion
    #region Constructor
    public Item()
    {
        name = "";
        id = -1;
    }

    public Item(ItemObject item)
    {
        name = item.name;
        id = item.data.id;
        type = item.data.type;
        buffs = new ItemBuff[item.data.buffs.Length];
        for(int i = 0; i< buffs.Length; i++)
        {
            buffs[i] = new ItemBuff( item.data.buffs[i].max, item.data.buffs[i].min);
            buffs[i].attribute = item.data.buffs[i].attribute;
        }
    }
    #endregion
    #region Function
    //Get an item's specific attribut if it's exist
    public ItemBuff GetItemBuff(Attributes attribute)
    {
        foreach (ItemBuff buff in buffs)
        {
            if (buff.attribute == attribute)
            {
                return buff;
            }
        }
        return null;
    }
    #endregion
}