using UnityEngine;
using Unity.Netcode;

public class MP_DrawCard : NetworkBehaviour
{
    #region Variables
    [SerializeField] private ItemDataBaseObject items;
    [SerializeField] private MP_Characters characters;
    [SerializeField] private MP_Cheating cheat;
    [SerializeField] private WriteLog writeLog;
    #endregion
    #region
    //The current active character is getting a random item
    [Rpc(SendTo.Server)]
    public void DrawRpc()
    {
        GameObject character = characters.GetActiveCharacter();
        if (character.CompareTag("MP_Player"))
        {
            MP_Player player = character.GetComponent<MP_Player>();
            if (player.DrawableCards > 0)
            {
                Item item = new Item(items.GetRandomItem());
                player.DrawableCards--;
                if (character.GetComponent<NetworkObject>().IsOwnedByServer)
                {
                    player.Inventory.AddItem(item, 1);
                }
                else
                {
                    player.AddItemToInventoryClientRpc(item.ID, 1);
                }
                WriteLogRpc($"{player.Name} found a new item.");
            }
            else if (cheat.Cheat)
            {
                player.Cheated = true;
                Item item = new Item(items.GetRandomItem());
                player.DrawableCards = 0;
                if (character.GetComponent<NetworkObject>().IsOwnedByServer)
                {
                    player.Inventory.AddItem(item, 1);
                }
                else
                {
                    player.AddItemToInventoryClientRpc(item.ID, 1);
                }
                WriteLogRpc($"{player.Name} found a new item.");
            }
            player.UpdateStatsVisualClientRpc();
        }
        if (character.CompareTag("MP_Bot"))
        {
            MP_Bot bot = character.GetComponent<MP_Bot>();
            if (bot.DrawableCards > 0)
            {
                Item item = new Item(items.GetRandomItem());
                bot.DrawableCards--;
                bot.Inventory.AddItem(item, 1);
                WriteLogRpc($"{bot.Name} found a new item.");
            }
        }
    }

    //Write in the log for every player
    [Rpc(SendTo.ClientsAndHost)]
    private void WriteLogRpc(string message)
    {
        writeLog.WriteNewLog(message);
    }
    #endregion
}