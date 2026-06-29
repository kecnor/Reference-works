using UnityEngine.EventSystems;
using Unity.Netcode;

public class MP_EndGameClicked : NetworkBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        gameObject.GetComponent<MP_Endgame>().EndGame();
    }
}