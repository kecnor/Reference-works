using UnityEngine;

public class SP_DrawCard : MonoBehaviour
{
    #region Variables
    [SerializeField] private ItemDataBaseObject items;
    [SerializeField] private SP_Characters characters;
    [SerializeField] private SP_Cheating cheat;
    [SerializeField] private WriteLog writeLog;
    #endregion
    #region Function
    //The current active character is getting a random item
    public void Draw()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("SP_Player"))
        {
            SP_Player player = character.GetComponent<SP_Player>();
            if (player.DrawableCards > 0)
            {
                Item item = new Item(items.GetRandomItem());
                player.DrawableCards--;
                player.Inventory.AddItem(item, 1);
                writeLog.WriteNewLog($"{player.Name} found a new item.");
            }
            else if (cheat.Cheat)
            {
                player.Cheated = true;
                Item item = new Item(items.GetRandomItem());
                player.DrawableCards = 0;
                player.Inventory.AddItem(item, 1);
                writeLog.WriteNewLog($"{player.Name} found a new item.");
            }
            player.UpdateStatsVisual();
        }
        if (character.CompareTag("SP_Bot"))
        {
            SP_Bot bot = character.GetComponent<SP_Bot>();
            if (bot.DrawableCards > 0)
            {
                Item item = new Item(items.GetRandomItem());
                bot.DrawableCards--;
                bot.Inventory.AddItem(item, 1);
                writeLog.WriteNewLog($"{bot.Name} found a new item.");
            }
        }
    }
    #endregion
}