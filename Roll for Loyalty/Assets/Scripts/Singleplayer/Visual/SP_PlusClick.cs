using UnityEngine;
using UnityEngine.EventSystems;

public class SP_PlusClick : MonoBehaviour, IPointerDownHandler
{
    public void OnPointerDown(PointerEventData eventData)
    {
        GameObject obj = GameObject.Find("ScriptObjects/Singleplayer/Singleplayerplus");
        SP_Plus plusScript = obj.GetComponent<SP_Plus>();
        plusScript.PlusClicked(gameObject);
    }
}
