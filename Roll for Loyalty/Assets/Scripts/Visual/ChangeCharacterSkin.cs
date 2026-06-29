using TMPro;
using UnityEngine;

public class ChangeCharacterSkin : MonoBehaviour
{
    #region Variables
    [SerializeField] private Sprite[] skins;
    #endregion
    #region Functions
    //Set the character's skin
    public void CharacterClassValueChanged()
    {
        string characterClass = transform.parent.Find("Class/Label").GetComponent<TextMeshProUGUI>().text;
        ChangeSkin(characterClass);
    }

    public void ChangeSkin(string characterClass)
    {
        SpriteRenderer characterSkin = GetComponent<SpriteRenderer>();
        switch (characterClass)
        {
            case "Random":
                characterSkin.sprite = skins[0];
                break;
            case "Fighter":
                characterSkin.sprite = skins[1];
                break;
            case "Ranger":
                characterSkin.sprite = skins[2];
                break;
            case "Rouge":
                characterSkin.sprite = skins[3];
                break;
            case "Wizard":
                characterSkin.sprite = skins[4];
                break;
            default:
                characterSkin.sprite = skins[0];
                break;
        }
    }
    #endregion
}
