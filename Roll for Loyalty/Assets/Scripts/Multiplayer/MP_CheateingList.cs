using UnityEngine;
using Unity.Netcode;

public class MP_CheateingList : NetworkBehaviour
{
    #region Variables
    [SerializeField] private MP_Characters characters;
    private MP_Player player;
    #endregion
    #region Functions
    //The choosen character gets punished if cheated 
    [Rpc(SendTo.Server)]
    public void PunishCheaterRpc(string name, RpcParams rpcParams = default)
    {
        ulong senderId = rpcParams.Receive.SenderClientId;
        GameObject senderCharacter = GetCharacterByClientId(senderId);
        player = senderCharacter.GetComponent<MP_Player>();
        if (name != "no one")
        {
            GameObject character = characters.GetCharacterByName(name);
            if (character.CompareTag("MP_Player"))
            {
                MP_Player cheater = character.GetComponent<MP_Player>();
                if (cheater != player)
                {
                    cheater.CaughtOnCheating(player.ReputationID);
                }
            }
            else if (character.CompareTag("MP_Bot"))
            {
                MP_Bot bot = character.GetComponent<MP_Bot>();
                if (bot.Cheated)
                {
                    bot.CaughtOnCheating(player.ReputationID);
                }
            }
        }
    }

    public GameObject GetCharacterByClientId(ulong clientId)
    {
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            NetworkObject netObj = characterinfo.character.GetComponent<NetworkObject>();
            if (netObj.OwnerClientId == clientId)
            {
                return characterinfo.character;
            }
        }
        return null;
    }
    #endregion
}