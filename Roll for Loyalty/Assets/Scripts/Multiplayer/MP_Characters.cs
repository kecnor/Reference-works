using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MP_Characters : NetworkBehaviour
{
    #region Variable
    private List<(string, GameObject)> characters = new List<(string, GameObject)>();

    //Getter
    public List<(string, GameObject)> CharacterList { get { return characters; } }
    #endregion
    #region Functions
    //Changes the character list
    public void AddCharacter(string name, GameObject character)
    {
        characters.Add((name, character));
    }

    public void RemoveCharacter(string name, GameObject character)
    {
        characters.Remove((name, character));
    }

    //Returns the character by the given id
    public int GetActiveCharacterID()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Item2.CompareTag("MP_Player"))
            {
                if (characters[i].Item2.GetComponent<MP_Player>().Active)
                {
                    return i;
                }
            }
            else if (characters[i].Item2.CompareTag("MP_Bot"))
            {
                if (characters[i].Item2.GetComponent<MP_Bot>().Active)
                {
                    return i;
                }
            }
        }
        return -1;
    }

    //Returns the character by it's name
    public GameObject GetCharacterByName(string name)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Item1 == name)
            {
                return characters[i].Item2;
            }
        }
        return null;
    }

    //Returns the given characters id
    private int GetCharacterId(GameObject character)
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Item2 == character || (character == null && characters[i].Item1 == "Player"))
            {
                return i;
            }
        }
        return -1;
    }

    //Return the only character, who's activity is true
    public GameObject GetActiveCharacter()
    {
        foreach ((string type, GameObject character) charaterinfo in characters)
        {
            if (charaterinfo.character.CompareTag("MP_Player"))
            {
                if (charaterinfo.character.GetComponent<MP_Player>().Active)
                {
                    return charaterinfo.character;
                }
            }
            else if (charaterinfo.character.CompareTag("MP_Bot"))
            {
                if (charaterinfo.character.GetComponent<MP_Bot>().Active)
                {
                    return charaterinfo.character;
                }
            }
        }
        return null;
    }

    //Getting ready every character to start the game
    public void SetUpCharacters()
    {
        RandomCharacterSequence();
        SetUpReputations();
    }

    //Activate the first character
    public void ActivateFirstCharacter()
    {
        SetCharacterActivity(0, true);
        ActivateCharacter();
    }

    //Shuffle the character's order
    private void RandomCharacterSequence()
    {
        for (int i = characters.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            if (characters[i].Item2.CompareTag("MP_Player"))
            {
                characters[i].Item2.GetComponent<MP_Player>().ReputationID = i;
            }
            else if (characters[i].Item2.CompareTag("MP_Bot"))
            {
                characters[i].Item2.GetComponent<MP_Bot>().ReputationID = i;
            }
        }
    }

    //Setup reputation system for every character
    private void SetUpReputations()
    {
        foreach ((string name, GameObject character) character in characters)
        {
            if (character.character.CompareTag("MP_Player"))
            {
                character.character.GetComponent<MP_Player>().SetReputation(characters.Count);
            }
            else if (character.character.CompareTag("MP_Bot"))
            {
                character.character.GetComponent<MP_Bot>().SetReputation(characters.Count);
            }
        }
    }


    //Setup the next character's turn 
    public void EndTurn()
    {
        int activeCharacterID = GetActiveCharacterID();
        SetCharacterActivity(activeCharacterID, false);
        if (activeCharacterID == characters.Count - 1)
        {
            activeCharacterID = 0;
        }
        else
        {
            activeCharacterID++;
        }
        SetCharacterActivity(activeCharacterID, true);
        ActivateCharacter();
    }

    //Set the given character's activity
    public void SetCharacterActivity(int id, bool isActive)
    {
        if (characters[id].Item2.CompareTag("MP_Player"))
        {
            characters[id].Item2.GetComponent<MP_Player>().Active = isActive;
            foreach (var item in characters[id].Item2.GetComponent<MP_Player>().AttributeList)
            {
                item.Value.TempValue = 0;
            }
        }
        else if (characters[id].Item2.CompareTag("MP_Bot"))
        {
            characters[id].Item2.GetComponent<MP_Bot>().Active = isActive;
            foreach (var item in characters[id].Item2.GetComponent<MP_Bot>().AttributeList)
            {
                item.Value.TempValue = 0;
            }
        }
    }

    //Activate the character's turn
    private void ActivateCharacter()
    {
        GameObject character = GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            character.GetComponent<MP_Player>().ActivateTurnRpc();
        }
        else if (character.CompareTag("MP_Bot"))
        {
            character.GetComponent<MP_Bot>().ActivateTurn();
        }
    }

    //Give the character their buff
    public void ApplyBuff(ItemBuff[] buffs, GameObject giver)
    {
        GameObject character = GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            character.GetComponent<MP_Player>().ApplyBuff(buffs, GetCharacterId(giver));
        }
        else if (character.CompareTag("MP_Bot"))
        {
            character.GetComponent<MP_Bot>().ApplyBuff(buffs, GetCharacterId(giver));
        }

    }

    //Swap the disconnected player with a new bot's
    public void SwapCharacters(GameObject fromCharacter, GameObject intoCharacter)
    {
        int index = GetCharacterId(fromCharacter);
        string name = fromCharacter.GetComponent<MP_Player>().Name;
        characters[index] = (name, intoCharacter);
    }
    #endregion
}