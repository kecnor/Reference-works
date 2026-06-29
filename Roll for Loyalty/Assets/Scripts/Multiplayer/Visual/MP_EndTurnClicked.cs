using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class MP_EndTurnClicked : NetworkBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SendDrawClickedToServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void SendDrawClickedToServerRpc()
    {
        GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>().EndTurn();
    }
}