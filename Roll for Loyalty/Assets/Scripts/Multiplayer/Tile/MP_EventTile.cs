using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MP_EventTile : MP_Tile
{
    #region Variables
    private List<Action> Events = new List<Action>();
    private MP_Characters characters;
    private WriteLog writeLog;
    #endregion
    #region Constructor
    void Awake()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
        Transform log = multiplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();
        characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();

        AddEvents();
    }
    #endregion
    #region Functions

    //Activateing the tile
    public override void ActivateTile()
    {
        Events[UnityEngine.Random.Range(0, Events.Count)].Invoke();
    }

    //Fill the event list with Events
    private void AddEvents()
    {
        Events.Add(Fog);
        Events.Add(ClassCursed);
        Events.Add(RaceCursed);
        Events.Add(TeleportToDiscoveredTile);
        Events.Add(BlessingOfLevel);
        Events.Add(CurseOfLevel);
        Events.Add(BlessingOfStrength);
        Events.Add(CurseOfStrength);
        Events.Add(BlessingOfAgility);
        Events.Add(CurseOfAgility);
        Events.Add(BlessingOfIntelect);
        Events.Add(CurseOfIntelect);
        Events.Add(BlessingOfCharisma);
        Events.Add(CurseOfCharisma);
    }
    #region Events
    private void Fog()
    {
        MovementCost++;
        WriteLogRpc($"The room is filled with fog. Now to cross this room is cost {MovementCost} stamina");
    }

    private void ClassCursed()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.PlayerClass = player.RandomClass();
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"The room cursed {player.Name}'s class is slowly changing into: {player.PlayerClass}");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.BotClass = bot.RandomClass();
            WriteLogRpc($"The room cursed {bot.Name}'s class is slowly changing into: {bot.BotClass}");
        }
    }

    private void RaceCursed()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.PlayerRace = player.RandomRace();
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"The room cursed {player.Name}'s race is slowly changing into: {player.PlayerRace}");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.BotRace = bot.RandomRace();
            WriteLogRpc($"The room cursed {bot.Name}'s race is slowly changing into: {bot.BotRace}");
        }
    }

    public void TeleportToDiscoveredTile()
    {
        GameObject character = characters.GetActiveCharacter();
        GameBoard gameBoard = GameObject.Find("ScriptObjects/GameBoard").GetComponent<GameBoard>();
        MoveCharacterRpc(character, gameBoard.GetRandomTilePosition());
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            WriteLogRpc($"{player.Name} is teleported to ({player.transform.position.x}, {player.transform.position.y})");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            WriteLogRpc($"{bot.Name} is teleported to ({bot.transform.position.x}, {bot.transform.position.y})");
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void MoveCharacterRpc(NetworkObjectReference characterNetworkObject, Vector3 position)
    {
        if (characterNetworkObject.TryGet(out NetworkObject character))
        {
            character.transform.position = position;
        }
    }

    public void BlessingOfLevel()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            if (player.Level != 9)
            {
                player.Level++;
                player.UpdateStatsVisualClientRpc();
                WriteLogRpc($"{player.Name}'s level has been increesed by 1. {player.Name} is lvl{player.Level}");
            }
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            if (bot.Level != 9)
            {
                bot.Level++;
                WriteLogRpc($"{bot.Name}'s level has been increesed by 1. {bot.Name} is lvl{bot.Level}");
            }
        }
    }

    public void CurseOfLevel()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            if (player.Level != 1)
            {
                player.Level--;
                player.UpdateStatsVisualClientRpc();
                WriteLogRpc($"{player.Name}'s level has been decresed by 1. {player.Name} is lvl{player.Level}");
            }
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            if (bot.Level != 1)
            {
                bot.Level--;
                WriteLogRpc($"{bot.Name}'s level has been decreesed by 1. {bot.Name} is lvl{bot.Level}");
            }
        }
    }

    public void BlessingOfStrength()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[1].Value.BaseValue += 5;
            player.AttributeNetworkList[1] = player.AttributeList[1].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became stronger");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[1].Value.BaseValue += 5;
            WriteLogRpc($"{bot.Name}'s became stronger");
        }
    }

    public void CurseOfStrength()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[1].Value.BaseValue -= 5;
            player.AttributeNetworkList[1] = player.AttributeList[1].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became weaker");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[1].Value.BaseValue -= 5;
            WriteLogRpc($"{bot.Name}'s became weaker");
        }
    }

    public void BlessingOfAgility()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[2].Value.BaseValue += 5;
            player.AttributeNetworkList[2] = player.AttributeList[2].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became quicker");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[2].Value.BaseValue += 5;
            WriteLogRpc($"{bot.Name}'s became quicker");
        }
    }

    public void CurseOfAgility()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[2].Value.BaseValue -= 5;
            player.AttributeNetworkList[2] = player.AttributeList[2].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became slower");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[2].Value.BaseValue -= 5;
            WriteLogRpc($"{bot.Name}'s became slower");
        }
    }

    public void BlessingOfIntelect()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[3].Value.BaseValue += 5;
            player.AttributeNetworkList[3] = player.AttributeList[3].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became smarter");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[3].Value.BaseValue += 5;
            WriteLogRpc($"{bot.Name}'s became smarter");
        }
    }

    public void CurseOfIntelect()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[3].Value.BaseValue -= 5;
            player.AttributeNetworkList[3] = player.AttributeList[3].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became dumber");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[3].Value.BaseValue -= 5;
            WriteLogRpc($"{bot.Name}'s became dumber");
        }
    }

    public void BlessingOfCharisma()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[4].Value.BaseValue += 5;
            player.AttributeNetworkList[4] = player.AttributeList[4].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became more charismatic");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[4].Value.BaseValue += 5;
            WriteLogRpc($"{bot.Name}'s became more charismatic");
        }
    }

    public void CurseOfCharisma()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.AttributeList[4].Value.BaseValue -= 5;
            player.AttributeNetworkList[4] = player.AttributeList[4].Value.ModifiedValue;
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"{player.Name}'s became less appealing");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.AttributeList[4].Value.BaseValue -= 5;
            WriteLogRpc($"{bot.Name}'s became less appealing");
        }
    }
    #endregion

    //Write in the log for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void WriteLogRpc(string message)
    {
        if (writeLog == null)
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
            Transform log = multiplayer.Find("Log");
            Transform viewport = log.Find("Viewport");
            writeLog = viewport.Find("Content").GetComponent<WriteLog>();
        }
        writeLog.WriteNewLog(message);
    }
    #endregion
}