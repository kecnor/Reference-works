using System.IO;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveLoad : MonoBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private SP_Cheating cheat;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private SpawnGameObject spawnObject;
    [SerializeField] private SP_Characters characters;
    [SerializeField] private ItemDataBaseObject items;
    [SerializeField] private SP_TileScript tileScript;
    [SerializeField] private SP_Plus plus;
    [SerializeField] private UserInterface parentInventory;
    [SerializeField] private UserInterface parentEquipment;
    [SerializeField] private WriteLog writeLog;
    //Characters
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private GameObject botPrefab;
    //Scene
    [SerializeField] private GameObject gameField;

    private string saveFileName = "save.txt";
    #endregion
    #region Save
    //Saving the game's characters, board and game settings
    public void SaveFile()
    {
        List<string> saveLines = new List<string>();

        saveLines.Add("Cheating");
        saveLines.Add(cheat.Cheat.ToString());
        saveLines.Add("GameBoard");
        saveLines.AddRange(CreateGameBoardList());

        saveLines.Add("Characters");
        foreach ((string, GameObject) charactersInfo in characters.CharacterList)
        {
            if (charactersInfo.Item2.CompareTag("SP_Player"))
            {
                saveLines.Add("Player");
                saveLines.AddRange(CreatePlayerList(charactersInfo.Item2));
            }
            else if (charactersInfo.Item2.CompareTag("SP_Bot"))
            {
                saveLines.Add("Bot");
                saveLines.AddRange(CreateBotList(charactersInfo.Item2));
            }
        }
        string path = Path.Combine(Application.dataPath, saveFileName);
        File.WriteAllLines(path, saveLines);
        writeLog.WriteNewLog($"SuccessFull save at {path}");
    }
    
    //Extract the board pieces's informations
    private List<string> CreateGameBoardList()
    {
        List<string> boardPieces = new List<string>();
        foreach (KeyValuePair<Vector3, GameObject> boardPiece in gameBoard.GameBoardPieces)
        {
            if (boardPiece.Value.CompareTag("Tile"))
            {
                SP_Tile tile = boardPiece.Value.GetComponent<SP_Tile>();
                boardPieces.Add($"Tile:{boardPiece.Key.x},{boardPiece.Key.y},{boardPiece.Key.z}:{tile.TileType}:{tile.MovementCost}:{tile.NorthDoor}:{tile.EastDoor}:{tile.SouthDoor}:{tile.WestDoor}");
            }
            else if (boardPiece.Value.CompareTag("Plus"))
            {
                boardPieces.Add($"Plus:{boardPiece.Key.x},{boardPiece.Key.y},{boardPiece.Key.z}");
            }
        }
        return boardPieces;
    }

    //Extract the player's informations
    private List<string> CreatePlayerList(GameObject character)
    {
        List<string> player = new List<string>();
        SP_Player playerInfo = character.GetComponent<SP_Player>();
        player.Add($"{playerInfo.transform.position.x},{playerInfo.transform.position.y},{playerInfo.transform.position.z}");
        player.Add(playerInfo.Name);
        player.Add(playerInfo.Level.ToString());
        player.Add(playerInfo.PlayerClass.ToString());
        player.Add(playerInfo.PlayerRace.ToString());
        player.Add(playerInfo.DrawableCards.ToString());
        player.Add(playerInfo.Active.ToString());
        player.Add(playerInfo.Cheated.ToString());
        string attributes = "";
        foreach (SP_Attribute attribute in playerInfo.AttributeList)
        {
            attributes += $"{attribute.Value.BaseValue.ToString()}:";
        }
        player.Add(attributes);
        string inventoryItems = "";
        foreach (InventorySlot slot in playerInfo.Inventory.GetSlots)
        {
            inventoryItems += $"{slot.Item.ID},{slot.Amount}:";
        }
        player.Add(inventoryItems);
        string equipmentItems = "";
        foreach (InventorySlot slot in playerInfo.Equipment.GetSlots)
        {
            equipmentItems += $"{slot.Item.ID},{slot.Amount}:";
        }
        player.Add(equipmentItems);
        player.Add(playerInfo.ReputationID.ToString());
        player.Add(string.Join(":", playerInfo.Reputations));
        return player;
    }

    //Extract the bot's informations
    private List<string> CreateBotList(GameObject character)
    {
        List<string> bot = new List<string>();
        SP_Bot botInfo = character.GetComponent<SP_Bot>();
        bot.Add($"{botInfo.transform.position.x},{botInfo.transform.position.y},{botInfo.transform.position.z}");
        bot.Add(botInfo.Name);
        bot.Add(botInfo.Level.ToString());
        bot.Add(botInfo.BotClass.ToString());
        bot.Add(botInfo.BotRace.ToString());
        bot.Add(botInfo.DrawableCards.ToString());
        bot.Add(botInfo.Active.ToString());
        bot.Add(botInfo.Cheated.ToString());
        string attributes = "";
        foreach (SP_Bot_Attribute attribute in botInfo.AttributeList)
        {
            attributes += $"{attribute.Value.BaseValue.ToString()}:";
        }
        bot.Add(attributes);
        string inventoryItems = "";
        foreach (InventorySlot slot in botInfo.Inventory.GetSlots)
        {
            inventoryItems += $"{slot.Item.ID},{slot.Amount}:";
        }
        bot.Add(inventoryItems);
        string equipmentItems = "";
        foreach (InventorySlot slot in botInfo.Equipment.GetSlots)
        {
            equipmentItems += $"{slot.Item.ID},{slot.Amount}:";
        }
        bot.Add(equipmentItems);
        bot.Add(botInfo.ReputationID.ToString());
        bot.Add(string.Join(":", botInfo.Reputations));
        return bot;
    }

    #endregion
    #region Load
    //Loading the game's characters, board and game settings
    public void LoadFile()
    {
        string path = Path.Combine(Application.dataPath, saveFileName);
        if (!File.Exists(path))
        {
            Debug.Log("No save file found.");
            return;
        }

        string[] lines = File.ReadAllLines(path);
        int counter = 1;

        cheat.Cheat = ConvertBool(lines[counter]);

        counter = 3;
        for (int i = counter; lines[i] != "Characters"; i++)
        {
            string[] boardPiece = lines[i].Split(":");

            string[] values = boardPiece[1].Split(',');
            Vector3 koordinates = new Vector3(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));

            if (boardPiece[0] == "Tile")
            {
                TileTypes type = GetTileEype(boardPiece[2]);
                bool[] doors = new bool[4];
                for (int j = 0; j < 4; j++)
                {
                    doors[j] = ConvertBool(boardPiece[4+j]);
                }
                tileScript.CreateTile(koordinates, type, doors);
                gameBoard.GetGameObject(koordinates).GetComponent<SP_Tile>().MovementCost = int.Parse(boardPiece[3]); 
            }
            if (boardPiece[0] == "Plus")
            {
                plus.CreatePlus(koordinates);
            }
            counter++;
        }

        counter++;
        for (int i = counter; i < lines.Length; i+= 14)
        {
            if (lines[i] == "Player")
            {
                string[] values = lines[i+1].Split(',');
                Vector3 koordinates = new Vector3(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));

                GameObject newPlayer = spawnObject.SpawnNewObject(playerPrefab, koordinates, "Player", gameField);
                SP_Player player = newPlayer.GetComponent<SP_Player>();
                newPlayer.transform.GetChild(0).GetComponent<TextMeshPro>().text = lines[i + 2];
                player.Init(lines[i+2], lines[i + 4], lines[i + 5]);
                player.Level = int.Parse(lines[i + 3]);
                player.DrawableCards = int.Parse(lines[i + 6]);
                player.Active = ConvertBool(lines[i + 7]);
                player.Cheated = ConvertBool(lines[i + 8]);
                string[] attributes = lines[i + 9].Split(':');
                for (int j = 0; j < player.AttributeList.Length; j++)
                {
                    player.AttributeList[j].Value.BaseValue = int.Parse(attributes[j]);
                }
                parentInventory.BindInventory(player.Inventory);
                string[] inventoryItems = lines[i + 10].Split(':');
                for (int j = 0; j < player.Inventory.GetSlots.Length; j++)
                {
                    int itemID = int.Parse(inventoryItems[j].Split(',')[0]);
                    int amount = int.Parse(inventoryItems[j].Split(',')[1]);
                    if (itemID != -1)
                    {
                        player.Inventory.GetSlots[j].LoadSlot(new Item(items.GetItem(itemID)), amount);
                    }
                }
                parentEquipment.BindInventory(player.Equipment);
                string[] equipmentItems = lines[i + 11].Split(':');
                for (int j = 0; j < player.Equipment.GetSlots.Length; j++)
                {
                    int itemID = int.Parse(equipmentItems[j].Split(',')[0]);
                    int amount = int.Parse(equipmentItems[j].Split(',')[1]);
                    if (itemID != -1)
                    {
                        player.Equipment.GetSlots[j].LoadSlot(new Item(items.GetItem(itemID)), amount);
                    }
                }
                player.ReputationID = int.Parse(lines[i+12]);
                string[] reputations = lines[i + 13].Split(':');
                player.Reputations = new int[reputations.Length];
                for (int j = 0; j < player.Reputations.Length; j++)
                {
                    player.Reputations[j] = int.Parse(reputations[j]);
                }
                characters.AddCharacter(player.Name, player.gameObject);
                player.UpdateStatsVisual();
            }
            if (lines[i] == "Bot")
            {
                string[] values = lines[i + 1].Split(',');
                Vector3 koordinates = new Vector3(int.Parse(values[0]), int.Parse(values[1]), int.Parse(values[2]));

                GameObject newBot = spawnObject.SpawnNewObject(botPrefab, koordinates, "Bot", gameField);
                SP_Bot bot = newBot.GetComponent<SP_Bot>();
                newBot.transform.GetChild(0).GetComponent<TextMeshPro>().text = lines[i + 2];
                bot.Init(lines[i + 2], lines[i + 4], lines[i + 5], "netural");
                bot.Level = int.Parse(lines[i + 3]);
                bot.DrawableCards = int.Parse(lines[i + 6]);
                bot.Active = ConvertBool(lines[i + 7]);
                bot.Cheated = ConvertBool(lines[i + 8]);
                string[] attributes = lines[i + 9].Split(':');
                for (int j = 0; j < bot.AttributeList.Length; j++)
                {
                    bot.AttributeList[j].Value.BaseValue = int.Parse(attributes[j]);
                }
                string[] inventoryItems = lines[i + 10].Split(':');
                for (int j = 0; j < bot.Inventory.GetSlots.Length; j++)
                {
                    int itemID = int.Parse(inventoryItems[j].Split(',')[0]);
                    int amount = int.Parse(inventoryItems[j].Split(',')[1]);
                    if (itemID != -1)
                    {
                        bot.Inventory.AddItem(new Item(items.GetItem(itemID)), amount);
                    }
                }
                string[] equipmentItems = lines[i + 11].Split(':');
                for (int j = 0; j < bot.Equipment.GetSlots.Length; j++)
                {
                    int itemID = int.Parse(equipmentItems[j].Split(',')[0]);
                    int amount = int.Parse(equipmentItems[j].Split(',')[1]);
                    if (itemID != -1)
                    {
                        bot.Equipment.AddItem(new Item(items.GetItem(itemID)), amount);
                    }
                }
                bot.ReputationID = int.Parse(lines[i + 12]);
                string[] reputations = lines[i + 13].Split(':');
                bot.Reputations = new int[reputations.Length];
                for (int j = 0; j < bot.Reputations.Length; j++)
                {
                    bot.Reputations[j] = int.Parse(reputations[j]);
                }
                characters.AddCharacter(bot.Name, bot.gameObject);
            }
        }
    }

    //Convert string into a bool
    private bool ConvertBool(string text)
    {
        switch (text)
        {
            case "True":
                return true;
            case "False":
                return false;
            default:
                return false;
        }
    }

    //Convert string into a TileType
    private TileTypes GetTileEype(string type)
    {
        switch (type)
        {
            case "MonsterRoom":
                return TileTypes.MonsterRoom;
            case "TreasureRoom":
                return TileTypes.TreasureRoom;
            case "EventRoom":
                return TileTypes.EventRoom;
            case "EmptyRoom":
                return TileTypes.EmptyRoom;
            case "Hallway":
                return TileTypes.Hallway;
            default:
                return TileTypes.EmptyRoom;
        }
    }
    #endregion
}