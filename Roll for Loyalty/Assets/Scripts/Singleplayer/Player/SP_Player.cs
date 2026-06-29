using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SP_Player : MonoBehaviour
{
    #region Variables
    //Base values
    private string playerName;
    private int level;
    private Classes playerClass;
    private Races playerRace;
    private SP_Attribute[] attributes;
    private int movement;
    private int attackPower;
    private int drawableCards;
    private int[] reputations;
    private int repotationID;
    private bool active;
    private bool cheated;
    private bool fighting = false;

    private InventoryObject inventory;
    private InventoryObject equipment;

    //Scriptable Objects
    private SP_Cheating cheat;
    private WriteLog writeLog;

    //GameObject
    private Transform stats;


    //Setter & Getters
    public string Name { get { return playerName; } }
    public int Level { get { return level; } set { level = value; } }
    public Classes PlayerClass { get { return playerClass; } set { playerClass = value; } }
    public Races PlayerRace { get { return playerRace; }  set { playerRace = value; } }
    public SP_Attribute[] AttributeList { get { return attributes; } }
    public int Movement { get { return movement; } set { movement = value; } }
    public int AttackPower { get { return attackPower; } }
    public int DrawableCards { get { return drawableCards; } set { drawableCards = value; } }
    public int[] Reputations { get { return reputations; } set { reputations = value; } }
    public int ReputationID { get { return repotationID; } set { repotationID = value; } }
    public bool Active { get { return active; } set { active = value; } }
    public bool Cheated { get { return cheated; } set { cheated = value; } }
    public bool Fighting { get { return fighting; } set { fighting = value; } }
    public InventoryObject Inventory { get { return inventory; } }
    public InventoryObject Equipment { get { return equipment; } }
    #endregion
    #region Constructor
    public void Init(string name, string playerClass, string playerRace)
    {
        this.playerName = name;
        level = 1;
        this.playerClass = GetClass(playerClass);
        this.playerRace = GetRace(playerRace);
        attributes = new SP_Attribute[6];
        attributes[0] = new SP_Attribute(this, Attributes.Health, 6);
        attributes[1] = new SP_Attribute(this, Attributes.Strength, 10);
        attributes[2] = new SP_Attribute(this, Attributes.Agility, 10);
        attributes[3] = new SP_Attribute(this, Attributes.Intellect, 10);
        attributes[4] = new SP_Attribute(this, Attributes.Charisma, 10);
        attributes[5] = new SP_Attribute(this, Attributes.Stamina, 5);
        movement = attributes[5].Value.ModifiedValue;
        drawableCards = 4;

        inventory = Resources.Load("PlayerInventory") as InventoryObject;
        equipment = Resources.Load("PlayerEquipment") as InventoryObject;
        foreach (InventorySlot slot in equipment.GetSlots)
        {
            slot.OnBeforeUpdate += OnBeforeSlotUpdate;
            slot.OnAfterUpdate += OnAfterSlotUpdate;
        }

        CalculateAttackPower();

        GetComponent<ChangeCharacterSkin>().ChangeSkin(this.playerClass.ToString());

        cheat = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCheat").GetComponent<SP_Cheating>();
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform singleplayer = mainCamera.transform.Find("SinglePlayer");
        stats = singleplayer.Find("Stats");
        Transform log = singleplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();
        UserInterface inventoryUI = singleplayer.Find("Inventory").GetComponent<UserInterface>();
        inventoryUI.BindInventory(inventory);
        inventoryUI.OnUseItem = buffs =>
        {
            SP_Characters characters = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCharacters").GetComponent<SP_Characters>();
            characters.ApplyBuff(buffs, gameObject);
        };
    }
    #endregion
    #region Functions
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }

    //Convert string into enum variables
    private Classes GetClass(string playerClass)
    {
        switch (playerClass)
        {
            case "Fighter":
                return Classes.Fighter;
            case "Rouge":
                return Classes.Rouge;
            case "Ranger":
                return Classes.Ranger;
            case "Wizard":
                return Classes.Wizard;
            case "Random":
                return RandomClass();
            default:
                return RandomClass();
        }
    }

    private Races GetRace(string playerRace)
    {
        switch (playerRace)
        {
            case "Human":
                return Races.Human;
            case "Dwarf":
                return Races.Dwarf;
            case "Elf":
                return Races.Elf;
            case "Random":
                return RandomRace();
            default:
                return RandomRace();
        }
    }

    //Random generators for enums
    public Classes RandomClass()
    {
        Array values = Enum.GetValues(typeof(Classes));
        return (Classes)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    public Races RandomRace()
    {
        Array values = Enum.GetValues(typeof(Races));
        return (Races)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    //Setting up the player reputations
    public void SetReputation(int size)
    {
        reputations = new int[size];
        for(int i = 0; i < size; i++)
        {
            reputations[i] = 0;
        }
    }

    //Activateing the player's turn
    public void ActivateTurn()
    {
        cheated = false;
        movement = attributes[5].Value.ModifiedValue;
        UpdateStatsVisual();
    }

    //Changes the equiped items on the player
    public void OnBeforeSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parentInventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < _slot.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].Type == _slot.Item.Buffs[i].attribute)
                            attributes[j].Value.RemoveModifier(_slot.Item.Buffs[i]);
                    }
                }
                break;
            default:
                break;
        }
        writeLog.WriteNewLog($"{playerName} unequiped a {_slot.Item.name}. {playerName}'s new attack power is {attackPower}");
    }
    public void OnAfterSlotUpdate(InventorySlot _slot)
    {
        if (_slot.ItemObject == null)
            return;
        switch (_slot.parentInventory.type)
        {
            case InterfaceType.Inventory:
                break;
            case InterfaceType.Equipment:
                for (int i = 0; i < _slot.Item.Buffs.Length; i++)
                {
                    for (int j = 0; j < attributes.Length; j++)
                    {
                        if (attributes[j].Type == _slot.Item.Buffs[i].attribute)
                            attributes[j].Value.AddModifier(_slot.Item.Buffs[i]);
                    }
                }
                break;
            default:
                break;
        }
        writeLog.WriteNewLog($"{playerName} equiped a {_slot.Item.name}. {playerName}'s new attack power is {attackPower}");
    }

    //Update the player's attackPower if nessesarry and Update visuals
    public void AttributeModified(SP_Attribute attribute)
    {
        CalculateAttackPower();
        UpdateStatsVisual();
        if (fighting)
        {
            SP_MonsterTile tile = GameObject.Find("ScriptObjects/GameBoard").GetComponent<GameBoard>().GetGameObject(transform.position).GetComponent<SP_MonsterTile>();
            tile.UpdateVisuals();
        }
    }
    
    //Update the stats GameObjetc visual
    public void UpdateStatsVisual()
    {
        stats.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerClass.ToString();
        stats.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerRace.ToString();
        stats.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Level.ToString();
        for (int i = 0; i < attributes.Length - 1; i++)
        {
            stats.transform.GetChild(i + 3).GetComponent<TextMeshProUGUI>().text = attributes[i].Value.ModifiedValue.ToString();
        }
        stats.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = movement.ToString();
        stats.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = attackPower.ToString();
        stats.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = drawableCards.ToString();
    }

    //Calculating player's Attack power
    private void CalculateAttackPower()
    {
        int topAttribute = 0;
        foreach (SP_Attribute attribute in attributes)
        {
            if (attribute.Value.ModifiedValue > topAttribute)
            {
                topAttribute = attribute.Value.ModifiedValue;
            }
        }
        attackPower = topAttribute;
        attackPower += BonusFromClass();
    }
    private int BonusFromClass()
    {
        int count = 0;
        Attributes attribute = GetBestAttribute();
        foreach (InventorySlot slot in equipment.GetSlots) 
        {
            if (slot.Item.id != -1)
            {
                foreach (ItemBuff buff in slot.Item.buffs)
                {
                    if (buff.attribute == attribute)
                    {
                        count++;
                    }
                }
            }
        }
        return count * level;
    }

    //Provides the bonus giver attribute of the class
    private Attributes GetBestAttribute()
    {
        switch (playerClass)
        {
            case Classes.Fighter:
                return Attributes.Strength;
            case Classes.Ranger:
                return Attributes.Agility;
            case Classes.Rouge:
                return Attributes.Agility;
            case Classes.Wizard:
                return Attributes.Intellect;
            default:
                return Attributes.Strength;
        }
    }

    //Player applying an item's buffs
    public void ApplyBuff(ItemBuff[] buffs, int id)
    {
        foreach (ItemBuff buff in buffs)
        {
            foreach (SP_Attribute attribute in attributes)
            {
                if (buff.attribute == attribute.Type)
                {
                    if (buff.value > 0 && id != repotationID)
                    {
                        reputations[id]++;
                    }
                    else if (id != repotationID)
                    {
                        reputations[id]--;
                    }
                    attribute.Value.TempValue = buff.value;
                }
            }
        }
    }

    //Player gets punished for cheating
    public void CaughtOnCheating(int reputationindex)
    {
        cheated = false;
        List<Item> items = new List<Item>();
        foreach (InventorySlot slot in inventory.GetSlots)
        {
            if (slot.Item.ID != -1)
            {
                items.Add(slot.Item);
            }
        }
        int minusItems = cheat.Punishment;
        if (items.Count < minusItems)
        {
            minusItems = items.Count;
        }
        for (int i = 0; i < minusItems; i++)
        {
            Item minusItem = items[UnityEngine.Random.Range(0, items.Count)];
            items.Remove(minusItem);
            inventory.RemoveItem(minusItem);
        }
        reputations[reputationindex]--;
        writeLog.WriteNewLog($"{playerName} has been caught on cheating");
    }
    #endregion
}