using UnityEngine;

public class MP_TreasureTile : MP_Tile
{
    //Activateing the tile
    public override void ActivateTile()
    {
        MP_Characters characters = GameObject.Find("ScriptObjects/Multiplayer/MultiplayerCharacters").GetComponent<MP_Characters>();
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            character.GetComponent<MP_Player>().DrawableCards++;
        }
        if (character.CompareTag("MP_Bot"))
        {
            character.GetComponent<MP_Bot>().DrawableCards++;
        }
    }
}