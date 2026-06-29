using System.Collections.Generic;
using UnityEngine;

public class SP_Characters : MonoBehaviour
{
    #region Variable
    private List<(string, GameObject)> characters = new List<(string, GameObject)>();

    //Getter
    public List<(string, GameObject)> CharacterList {get { return characters; } }
    #endregion
    #region Functions
    //Changes the character list
    public void AddCharacter(string type, GameObject character)
    {
        characters.Add((type, character));
    }

    public void RemoveCharacter(string type, GameObject character)
    {
        characters.Remove((type, character));
    }

    //Returns the character by the given id
    public int GetActiveCharacterID()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Item2.CompareTag("SP_Player"))
            {
                if (characters[i].Item2.GetComponent<SP_Player>().Active)
                {
                    return i;
                }
            }
            else if (characters[i].Item2.CompareTag("SP_Bot"))
            {
                if (characters[i].Item2.GetComponent<SP_Bot>().Active)
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
            if (characters[i].Item2 == character || (character == null && characters[i].Item2.CompareTag("SP_Player")))
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
            if (charaterinfo.character.CompareTag("SP_Player"))
            {
                if (charaterinfo.character.GetComponent<SP_Player>().Active)
                {
                    return charaterinfo.character;
                }
            }
            else if (charaterinfo.character.CompareTag("SP_Bot"))
            {
                if (charaterinfo.character.GetComponent<SP_Bot>().Active)
                {
                    return charaterinfo.character;
                }
            }
        }
        return null;
    }

    //Returns the order of the characters by their name
    public List<string> GetGameOrder()
    {
        List<string> names = new List<string>();
        for (int i = 0; i < characters.Count; i++)
        {
            if (characters[i].Item2.CompareTag("SP_Player"))
            {
                names.Add(characters[i].Item2.GetComponent<SP_Player>().Name);
            }
            else if (characters[i].Item2.CompareTag("SP_Bot"))
            {
                names.Add(characters[i].Item2.GetComponent<SP_Bot>().Name);
            }
        }
        return names;
    }

    //Getting ready every character to start the game
    public void SetUpCharacters()
    {
        RandomCharacterSequence();
        SetUpReputations();
        SetCharacterActivity(0, true);
    }

    //Shuffle the character's order
    private void RandomCharacterSequence()
    {
        for (int i = characters.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (characters[i], characters[randomIndex]) = (characters[randomIndex], characters[i]);
            if (characters[i].Item2.CompareTag("SP_Player"))
            {
                characters[i].Item2.GetComponent<SP_Player>().ReputationID = i;
            }
            else if (characters[i].Item2.CompareTag("SP_Bot"))
            {
                characters[i].Item2.GetComponent<SP_Bot>().ReputationID = i;
            }
        }
    }

    //Setup reputation system for every character
    private void SetUpReputations()
    {
        foreach ((string name, GameObject character) character in characters)
        {
            if (character.character.CompareTag("SP_Player"))
            {
                character.character.GetComponent<SP_Player>().SetReputation(characters.Count);
            }
            else if (character.character.CompareTag("SP_Bot"))
            {
                character.character.GetComponent<SP_Bot>().SetReputation(characters.Count);
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
        if (characters[id].Item2.CompareTag("SP_Player"))
        {
            characters[id].Item2.GetComponent<SP_Player>().Active = isActive;
            foreach (var item in characters[id].Item2.GetComponent<SP_Player>().AttributeList)
            {
                item.Value.TempValue = 0;
            }
        }
        else if (characters[id].Item2.CompareTag("SP_Bot"))
        {
            characters[id].Item2.GetComponent<SP_Bot>().Active = isActive;
            foreach (var item in characters[id].Item2.GetComponent<SP_Bot>().AttributeList)
            {
                item.Value.TempValue = 0;
            }
        }
    }

    //Activate the character's turn
    public void ActivateCharacter()
    {
        GameObject character = GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            character.GetComponent<SP_Player>().ActivateTurn();
        }
        else if (character.CompareTag("SP_Bot"))
        {
            character.GetComponent<SP_Bot>().ActivateTurn();
        }
    }

    //Give the character their buff
    public void ApplyBuff(ItemBuff[] buffs, GameObject giver)
    {
        GameObject character = GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            character.GetComponent<SP_Player>().ApplyBuff(buffs, GetCharacterId(giver));
        }
        else if (character.CompareTag("SP_Bot"))
        {
            character.GetComponent<SP_Bot>().ApplyBuff(buffs, GetCharacterId(giver));
        }
    }
    #endregion
}