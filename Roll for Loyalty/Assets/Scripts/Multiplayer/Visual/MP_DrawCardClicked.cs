using UnityEngine;
using UnityEngine.EventSystems;
using Unity.Netcode;

public class MP_DrawCardClicked : NetworkBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        SendDrawClickedToServerRpc();
    }

    [Rpc(SendTo.Server)]
    private void SendDrawClickedToServerRpc()
    {
        GameObject obj = GameObject.Find("ScriptObjects/Multiplayer/DrawCard");
        MP_DrawCard drawCard = obj.GetComponent<MP_DrawCard>();
        drawCard.DrawRpc();
    }
}
