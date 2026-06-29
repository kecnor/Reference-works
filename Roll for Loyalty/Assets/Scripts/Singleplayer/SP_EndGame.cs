using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SP_EndGame : MonoBehaviour
{
    #region Variables
    //Scriptable Objects
    [SerializeField] private SP_Characters characters;
    [SerializeField] private GameBoard gameBoard;
    [SerializeField] private TextMeshProUGUI log;
    [SerializeField] private ToggleVisibility toggleVisibility;

    //Scenes
    [SerializeField] private GameObject winnerScene;
    [SerializeField] private GameObject singleplayerInventory;
    [SerializeField] private GameObject gameField;

    //Inventories
    private InventoryObject inventory;
    private InventoryObject equipment;
    #endregion
    #region Functions
    public void Won(string name)
    {
        ChangeScene(name);
        EndGame();
    }

    //Change the visuals
    private void ChangeScene(string name)
    {
        toggleVisibility.AppearObject(winnerScene);
        toggleVisibility.DisappearObject(singleplayerInventory);
        toggleVisibility.DisappearObject(gameField);
        winnerScene.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{name} won the game";
    }

    //Reset every Objects to it's default
    public void EndGame()
    {
        ClearCharacters();
        ClearGameBoard(); 
        ClearLog();
    }

    private void ClearCharacters()
    {
        foreach ((string name, GameObject character) characterinfo in characters.CharacterList)
        {
            if (characterinfo.character.CompareTag("SP_Player"))
            {
                inventory = characterinfo.character.GetComponent<SP_Player>().Inventory;
                inventory.Container.Clear();
                equipment = characterinfo.character.GetComponent<SP_Player>().Equipment;
                equipment.Container.Clear();
            }
            Destroy(characterinfo.character);
        }
        characters.CharacterList.Clear();
    }

    private void ClearGameBoard()
    {
        foreach (KeyValuePair<Vector3, GameObject> boardPiece in gameBoard.GameBoardPieces)
        {
            Destroy(boardPiece.Value);
        }
        gameBoard.GameBoardPieces.Clear();
    }

    private void ClearLog()
    {
        log.text = string.Empty;
    }
    #endregion
}