
using TMPro;
using UnityEngine;

public class CheaterConnector : MonoBehaviour
{
    #region Variable
    private MP_CheateingList cheating;
    #endregion
    #region Function
    public void PunishCharacter()
    {
        cheating = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCheaterList").GetComponent<MP_CheateingList>();
        string name = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        cheating.PunishCheaterRpc(name);
    }
    #endregion
}