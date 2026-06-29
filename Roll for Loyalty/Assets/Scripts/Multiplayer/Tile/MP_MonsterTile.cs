using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class MP_MonsterTile : MP_Tile
{
    #region Variable
    //Scriptable Object
    private Monster monster;
    private MP_Characters characters;
    private MP_Player player;
    private MP_Bot bot;
    private WriteLog writeLog;
    //Scene
    private GameObject fightScene;
    private GameObject fightingRoom;
    // prefab
    private GameObject character;

    private int attackPower;
    private List<Action> LoseEvent = new List<Action>();
    #endregion
    #region Function
    //Activateing the tile
    public override void ActivateTile()
    {
        AddEvent();

        characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            player = character.GetComponent<MP_Player>();
            player.Fighting = true;
            attackPower = player.AttackPower;
            monster = new Monster(player.AttackPower);
        }
        else if (character.CompareTag("MP_Bot"))
        {
            bot = character.GetComponent<MP_Bot>();
            bot.Fighting = true;
            attackPower = bot.AttackPower;
            monster = new Monster(bot.AttackPower);
        }

        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayer");
        Transform log = multiplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();

        fightScene = Resources.Load<GameObject>("MultiplayerFightScene");

        WouldBotsInterfiere();

        CreateFightScene();
        SetScenePositionRpc(fightingRoom);
        UpdateFightRpc();

        StopAllCoroutines();
        StartFightTimerRpc(5f);
    }

    //Every bot decide if they wanna interfier in the battle
    private void WouldBotsInterfiere()
    {
        foreach ((string name, GameObject character) otherCharacter in characters.CharacterList)
        {
            if (otherCharacter.character.CompareTag("MP_Bot") && otherCharacter.character != character)
            {
                MP_Bot otherBot = otherCharacter.character.GetComponent<MP_Bot>();
                if (otherBot != null)
                {
                    otherBot.WouldIntervene(character);
                }
            }
        }
    }

    //Creating, updateing the visuals
    private void CreateFightScene()
    {

        GameObject mainCamera = GameObject.Find("Main Camera");
        if (player != null)
        {
            Transform endTurnTransform = mainCamera.transform.Find("MultiPlayer/End Turn");
            if (endTurnTransform != null)
            {
                GameObject endTurn = endTurnTransform.gameObject;
                endTurn.SetActive(false);
            }
        }
        fightingRoom = Instantiate(fightScene, Vector3.zero, Quaternion.identity);
        fightingRoom.name = "FightScene";
        fightingRoom.GetComponent<NetworkObject>().Spawn();
    }

    //Set the Fighscene position 
    [Rpc(SendTo.ClientsAndHost)]
    private void SetScenePositionRpc(NetworkObjectReference fightSceneRef)
    {
        if (!fightSceneRef.TryGet(out NetworkObject fightscene))
        {
            Debug.LogError("UpdateVisualsRpc: fight scene reference could not be resolved.");
            return;
        }
        GameObject mainCamera = GameObject.Find("Main Camera");
        fightscene.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0);
    }

    //Creating, updateing the visuals
    [Rpc(SendTo.Server)]
    public void UpdateFightRpc()
    {
        if (character.CompareTag("MP_Player"))
        {
            player = character.GetComponent<MP_Player>();
            attackPower = player.AttackPower;
        }
        else if (character.CompareTag("MP_Bot"))
        {
            bot = character.GetComponent<MP_Bot>();
            attackPower = bot.AttackPower;
        }
        UpdateVisualsRpc(fightingRoom, character, attackPower.ToString(), monster.AttackPower.ToString());
        BotInterfiereRpc();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateVisualsRpc(NetworkObjectReference fightSceneRef, NetworkObjectReference characterRef, string attackPower, string monsterattackPower)
    {
        if (!fightSceneRef.TryGet(out NetworkObject fightscene))
        {
            Debug.LogError("UpdateVisualsRpc: fight scene reference could not be resolved.");
            return;
        }

        if (!characterRef.TryGet(out NetworkObject character))
        {
            Debug.LogError("UpdateVisualsRpc: character reference could not be resolved.");
            return;
        }

        fightscene.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = character.GetComponent<SpriteRenderer>().sprite;
        fightscene.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = attackPower;
        fightscene.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Monster");
        fightscene.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = monsterattackPower;
    }

    [Rpc(SendTo.Server)]
    private void BotInterfiereRpc()
    {
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character.CompareTag("MP_Bot"))
            {
                characterinfo.character.GetComponent<MP_Bot>().Interfier(monster.AttackPower);
            }
        }
    }

    //Timer
    [Rpc(SendTo.Server)]
    private void StartFightTimerRpc(float time)
    {
        StopAllCoroutines();
        StartCoroutine(FightTimerCoroutine(time));
    }

    private IEnumerator FightTimerCoroutine(float time)
    {
        float remaining = time;
        while (remaining > 0f)
        {
            UpdateTimerRpc(fightingRoom, remaining);
            remaining -= Time.deltaTime;
            yield return null;
        }

        UpdateTimerRpc(fightingRoom, 0);

        Fight();

        fightingRoom.GetComponent<NetworkObject>().Despawn();
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void UpdateTimerRpc(NetworkObjectReference fightSceneRef, float time)
    {
        if (!fightSceneRef.TryGet(out NetworkObject fightscene))
        {
            Debug.LogError("UpdateVisualsRpc: fight scene reference could not be resolved.");
            return;
        }
        fightscene.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = Mathf.CeilToInt(time).ToString();
    }

    // Deciding the winner and the consequences
    private void Fight()
    {
        ItemDataBaseObject items = Resources.Load<ItemDataBaseObject>("Items");
        if (items == null)
        {
            Debug.LogError("Fight: Items database not found in Resources.");
            return;
        }

        if (player != null)
        {
            if (player.AttackPower > monster.AttackPower)
            {
                player.Inventory.AddItem(new Item(items.itemObjects[0]), monster.Reward);
                player.Level++;
                player.DrawableCards++;
                player.UpdateStatsVisualClientRpc();
                WriteLogRpc($"{player.Name} is now {player.Level} lvl by defeating the monster.");
                if (player.Level == 10)
                {
                    MP_Endgame endGame = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerEndGame").GetComponent<MP_Endgame>();
                    endGame.WonRpc(player.Name);
                }
            }
            else
            {
                WriteLogRpc($"{player.Name} defeated by the monster.");
                LoseEvent[UnityEngine.Random.Range(0, LoseEvent.Count)].Invoke();
            }

            player.Fighting = false;
        }
        else if (bot != null)
        {
            if (bot.AttackPower > monster.AttackPower)
            {
                bot.Inventory.AddItem(new Item(items.itemObjects[0]), monster.Reward);
                bot.Level++;
                bot.DrawableCards++;
                WriteLogRpc($"{bot.Name} is now {bot.Level} lvl by defeating the monster.");
                if (bot.Level == 10)
                {
                    MP_Endgame endGame = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerEndGame").GetComponent<MP_Endgame>();
                    endGame.WonRpc(player.Name);
                }
            }
            else
            {
                WriteLogRpc($"{bot.Name} defeated by the monster.");
                LoseEvent[UnityEngine.Random.Range(0, LoseEvent.Count)].Invoke();
            }

            bot.Fighting = false;
        }
    }

    //Fill the event list with Events
    private void AddEvent()
    {
        LoseEvent.Add(ClassCursed);
        LoseEvent.Add(RaceCursed);
        LoseEvent.Add(CurseOfLevel);
        LoseEvent.Add(CurseOfStrength);
        LoseEvent.Add(CurseOfAgility);
        LoseEvent.Add(CurseOfIntelect);
        LoseEvent.Add(CurseOfCharisma);
    }
    #region Events
    private void ClassCursed()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            player.PlayerClass = player.RandomClass();
            player.UpdateStatsVisualClientRpc();
            WriteLogRpc($"The monster cursed {player.Name}'s class is slowly changing into: {player.PlayerClass}");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.BotClass = bot.RandomClass();
            WriteLogRpc($"The monster cursed {bot.Name}'s class is slowly changing into: {bot.BotClass}");
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
            WriteLogRpc($"The monster cursed {player.Name}'s race is slowly changing into: {player.PlayerRace}");
        }
        else if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            bot.BotRace = bot.RandomRace();
            WriteLogRpc($"The monster cursed {bot.Name}'s race is slowly changing into: {bot.BotRace}");
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