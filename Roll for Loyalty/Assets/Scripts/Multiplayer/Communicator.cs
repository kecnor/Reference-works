using TMPro;
using UnityEngine;
using Unity.Netcode;

public class Communicator : NetworkBehaviour
{
    #region Variables
    [SerializeField] private MP_Characters characters;
    [SerializeField] private TMP_InputField chatInput;
    [SerializeField] private WriteLog writeLog;
    #endregion
    #region Functions
    //Send message
    private void Update()
    {
        if (!IsClient || !IsSpawned)
            return;

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (chatInput == null)
            {
                Debug.LogError("Communicator: chatInput is null.");
                return;
            }

            string message = chatInput.text;

            if (string.IsNullOrWhiteSpace(message))
                return;

            SendChatMessageToServerRpc(message);
            chatInput.text = "";
        }
    }

    [Rpc(SendTo.Server)]
    public void SendChatMessageToServerRpc(string message, RpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;

        GameObject senderCharacter = GetCharacterByClientId(senderId);

        string playerName = "Unknown";

        if (senderCharacter != null)
        {
            MP_Player player = senderCharacter.GetComponent<MP_Player>();
            if (player != null)
            {
                playerName = player.name;
            }
        }

        string finalMessage = playerName + ": " + message;
        SendChatMessageClientRpc(finalMessage);
    }

    //Get the message sender's name
    private GameObject GetCharacterByClientId(ulong clientId)
    {
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character != null)
            { 

            NetworkObject netObj = characterinfo.character.GetComponent<NetworkObject>();
                if (netObj != null && netObj.OwnerClientId == clientId)
                {
                    return characterinfo.character;
                }
            }
        }
        return null;
    }

    //Write message to all player
    [Rpc(SendTo.ClientsAndHost)]
    public void SendChatMessageClientRpc(string message)
    {
        if (writeLog == null)
        {
            Debug.LogError("Communicator: writeLog is null.");
            return;
        }

        writeLog.WriteNewLog(message);
    }
    #endregion
}