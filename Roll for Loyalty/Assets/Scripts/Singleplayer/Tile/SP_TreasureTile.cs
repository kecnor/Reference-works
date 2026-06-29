using UnityEngine;

public class SP_TreasureTile : SP_Tile
{
    //Activateing the tile
    public override void ActivateTile()
    {
        SP_Characters characters = GameObject.Find("ScriptObjects/Singleplayer/SingleplayerCharacters").GetComponent<SP_Characters>();
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            character.GetComponent<SP_Player>().DrawableCards++;
        }
        if (character.CompareTag("SP_Bot"))
        {
            character.GetComponent<SP_Bot>().DrawableCards++;
        }
    }
}