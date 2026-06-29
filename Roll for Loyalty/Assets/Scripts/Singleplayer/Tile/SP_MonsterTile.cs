using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SP_MonsterTile : SP_Tile
{
    #region Variables
    //Scriptable Objects
    private Monster monster;
    private SP_Characters characters;
    private SP_Player player;
    private SP_Bot bot;
    private WriteLog writeLog;
    //Scene
    private GameObject fightScene;
    private GameObject fightingRoom;
    //prefab
    private GameObject character;

    private List<Action> LoseEvent = new List<Action>();
    #endregion
    #region Functions
    //Activateing the tile
    public override void ActivateTile()
    {
        AddEvents();

        characters = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCharacters").GetComponent<SP_Characters>();
        character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            player = character.GetComponent<SP_Player>();
            player.Fighting = true;
            monster = new Monster(player.AttackPower);
        }
        else if (character.CompareTag("SP_Bot"))
        {
            bot = character.GetComponent<SP_Bot>();
            bot.Fighting = true;
            monster = new Monster(bot.AttackPower);
        }

        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform singleplayer = mainCamera.transform.Find("SinglePlayer");
        Transform log = singleplayer.Find("Log");
        Transform viewport = log.Find("Viewport");
        writeLog = viewport.Find("Content").GetComponent<WriteLog>();

        fightScene = Resources.Load<GameObject>("SingleplayerFightScene");

        WouldBotsInterfiere();

        CreateFightScene();
        UpdateVisuals();

        StopAllCoroutines();
        StartCoroutine(FightTimerWithUI(5f));
    }

    //Every bot decide if they wanna interfier in the battle
    private void WouldBotsInterfiere()
    {
        foreach ((string name, GameObject character) otherCharacter in characters.CharacterList)
        {
            if (otherCharacter.character.CompareTag("SP_Bot") && otherCharacter.character != character)
            {
                SP_Bot otherBot = otherCharacter.character.GetComponent<SP_Bot>();
                otherBot.WouldIntervene(character);
            }
        }

    }

    //Creating, updateing the visuals
    private void CreateFightScene()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        if (player != null)
        {
            Transform endTrunTransform = mainCamera.transform.Find("SinglePlayer/End Turn");
            GameObject endTurn = endTrunTransform.gameObject;
            endTurn.SetActive(false);
        }
        fightingRoom = Instantiate(fightScene, new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0), Quaternion.identity);
        fightingRoom.transform.SetParent(mainCamera.transform);
    }

    public void UpdateVisuals()
    {
        int attackPower = 0;
        if (player != null)
        {
            attackPower = player.AttackPower;
        }
        else if (bot != null)
        {
            attackPower = bot.AttackPower;
        }

        fightingRoom.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>().sprite = character.GetComponent<SpriteRenderer>().sprite;
        fightingRoom.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = attackPower.ToString();
        fightingRoom.transform.GetChild(1).GetChild(0).GetComponent<SpriteRenderer>().sprite = monster.MonsterSprite;
        fightingRoom.transform.GetChild(1).GetChild(1).GetComponent<TextMeshProUGUI>().text = monster.AttackPower.ToString();

        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character.CompareTag("SP_Bot"))
            {
                characterinfo.character.GetComponent<SP_Bot>().Interfier(monster.AttackPower);
            }
        }
    }

    //Timer
    private IEnumerator FightTimerWithUI(float time)
    {
        TextMeshProUGUI timerText = fightingRoom.transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        float remaining = time;
        while (remaining > 0f)
        {
            timerText.text = Mathf.CeilToInt(remaining).ToString();
            remaining -= Time.deltaTime;
            yield return null;
        }

        timerText.text = "0";
        fight();
        if (player != null)
        {
            GameObject mainCamera = GameObject.Find("Main Camera");
            Transform endTrunTransform = mainCamera.transform.Find("SinglePlayer/End Turn");
            GameObject endTurn = endTrunTransform.gameObject;
            endTurn.SetActive(true);
        }
        Destroy(fightingRoom);
    }

    // Deciding the winner and the consequences
    private void fight()
    {
        ItemDataBaseObject items = Resources.Load<ItemDataBaseObject>("Items");
        if (player != null)
        {
            if (player.AttackPower > monster.AttackPower)
            {
                player.Inventory.AddItem(new Item(items.itemObjects[0]), monster.Reward);
                player.Level++;
                player.DrawableCards++;
                writeLog.WriteNewLog($"{player.Name} is now {player.Level} lvl by defeating the monster.");
                if (player.Level == 10)
                {
                    Destroy(fightingRoom);
                    SP_EndGame endGame = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerEndGame").GetComponent<SP_EndGame>();
                    endGame.Won(player.Name);
                }
            }
            else
            {
                writeLog.WriteNewLog($"{player.Name} defeated by the monster.");
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
                writeLog.WriteNewLog($"{bot.Name} is now {bot.Level} lvl by defeating the monster.");
                if (bot.Level == 10)
                {
                    Destroy(fightingRoom);
                    SP_EndGame endGame = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerEndGame").GetComponent<SP_EndGame>();
                    endGame.Won(bot.Name);
                }
            }
            else
            {
                writeLog.WriteNewLog($"{bot.Name} defeated by the monster.");
                LoseEvent[UnityEngine.Random.Range(0, LoseEvent.Count)].Invoke();
            }
            bot.Fighting = false;
        }
    }

    //Fill the event list with Events
    private void AddEvents()
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
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.PlayerClass = player.RandomClass();
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"The monster cursed {player.Name}'s class is slowly changing into: {player.PlayerClass}");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.BotClass = bot.RandomClass();
            writeLog.WriteNewLog($"The monster cursed {bot.Name}'s class is slowly changing into: {bot.BotClass}");
        }
    }
    private void RaceCursed()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.PlayerRace = player.RandomRace();
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"The monster cursed {player.Name}'s race is slowly changing into: {player.PlayerRace}");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.BotRace = bot.RandomRace();
            writeLog.WriteNewLog($"The monster cursed {bot.Name}'s race is slowly changing into: {bot.BotRace}");
        }
    }
    public void CurseOfLevel()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            if (player.Level != 1)
            {
                player.Level--;
                player.UpdateStatsVisual();
                writeLog.WriteNewLog($"{player.Name}'s level has been decresed by 1. {player.Name} is lvl{player.Level}");
            }
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            if (bot.Level != 1)
            {
                bot.Level--;
                writeLog.WriteNewLog($"{bot.Name}'s level has been decreesed by 1. {bot.Name} is lvl{bot.Level}");
            }
        }
    }
    public void CurseOfStrength()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.AttributeList[1].Value.BaseValue -= 5;
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"{player.Name}'s became weaker");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.AttributeList[1].Value.BaseValue -= 5;
            writeLog.WriteNewLog($"{bot.Name}'s became weaker");
        }
    }
    public void CurseOfAgility()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.AttributeList[2].Value.BaseValue -= 5;
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"{player.Name}'s became slower");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.AttributeList[2].Value.BaseValue -= 5;
            writeLog.WriteNewLog($"{bot.Name}'s became slower");
        }
    }
    public void CurseOfIntelect()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.AttributeList[3].Value.BaseValue -= 5;
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"{player.Name}'s became dumber");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.AttributeList[3].Value.BaseValue -= 5;
            writeLog.WriteNewLog($"{bot.Name}'s became dumber");
        }
    }
    public void CurseOfCharisma()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            player.AttributeList[4].Value.BaseValue -= 5;
            player.UpdateStatsVisual();
            writeLog.WriteNewLog($"{player.Name}'s became less appealing");
        }
        else if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            bot.AttributeList[4].Value.BaseValue -= 5;
            writeLog.WriteNewLog($"{bot.Name}'s became less appealing");
        }
    }
    #endregion
    #endregion
}