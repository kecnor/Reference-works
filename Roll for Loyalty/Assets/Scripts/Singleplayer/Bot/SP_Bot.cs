using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SP_Bot : MonoBehaviour
{
    #region Variables
    //Base values
    private string botName;
    private int level;
    private Classes botClass;
    private Races botRace;
    private Attitude botAttitude;
    private SP_Bot_Attribute[] attributes;
    private int movement;
    private int attackPower;
    private int drawableCards;
    private int[] reputations;
    private int repotationID;
    private bool active;
    private bool cheated;
    private bool fighting = false;
    private bool wouldInterfier = false;

    //inventories
    private InventoryObject inventory;
    private InventoryObject equipment;

    //Scriptable Objects
    private SP_MoveBot moveBot;
    private SP_Characters characters;
    private SP_DrawCard drawCard;
    private SP_Cheating cheating;
    private WriteLog writeLog;

    private ItemType[] equipableItemTypes = new ItemType[] { ItemType.Helmet, ItemType.Armor, ItemType.Leggings, ItemType.MainHandWeapon, ItemType.OffHandWeapon, ItemType.Shield, ItemType.Accessories};

    //Getters & Setters
    public string Name { get { return botName; } }
    public int Level { get { return level; } set { level = value; } }
    public Classes BotClass { get { return botClass; } set { botClass = value; } }
    public Races BotRace { get { return botRace; } set { botRace = value; } }
    public SP_Bot_Attribute[] AttributeList { get { return attributes; } }
    public int Movement { get { return movement; } set { movement = value; } }
    public int AttackPower { get { return attackPower; } }
    public int DrawableCards { get { return drawableCards; } set { drawableCards = value; } }
    public int[] Reputations { get { return reputations; } set { reputations = value; } }
    public int ReputationID { get { return repotationID; } set { repotationID = value; } }
    public bool Cheated { get { return cheated; } set { cheated = value; } }
    public bool Active { get { return active; } set { active = value; } }
    public bool Fighting { get { return fighting; } set { fighting = value; } }
    public bool WouldInterfier { get { return wouldInterfier; } }
    public InventoryObject Inventory { get { return inventory; } }
    public InventoryObject Equipment { get { return equipment; } }
    #endregion
    #region Constructor
    public void Init(string name, string botClass, string botRace, string botAttidute)
    {
        this.botName = name;
        level = 1;
        this.botClass = GetClass(botClass);
        this.botRace = GetRace(botRace);
        this.botAttitude = GetAttidute(botAttidute);
        attributes = new SP_Bot_Attribute[6];
        attributes[0] = new SP_Bot_Attribute(this, Attributes.Strength, 10);
        attributes[1] = new SP_Bot_Attribute(this, Attributes.Agility, 10);
        attributes[2] = new SP_Bot_Attribute(this, Attributes.Intellect, 10);
        attributes[3] = new SP_Bot_Attribute(this, Attributes.Charisma, 10);
        attributes[4] = new SP_Bot_Attribute(this, Attributes.Stamina, 5);
        attributes[5] = new SP_Bot_Attribute(this, Attributes.Health, 6);
        movement = attributes[4].Value.ModifiedValue;
        drawableCards = 4;

        inventory = Instantiate(Resources.Load<InventoryObject>("PlayerInventory"));
        equipment = Instantiate(Resources.Load<InventoryObject>("PlayerEquipment"));

        CalculateAttackPower();

        characters = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCharacters").GetComponent<SP_Characters>();
        moveBot = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerMoveBot").GetComponent<SP_MoveBot>();
        drawCard = GameObject.Find("ScriptObjects/Singleplayer/DrawCard").GetComponent<SP_DrawCard>();
        cheating = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCheat").GetComponent<SP_Cheating>();
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform singleplayer = mainCamera.transform.Find("SinglePlayer");
        Transform log = singleplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();

        GetComponent<ChangeCharacterSkin>().ChangeSkin(this.botClass.ToString());

        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            EquipItem();
        }
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

    private Attitude GetAttidute(string botAttidute)
    {
        switch (botAttidute)
        {
            case "Friendly":
                return Attitude.Friendly;
            case "Netural":
                return Attitude.Netural;
            case "Aggressive":
                return Attitude.Aggressive;
            case "Random":
                return RandomAttidute();
            default:
                return RandomAttidute();
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

    public Attitude RandomAttidute()
    {
        Array values = Enum.GetValues(typeof(Attitude));
        return (Attitude)values.GetValue(UnityEngine.Random.Range(0, values.Length));
    }

    //Setting up the bot reputations
    public void SetReputation(int size)
    {
        int startingReputation = GetReputation();
        reputations = new int[size];
        for (int i = 0; i < size; i++)
        {
            reputations[i] = startingReputation;
        }
    }

    private int GetReputation()
    {
        switch (botAttitude)
        {
            case Attitude.Friendly:
                return 5;
            case Attitude.Netural:
                return 0;
            case Attitude.Aggressive:
                return -5;
            default:
                return 0;
        }
    }

    //Calculating the attack power of the bot
    private void CalculateAttackPower()
    {
        int topAttribute = 0;
        foreach (SP_Bot_Attribute attribute in attributes)
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
        Attributes attribute = GetBestAttribute(botClass);
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

    //Activating the bot's turn
    public void ActivateTurn()
    {
        StopAllCoroutines();
        StartCoroutine(ActivateTurnRoutine());
    }

    private IEnumerator ActivateTurnRoutine()
    {
        cheated = false;
        movement = attributes[4].Value.ModifiedValue;

        while (movement > 0)
        {
            while (fighting)
            {
                yield return null;
            }

            moveBot.MoveBotTowardsPlus();

            yield return null;
        }

        while (fighting)
        {
            yield return null;
        }
        DrawCards();
        CatchCheaters();
        characters.EndTurn();
    }

    //Bot drawing items
    private void DrawCards()
    {
        int draws = drawableCards;
        for (int i = 0; i < draws; i++)
        {
            drawCard.Draw();
            EquipItem();
            if (DrawableCards == 0 && cheating.Cheat)
            {
                drawableCards = Mathf.FloorToInt(UnityEngine.Random.Range(1, 5) / 4);
                if (drawableCards == 1)
                {
                    draws++;
                    cheated = true;
                }
            }
        }
    }

    //Bot trying to equip the drew item
    private void EquipItem()
    {
        Item equipableItem = GetEquipableItem();
        bool used = false;
        if (equipableItem != null)
        {
            List<InventorySlot> slots = SlotsForItem(equipableItem.type);
            foreach (InventorySlot slot in slots)
            {
                if (IsEmptySlot(slot.Item) && !used)
                {
                    slot.Item = equipableItem;
                    used = true;
                    foreach (ItemBuff buff in equipableItem.buffs)
                    {
                        UpdateStats(buff.attribute, buff, "Add");
                        writeLog.WriteNewLog($"{botName} equiped a {equipableItem.name}. {botName}'s new attack power is {attackPower}");
                    }
                }
                else if(!used)
                {
                    if (IsForClass(equipableItem.buffs))
                    {
                        if (FavourableValue(equipableItem.buffs) > FavourableValue(slot.Item.buffs))
                        {
                            foreach (ItemBuff buff in slot.Item.buffs)
                            {
                                UpdateStats(buff.attribute, buff, "Remove");
                            }
                            foreach (ItemBuff buff in equipableItem.buffs)
                            {
                                UpdateStats(buff.attribute, buff, "Add");
                            }
                            writeLog.WriteNewLog($"{botName} swapped {slot.Item.name} to {equipableItem.name}. {botName}'s new attack power is {attackPower}");
                            slot.Item = equipableItem;
                            used = true;
                        }
                    }
                }
            }
            inventory.RemoveItem(equipableItem);
        }
    }

    //Search for an equipable item
    private Item GetEquipableItem()
    {
        foreach (InventorySlot slot in inventory.GetSlots)
        {
            if (equipableItemTypes.Contains(slot.Item.type) && slot.Item.id != -1)
            {
                return slot.Item;
            }
        }
        return null;
    }

    private List<InventorySlot> SlotsForItem(ItemType type)
    {
        List<InventorySlot> slots = new List<InventorySlot>();
        foreach (InventorySlot slot in equipment.GetSlots)
        {
            if (slot.AllowedItems.Contains(type))
            {
                slots.Add(slot);
            }
        }
        return slots;
    }

    private bool IsEmptySlot(Item item)
    {
        return item.id == -1;
    }

    private bool IsForClass(ItemBuff[] buffs)
    {
        Attributes classAttribute = GetBestAttribute(botClass);
        foreach (ItemBuff buff in buffs)
        {
            if (buff.attribute == classAttribute)
            {
                return true;
            }
        }
        return false;
    }

    private int FavourableValue(ItemBuff[] buffs)
    {
        Attributes classAttribute = GetBestAttribute(botClass);
        foreach (ItemBuff buff in buffs)
        {
            if (buff.attribute == classAttribute)
            {
                return buff.value;
            }
        }
        return 0;
    }

    //Updating the bot's stats
    private void UpdateStats(Attributes attribute, ItemBuff value, string action)
    {
        foreach (SP_Bot_Attribute characterAttribute in attributes)
        {
            if (attribute == characterAttribute.Type)
            {
                if (action == "Add")
                {
                    characterAttribute.Value.AddModifier(value);
                }
                else if (action == "Remove")
                {
                    characterAttribute.Value.RemoveModifier(value);
                }
            }
        }
    }

    //Provides the bonus giver attribute of the class
    private Attributes GetBestAttribute(Classes characterClass)
    {
        switch (characterClass)
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

    //The bot decide if catch a cheater character
    private void CatchCheaters()
    {
        for (int i = 0; i < characters.CharacterList.Count; i++)
        {
            if (i != repotationID)
            {
                GameObject character = characters.CharacterList[i].Item2;
                if (character.CompareTag("SP_Player"))
                {
                    SP_Player player = character.GetComponent<SP_Player>();
                    int callCheat = UnityEngine.Random.Range(0, 20) - reputations[i];
                    if (player.Cheated && callCheat > 10)
                    {
                        player.CaughtOnCheating(repotationID);
                    }
                }
                if (character.CompareTag("SP_Bot"))
                {
                    SP_Bot bot = character.GetComponent<SP_Bot>();
                    int callCheat = UnityEngine.Random.Range(0, 20) - reputations[i];
                    if (bot.Cheated && callCheat > 10)
                    {
                        bot.CaughtOnCheating(repotationID);
                    }
                }
            }
        }
    }

    //The bot decide if wants to interfier in the fight
    public void WouldIntervene(GameObject character)
    {
        int plusLevel = 0;
        if (character.CompareTag("SP_Player"))
        {
            plusLevel = character.GetComponent<SP_Player>().Level;
        }
        else if (character.CompareTag("SP_Bot"))
        {
            plusLevel = character.GetComponent<SP_Bot>().Level;
        }
        int id = characters.GetActiveCharacterID();
        if (id != repotationID)
        {
            int wouldDo = UnityEngine.Random.Range(0,20);
            wouldDo -= reputations[id];
            wouldDo += plusLevel;
            if (wouldDo > 10 || wouldDo <= 0)
            {
                wouldInterfier = true;
            }
            else 
            {
                wouldInterfier = false;
            }
        }
    }

    public void Interfier(int monsterAttackPower)
    {
        Item bestItem;
        if (reputations[characters.GetActiveCharacterID()] > 0)
        {
            bestItem = GetBestItem("friend", monsterAttackPower);
        }
        else 
        {
            bestItem = GetBestItem("enemy", monsterAttackPower);
        }
        if (bestItem != null)
        {
            GameObject character = characters.GetActiveCharacter();
            if (character.CompareTag("SP_Player"))
            {
                character.GetComponent<SP_Player>().ApplyBuff(bestItem.buffs, repotationID);
            }
            else if (character.CompareTag("SP_Bot"))
            {
                character.GetComponent<SP_Bot>().ApplyBuff(bestItem.buffs, repotationID);
            }
        }

    }

    //Search for the best item for the situation
    private List<Item> GetPotions(string forWho, Attributes attribute)
    {
        List<Item> items = new List<Item>();
        if (forWho == "friend" || forWho == "itself")
        {
            foreach (InventorySlot slot in inventory.GetSlots)
            {
                if (slot.Item.type == ItemType.Potion)
                {
                    foreach (ItemBuff buff in slot.Item.buffs)
                    {
                        if (attribute == buff.attribute)
                        {
                            items.Add(slot.Item);
                        }
                    }
                }
            }
        }
        if (forWho == "enemy")
        {
            foreach (InventorySlot slot in inventory.GetSlots)
            {
                if (slot.Item.type == ItemType.Debuff)
                {
                    foreach (ItemBuff buff in slot.Item.buffs)
                    {
                        if (attribute == buff.attribute)
                        {
                            items.Add(slot.Item);
                        }
                    }
                }
            }
        }
        return items;
    }

    private Item GetBestItem(string forWho, int monsterAttackPower)
    {
        Attributes attribute = new Attributes();
        GameObject character = characters.GetActiveCharacter();
        int attackpower = 0;
        if (character.CompareTag("SP_Player"))
        {
            attribute = GetBestAttribute(character.GetComponent<SP_Player>().PlayerClass);
            attackpower = character.GetComponent<SP_Player>().AttackPower;
        }
        else if (character.CompareTag("SP_Bot"))
        {
            attribute = GetBestAttribute(character.GetComponent<SP_Bot>().botClass);
            attackpower = character.GetComponent<SP_Bot>().AttackPower;
        }

        List<Item> items = GetPotions(forWho, attribute);
        List<Item> goodItems = new List<Item>();
        foreach (Item item in items)
        {
            foreach (ItemBuff buff in item.buffs)
            {
                if (attribute == buff.attribute && item.type == ItemType.Potion && buff.value + attackpower > monsterAttackPower)
                {
                    goodItems.Add(item);
                }
                else if (attribute == buff.attribute && item.type == ItemType.Debuff && buff.value - attackpower <= monsterAttackPower)
                {
                    goodItems.Add(item);
                }
            }
        }
        return GetMinValueItem(goodItems, attribute);
    }

    private Item GetMinValueItem(List<Item> items, Attributes attribute)
    {
        int min = int.MinValue;
        Item minItem = null;
        foreach (Item item in items)
        {
            foreach (ItemBuff buff in item.buffs)
            {
                if (attribute == buff.attribute && min > buff.value)
                {
                    min = buff.value;
                    minItem = item;
                }
            }
        }
        return minItem;
    }

    //Bot applying an item's buffs
    public void ApplyBuff(ItemBuff[] buffs, int id)
    {
        foreach (ItemBuff buff in buffs)
        {
            foreach (SP_Bot_Attribute attribute in attributes)
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

    //Bot gets punished for cheating
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
        int minusItems = cheating.Punishment;
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
        writeLog.WriteNewLog($"{botName} has been caught on cheating");
    }

    //Update the bot's attackPower if nessesarry
    public void AttributeModified(SP_Bot_Attribute attribute)
    {
        CalculateAttackPower();
        if (fighting)
        {
            SP_MonsterTile tile = GameObject.Find("ScriptObjects/GameBoard").GetComponent<GameBoard>().GetGameObject(transform.position).GetComponent<SP_MonsterTile>();
            tile.UpdateVisuals();
        }
    }
    #endregion
}