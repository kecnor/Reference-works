using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class MP_PlusClick : NetworkBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SendPlusClickedToServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void SendPlusClickedToServerRpc()
    {
        GameObject obj = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerPlus");
        MP_Plus plusScript = obj.GetComponent<MP_Plus>();
        plusScript.PlusClicked(gameObject);
    }
}