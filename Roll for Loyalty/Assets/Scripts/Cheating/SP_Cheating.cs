using TMPro;
using UnityEngine;

public class SP_Cheating : MonoBehaviour
{
    #region Variables
    private bool cheat = false;
    private int punishment = 0;

    //Getters & Setter
    public bool Cheat { get{ return cheat; } set { cheat = value; } }
    public int Punishment { get { return punishment; }}
    #endregion
    #region Functions
    //Toggles the cheat availabity from the game settings
    public void ToggleCheat()
    {
        cheat = !cheat;
    }

    //Set the punishment value from the game settings
    public void SetPunishment()
    {
        punishment = int.Parse(GameObject.Find("Main Camera/SinglePlayerAdvancedSetttings/Punishment/ChoosePunishment/Label").GetComponent<TextMeshProUGUI>().text);
    }
    #endregion
}