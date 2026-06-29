using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class MP_Player : NetworkBehaviour
{
    #region Variables
    //Base values
    private string playerName;
    private NetworkVariable<int> level = new NetworkVariable<int>(0);
    private NetworkVariable<Classes> playerClass = new NetworkVariable<Classes>();
    private NetworkVariable<Races> playerRace = new NetworkVariable<Races>();
    public MP_Attribute[] attributes;
    private NetworkList<int> attributesList = new NetworkList<int>();
    private NetworkVariable<int> movement = new NetworkVariable<int>(0);
    private NetworkVariable<int> attackPower = new NetworkVariable<int>(0);
    private NetworkVariable<int> drawableCards = new NetworkVariable<int>(0);
    private int[] reputations;
    private NetworkVariable<int> repotationID = new NetworkVariable<int>(0);
    private NetworkVariable<bool> active = new NetworkVariable<bool>(false);
    private bool cheated;
    private NetworkVariable<bool> fighting = new NetworkVariable<bool>(false);

    private InventoryObject inventory;
    private InventoryObject equipment;

    //Scriptable Objects
    private WriteLog writeLog;
    private MP_Cheating cheat;

    //GameObject
    private Transform stats;

    //Setter & Getters
    public string Name { get { return playerName; }}
    public int Level { get { return level.Value; } set { level.Value = value; } }
    public Classes PlayerClass { get { return playerClass.Value; } set { playerClass.Value = value; } }
    public Races PlayerRace { get { return playerRace.Value; } set { playerRace.Value = value; } }
    public MP_Attribute[] AttributeList { get { return attributes; } }
    public NetworkList<int> AttributeNetworkList { get { return attributesList; } }
    public int Movement { get { return movement.Value; } set { movement.Value = value; } }
    public int AttackPower { get { return attackPower.Value; } }
    public int DrawableCards { get { return drawableCards.Value; } set { drawableCards.Value = value; } }
    public int[] Reputations { get { return reputations; } set { reputations = value; } }
    public int ReputationID { get { return repotationID.Value; } set { repotationID.Value = value; } }
    public bool Active { get { return active.Value; } set { active.Value = value; } }
    public bool Cheated { get { return cheated; } set { cheated = value; } }
    public bool Fighting { get { return fighting.Value; } set { fighting.Value = value; } }
    public InventoryObject Inventory { get { return inventory; } }
    public InventoryObject Equipment { get { return equipment; } }
    #endregion
    #region Constructor
    public void Init(string name, string playerClass, string playerRace)
    {
        this.playerName = name;
        level.Value = 1;
        this.playerClass.Value = GetClass(playerClass);
        this.playerRace.Value = GetRace(playerRace);
        attributes = new MP_Attribute[6];
        attributes[0] = new MP_Attribute(this, Attributes.Health, 6);
        attributes[1] = new MP_Attribute(this, Attributes.Strength, 10);
        attributes[2] = new MP_Attribute(this, Attributes.Agility, 10);
        attributes[3] = new MP_Attribute(this, Attributes.Intellect, 10);
        attributes[4] = new MP_Attribute(this, Attributes.Charisma, 10);
        attributes[5] = new MP_Attribute(this, Attributes.Stamina, 5);
        foreach (MP_Attribute attribute in attributes)
        {
            attributesList.Add(attribute.Value.ModifiedValue);
        }
        movement.Value = attributes[5].Value.ModifiedValue;
        drawableCards.Value = 4;

        inventory = Instantiate(Resources.Load<InventoryObject>("PlayerInventory"));
        equipment = Instantiate(Resources.Load<InventoryObject>("PlayerEquipment"));
        ClearInventoriesRpc();

        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
        Transform inventoryTransform = multiplayer.Find("Inventory");
        UserInterface inventoryUI = inventoryTransform.GetComponent<UserInterface>();
        inventoryUI.BindInventory(inventory);
        inventoryUI.OnUseItem = buffs =>
        {
            MP_Characters characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();

            characters.ApplyBuff(buffs, gameObject);
        };
        Transform equipmentTransform = multiplayer.Find("Equipment");
        UserInterface equipmentUI = equipmentTransform.GetComponent<UserInterface>();
        equipmentUI.BindInventory(equipment);
        foreach (InventorySlot slot in equipment.GetSlots)
        {
            slot.OnBeforeUpdate += OnBeforeSlotUpdate;
            slot.OnAfterUpdate += OnAfterSlotUpdate;
        }

        cheat = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCheat").GetComponent<MP_Cheating>();
        MP_Characters characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        SetReputation(characters.CharacterList.Count);

        ChangeSkinRpc(this.playerClass.Value.ToString());
    }
    #endregion
    #region Functions
    private void OnApplicationQuit()
    {
        inventory.Container.Clear();
        equipment.Container.Clear();
    }

    //Clear all inventories
    [Rpc(SendTo.Owner)]
    private void ClearInventoriesRpc()
    {
        if (!IsServer)
        {
            inventory = GameObject.Find("Main Camera/MultiPlayer/Inventory").GetComponent<DynamicInterface>().inventory;
            inventory.Container.Clear();
            equipment = GameObject.Find("Main Camera/MultiPlayer/Equipment").GetComponent<StaticInterface>().inventory;
            equipment.Container.Clear();
            foreach (InventorySlot slot in equipment.GetSlots)
            {
                slot.OnBeforeUpdate += OnBeforeSlotUpdate;
                slot.OnAfterUpdate += OnAfterSlotUpdate;
            }
        }
    }

    //Change all the players skin on the server
    [Rpc(SendTo.ClientsAndHost)]
    public void ChangeSkinRpc(string className)
    {
        GetComponent<ChangeCharacterSkin>().ChangeSkin(className);
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
        for (int i = 0; i < size; i++)
        {
            reputations[i] = 0;
        }
    }

    //Activateing the player's turn
    [Rpc(SendTo.Server)]
    public void ActivateTurnRpc()
    {
        cheated = false;
        movement.Value = attributes[5].Value.ModifiedValue;
        UpdateStatsVisualClientRpc();
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
                        { 
                            attributes[j].Value.RemoveModifier(_slot.Item.Buffs[i]);
                            attributesList[j] = attributes[j].Value.ModifiedValue;
                            UpdateStatsVisualClientRpc();
                        }
                    }
                }
                break;
            default:
                break;
        }
        WriteLogRpc($"{playerName} equiped a {_slot.Item.name}. {playerName}'s new attack power is {attackPower.Value}");
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
                        {
                            attributes[j].Value.AddModifier(_slot.Item.Buffs[i]);
                            attributesList[j] = attributes[j].Value.ModifiedValue;
                            UpdateStatsVisualClientRpc();
                        }
                    }
                }
                break;
            default:
                break;
        }
        WriteLogRpc($"{playerName} equiped a {_slot.Item.name}. {playerName}'s new attack power is {attackPower.Value}");
    }

    //Update the player's attackPower if nessesarry and Update visuals
    public void AttributeModified(MP_Attribute attribute)
    {
        CalculateAttackPower();
        UpdateStatsVisualClientRpc();

        if (fighting.Value)
        {
            MP_MonsterTile tile = GameObject.Find("ScriptObjects/GameBoard").GetComponent<GameBoard>().GetGameObject(transform.position).GetComponent<MP_MonsterTile>();
            tile.UpdateFightRpc();
        }
    }

    //Update the stats GameObjetc visual
    [Rpc(SendTo.Owner)]
    public void UpdateStatsVisualClientRpc()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
        stats = multiplayer.Find("Stats");

        stats.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = playerClass.Value.ToString();
        stats.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = playerRace.Value.ToString();
        stats.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = level.Value.ToString();
        for (int i = 0; i < attributesList.Count; i++)
        {
            stats.transform.GetChild(i + 3).GetComponent<TextMeshProUGUI>().text = attributesList[i].ToString();
        }
        stats.transform.GetChild(8).GetComponent<TextMeshProUGUI>().text = movement.Value.ToString();
        stats.transform.GetChild(9).GetComponent<TextMeshProUGUI>().text = attackPower.Value.ToString();
        stats.transform.GetChild(10).GetComponent<TextMeshProUGUI>().text = drawableCards.Value.ToString();
    }

    //Calculating player's Attack power
    private void CalculateAttackPower()
    {
        int topAttribute = 0;
        foreach (MP_Attribute attribute in attributes)
        {
            if (attribute.Value.ModifiedValue > topAttribute)
            {
                topAttribute = attribute.Value.ModifiedValue;
            }
        }
        attackPower.Value = topAttribute;
        attackPower.Value += BonusFromClass();
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
        return count * level.Value;
    }

    //Provides the bonus giver attribute of the class
    private Attributes GetBestAttribute()
    {
        switch (playerClass.Value)
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
            foreach (MP_Attribute attribute in attributes)
            {
                if (buff.attribute == attribute.Type)
                {
                    if (id != repotationID.Value && id != -1)
                    {
                        if (buff.value > 0)
                        {
                            reputations[id]++;
                        }
                        else
                        { 
                            reputations[id]--;
                        }
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
        WriteLogRpc($"{playerName} has been caught on cheating");
    }

    [Rpc(SendTo.Owner)]
    public void AddItemToInventoryClientRpc(int itemId, int amount)
    {
        inventory = GameObject.Find("Main Camera/MultiPlayer/Inventory").GetComponent<DynamicInterface>().inventory;
        ItemObject itemObject = inventory.database.GetItem(itemId);
        Item item = new Item(itemObject);
        inventory.AddItem(item, amount);
    }

    //Write in the log for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void WriteLogRpc(string message)
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
        Transform log = multiplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();

        writeLog.WriteNewLog(message);
    }
    #endregion
}