using Unity.Netcode;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MP_Endgame : NetworkBehaviour
{
    #region Variables
    //Scriptable Objects
    private MP_Characters characters;
    private GameBoard gameBoard;
    private ToggleVisibility toggleVisibility;

    //Game Objects
    private GameObject winnerScene;
    private GameObject mainMenu;
    private GameObject gameField;
    private GameObject playerInventory;

    //Inventories
    private InventoryObject inventory;
    private InventoryObject equipment;

    //texts
    private TextMeshProUGUI log;
    private TextMeshProUGUI chat;
    #endregion
    #region Functions
    [Rpc(SendTo.ClientsAndHost)]
    public void WonRpc(string name)
    {
        SwitchToWinnigScene();
        EndGame();
    }

    //Change the visuals
    private void SwitchToWinnigScene()
    {
        toggleVisibility = GameObject.Find("ScriptObjects/ToggleVisibility").GetComponent<ToggleVisibility>();
        GameObject mainCamera = GameObject.Find("Main Camera");
        winnerScene = mainCamera.transform.Find("Win").gameObject;

        toggleVisibility.AppearObject(winnerScene);
        winnerScene.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{name} won the game";
    }

    //Reset every Objects to it's default
    public void EndGame()
    {
        if (IsHost)
        {
            HostShutDownRpc();
            ClearCharacters();
            ClearGameBoard();
            BackToTheMainMenu();
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Closed the lobby");
        }
        else
        {
            LeaveGameServerRpc();
            ClearLog();
            ClearChat();
            BackToTheMainMenu();
            NetworkManager.Singleton.Shutdown();
            Debug.Log("Disconnected from the lobby");
        }
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void HostShutDownRpc()
    {
        ClearLog();
        ClearChat();
        BackToTheMainMenu();
    }

    //Resets the Objects 
    private void ClearGameBoard()
    {
        gameBoard = GameObject.Find("ScriptObjects/GameBoard").GetComponent<GameBoard>();
        foreach (KeyValuePair<Vector3, GameObject> boardPiece in gameBoard.GameBoardPieces)
        {
            boardPiece.Value.GetComponent<NetworkObject>().Despawn();
        }
        gameBoard.GameBoardPieces.Clear();
    }

    private void ClearCharacters()
    {
        characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character.CompareTag("MP_Player"))
            {
                inventory = characterinfo.character.GetComponent<MP_Player>().Inventory;
                inventory.Container.Clear();
                equipment = characterinfo.character.GetComponent<MP_Player>().Equipment;
                equipment.Container.Clear();
            }
            characterinfo.character.GetComponent<NetworkObject>().Despawn();
        }
        characters.CharacterList.Clear();
    }

    private void ClearLog()
    {
        log = GameObject.Find("Main Camera/MultiPlayer/Log/Viewport/Content").GetComponent<TextMeshProUGUI>();
        log.text = string.Empty;
    }

    private void ClearChat()
    {
        chat = GameObject.Find("Main Camera/MultiPlayer/Chat/Viewport/Content").GetComponent<TextMeshProUGUI>();
        chat.text = string.Empty;
    }

    //Change the visuals
    private void BackToTheMainMenu()
    {
        toggleVisibility = GameObject.Find("ScriptObjects/ToggleVisibility").GetComponent<ToggleVisibility>();
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainMenu = mainCamera.transform.Find("MainMenu").gameObject;
        gameField = GameObject.Find("MultiplayerGameField");
        playerInventory = GameObject.Find("Main Camera/MultiPlayer");
        toggleVisibility.AppearObject(mainMenu);
        toggleVisibility.DisappearObject(gameField);
        toggleVisibility.DisappearObject(playerInventory);
    }

    //If a clients leaves the server
    [Rpc(SendTo.Server)]
    private void LeaveGameServerRpc(RpcParams rpcParams = default)
    {
        ulong clientId = rpcParams.Receive.SenderClientId;

        characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        GameObject deletePlayer = null;
        foreach ((string name, GameObject character) characterInfo in characters.CharacterList)
        {
            NetworkObject netObj = characterInfo.character.GetComponent<NetworkObject>();
            if (netObj != null && netObj.OwnerClientId == clientId)
            {
                MP_Player player = characterInfo.character.GetComponent<MP_Player>();
                deletePlayer = player.gameObject;
                netObj.Despawn();
            }
        }
        CreateReplacement(deletePlayer);
    }

    //Repleace the client with a bot
    private void CreateReplacement(GameObject character)
    {
        GameObject botprefab = Resources.Load<GameObject>("Bot");
        MP_Bot bot = Instantiate(botprefab, character.transform.position, Quaternion.identity).GetComponent<MP_Bot>(); 
        bot.GetComponent<NetworkObject>().Spawn();
        bot.RepleacePlayer(character);
    }
    #endregion
}