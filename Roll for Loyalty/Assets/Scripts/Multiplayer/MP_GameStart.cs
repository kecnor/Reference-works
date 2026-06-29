using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Unity.Netcode;

public class MP_GameStart : NetworkBehaviour
{
    #region Variables
    //Scripts
    [SerializeField] private ToggleVisibility toggleVisibility;
    [SerializeField] private SpawnNetworkObject spawnObject;
    [SerializeField] private MP_Characters characters;
    [SerializeField] private MP_TileScript tileScript;
    [SerializeField] private MP_Plus plus;
    [SerializeField] private MP_Cheating cheating;
    [SerializeField] private WriteLog writeLog;
    //Scenes
    [SerializeField] private GameObject multiplayerHostMenu;
    [SerializeField] private GameObject multiplayerClientMenu;
    [SerializeField] private GameObject gameField;
    [SerializeField] private GameObject playerDisplay;
    //GameObjects & Prefabs
    [SerializeField] private NetworkObject player;
    [SerializeField] private NetworkObject bot;
    [SerializeField] private NetworkObject draw;
    [SerializeField] private NetworkObject endturn;
    [SerializeField] private NetworkObject endgame;

    List<List<string>> botsInfo;
    Vector3 firstPosition = new Vector3(0, 0, 0);

    private int playerCount;
    private int joinedPlayers;
    #endregion
    #region Functions

    //Starting the singleplayer game
    public void StartMultiPlayer()
    {
        playerCount = NetworkManager.Singleton.ConnectedClients.Count;
        joinedPlayers = 0;
        SetupBotsRpc();
        MultiplayerSetupRpc();
        ShowGameBoardToAllRpc();
    }

    //Setup player and bots provided by the clients
    [Rpc(SendTo.ClientsAndHost)] 
    private void MultiplayerSetupRpc() 
    {
        GameObject playerinfo = GameObject.FindWithTag("PlayerCreation");
        string name = playerinfo.transform.GetChild(1).transform.GetChild(0).transform.GetChild(2).GetComponent<TextMeshProUGUI>().text;
        string playerclass = playerinfo.transform.GetChild(2).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string playerrace = playerinfo.transform.GetChild(3).transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        CreatePlayerRpc(name, playerclass, playerrace, NetworkManager.Singleton.LocalClientId);
    }

    //Creating players on the server
    [Rpc(SendTo.Server)]
    private void CreatePlayerRpc(string name, string playerclass, string playerrace, ulong clientId)
    {
        GameObject newPlayer = spawnObject.SpawnNewObject(player, firstPosition, name, gameField.GetComponent<NetworkObject>());
        newPlayer.GetComponent<NetworkObject>().SpawnWithOwnership(clientId);
        newPlayer.GetComponent<MP_Player>().Init(name, playerclass, playerrace);
        newPlayer.transform.GetChild(0).GetComponent<TextMeshPro>().text = name;
        characters.AddCharacter(newPlayer.GetComponent<MP_Player>().Name, newPlayer.gameObject);
    }

    //Creating bots on the server
    [Rpc(SendTo.Server)]
    private void SetupBotsRpc()
    {
        botsInfo = GetBotsStartingInformations();
        for (int i = 0; i < botsInfo.Count; i++)
        {
            GameObject newBot = spawnObject.SpawnNewObject(bot, firstPosition, $"Bot{i}", gameField.GetComponent<NetworkObject>());
            newBot.GetComponent<NetworkObject>().Spawn();
            newBot.GetComponent<MP_Bot>().Init($"Bot{i}", botsInfo[i][0], botsInfo[i][1], botsInfo[i][2]);
            newBot.transform.GetChild(0).GetComponent<TextMeshPro>().text = $"Bot{i}";
            characters.AddCharacter(newBot.GetComponent<MP_Bot>().Name, newBot);
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

    //Changes the Gamescene to GameBoard for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void ShowGameBoardToAllRpc()
    {
        if (IsHost)
        {
            toggleVisibility.ChangeObjects(multiplayerHostMenu, gameField);
            GameObject endGame = spawnObject.SpawnNewObject(endgame, new Vector3(8, 4.5f, 0), "EndGameButton", gameField.GetComponent<NetworkObject>());
            endGame.GetComponent<NetworkObject>().Spawn();
            GameObject drawcard = spawnObject.SpawnNewObject(draw, new Vector3(7,4.5f,0), "DrawButton", gameField.GetComponent<NetworkObject>());
            drawcard.GetComponent<NetworkObject>().Spawn();
            GameObject endTurn = spawnObject.SpawnNewObject(endturn, new Vector3(6, 4.5f, 0), "EndTurnButton", gameField.GetComponent<NetworkObject>());
            endTurn.GetComponent<NetworkObject>().Spawn();
        }
        else 
        {
            toggleVisibility.ChangeObjects(multiplayerClientMenu, gameField);
        }
        toggleVisibility.AppearObject(playerDisplay);
        SpawnStartingObjectsRpc();
    }

    //Creates the game
    [Rpc(SendTo.Server)]
    private void SpawnStartingObjectsRpc()
    {
        joinedPlayers++;
        if (joinedPlayers == playerCount)
        {

            tileScript.CreateTile(firstPosition, TileTypes.EmptyRoom, new bool[] { true, false, true, true });
            plus.CreateDirections(firstPosition);
            characters.SetUpCharacters();

            List<string> names = new List<string>();
            for (int i = 0; i < characters.CharacterList.Count; i++)
            {
                names.Add(characters.CharacterList[i].Item1);
            }

            WriteLogRpc("Multiplayer Started");
            WriteLogRpc("Game order: " + string.Join(" -> ", names));

            characters.ActivateFirstCharacter();
            if (cheating.Cheat)
            {
                cheating.GetNameListRpc();
            }
        }
    }

    //Write in the log for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void WriteLogRpc(string message)
    {
        writeLog.WriteNewLog(message);
    }
    #endregion
}