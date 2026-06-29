using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class MP_Cheating : NetworkBehaviour
{
    #region Variables
    private NetworkVariable<bool> cheat = new NetworkVariable<bool>(false);
    private NetworkVariable<int> punishment = new NetworkVariable<int>(0);

    //Getters
    public bool Cheat { get { return cheat.Value; } }
    public int Punishment { get { return punishment.Value; } }
    #endregion
    #region Functions
    //Toggles the cheat availabity from the game settings
    public void ToggleCheat()
    {
        if (IsServer)
        {
            cheat.Value = !cheat.Value;
        }
        else
        {
            ToggleCheatServerRpc();
        }
    }

    [Rpc(SendTo.Server)]
    private void ToggleCheatServerRpc()
    {
        cheat.Value = !cheat.Value;
    }

    //Fill selection's visual with the characters's names
    [Rpc(SendTo.Server)]
    public void GetNameListRpc()
    {
        MP_Characters characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        string names = "no one";
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            names += $":{characterinfo.name}";
        }
        FillCheatersRpc(names);
    }

    [Rpc(SendTo.ClientsAndHost)]
    private void FillCheatersRpc(string names)
    {
        string[] namesArray = names.Split(':');
        GameObject mainCamera = GameObject.Find("Main Camera");
        Transform multiplayer = mainCamera.transform.Find("MultiPlayerCheatCharacters");
        TMP_Dropdown charactersOptions = multiplayer.Find("Characters").GetComponent<TMP_Dropdown>();
        charactersOptions.ClearOptions();
        charactersOptions.AddOptions(new List<string>(namesArray));
    }

    //Set the punishment value from the game settings
    public void SetPunishment()
    {
        if (IsServer)
        {
            punishment.Value = int.Parse(GameObject.Find("Main Camera/MultiPlayerAdvancedSetttings/Punishment/ChoosePunishment/Label").GetComponent<TextMeshProUGUI>().text);
            Debug.Log(this.punishment.Value);
        }
        else
        {
            SetPunishmentServerRpc(int.Parse(GameObject.Find("Main Camera/MultiPlayerAdvancedSetttings/Punishment/ChoosePunishment/Label").GetComponent<TextMeshProUGUI>().text));
        }
    }

    [Rpc(SendTo.Server)]
    private void SetPunishmentServerRpc(int punishment)
    {
        this.punishment.Value = punishment;
        Debug.Log(this.punishment.Value);
    }
    #endregion
}