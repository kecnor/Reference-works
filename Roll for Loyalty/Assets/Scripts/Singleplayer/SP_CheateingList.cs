using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SP_CheateingList : MonoBehaviour
{
    #region Variables
    [SerializeField] private SP_Characters characters;
    private SP_Player player;
    #endregion
    #region Constructor
    void Start()
    {
        FillDropdown();
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character.CompareTag("SP_Player"))
            {
                player = characterinfo.character.GetComponent<SP_Player>();
            }
        }
    }
    #endregion
    #region Functions
    //Fill selection's visual with the characters's names
    public void FillDropdown()
    {
        List<string> names = new List<string>();
        names.Add("no one");
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            names.Add(characterinfo.name);
        }

        GetComponent<TMP_Dropdown>().ClearOptions();
        GetComponent<TMP_Dropdown>().AddOptions(names);
    }

    //The choosen character gets punished if cheated 
    public void PunishCheater()
    {
        string name = transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        if (name != "no one")
        {
            GameObject character = characters.GetCharacterByName(name);
            if (character.CompareTag("SP_Bot"))
            {
                SP_Bot bot = character.GetComponent<SP_Bot>();
                if (bot.Cheated)
                {
                    bot.CaughtOnCheating(player.ReputationID);
                }
            }
        }
    }
    #endregion
}