using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SP_GameStart : MonoBehaviour
{
    #region Variables
    //Scripts
    [SerializeField] private ToggleVisibility toggleVisibility;
    [SerializeField] private SpawnGameObject spawnObject;
    [SerializeField] private SP_Characters characters;
    [SerializeField] private SP_TileScript tileScript;
    [SerializeField] private SP_Plus plus;
    [SerializeField] private WriteLog writeLog;

    //Scenes
    [SerializeField] private GameObject singleplayerMenu;
    [SerializeField] private GameObject gameField;

    //GameObjects & Prefabs
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject bot;

    private Vector3 firstPosition =Vector3.zero;
    #endregion
    #region Functions

    //Starting the singleplayer game
    public void StartSinglePlayer()
    {
        SetupSingleplayer();
        ShowGameboard();
        characters.ActivateCharacter();
    }

    //Setup player and bots provided by SinglePlayerMenu
    private void SetupSingleplayer()
    {
        SetupPlayer();
        SetupBots();
        tileScript.CreateTile(firstPosition, TileTypes.EmptyRoom, new bool[] { true, false, true, true });
        plus.CreateDirections(firstPosition);
        characters.SetUpCharacters();
    }

    private void SetupPlayer()
    {
        List<string> playerInfo = GetPlayerStartingInformations();
        GameObject newPlayer = spawnObject.SpawnNewObject(player, firstPosition, "Player", gameField);
        newPlayer.GetComponent<SP_Player>().Init(playerInfo[0], playerInfo[1], playerInfo[2]);
        newPlayer.transform.GetChild(0).GetComponent<TextMeshPro>().text = playerInfo[0];
        characters.AddCharacter(newPlayer.GetComponent<SP_Player>().Name, newPlayer);
    }
    private List<string> GetPlayerStartingInformations()
    {
        GameObject player = GameObject.FindWithTag("PlayerCreation");
        string name = player.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        string playerclass = player.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string playerrace = player.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        return new List<string> { name, playerclass, playerrace };
    }

    private void SetupBots()
    {
        List<List<string>> botsInfo = GetBotsStartingInformations();
        for (int i = 0; i < botsInfo.Count; i++)
        {
            GameObject newBot = spawnObject.SpawnNewObject(bot, firstPosition, "Bot", gameField);
            newBot.GetComponent<SP_Bot>().Init($"Bot{i}", botsInfo[i][0], botsInfo[i][1], botsInfo[i][2]);
            newBot.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"Bot{i}";
            characters.AddCharacter(newBot.GetComponent<SP_Bot>().Name, newBot);
        }
    }

    private List<List<string>> GetBotsStartingInformations()
    {
        List<List<string>> botsinfo = new List<List<string>>();
        GameObject[] bots = GameObject.FindGameObjectsWithTag("BotCreation");
        foreach (GameObject bot in bots)
        {
            List<string> botinfo = new List<string>();
            botinfo.Add(bot.transform.GetChild(1).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            botinfo.Add(bot.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            botinfo.Add(bot.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text);
            botsinfo.Add(botinfo);
        }
        return botsinfo;
    }

    //Update the visual for the player
    private void ShowGameboard()
    {
        toggleVisibility.ChangeObjects(singleplayerMenu, gameField);
        writeLog.WriteNewLog("Singleplayer started");
        writeLog.WriteNewLog("Game order: " + string.Join(" -> ", characters.GetGameOrder()));
    }
    #endregion
}