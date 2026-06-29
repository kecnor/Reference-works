using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StaticInterface : UserInterface
{
    #region Variable
    public GameObject[] slots;
    #endregion
    #region Function
    //Equips the inventory's slots with the nessesary events and visuals
    public override void CreateSlots()
    {
        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            var slot = slots[i];

            AddEvent(slot, EventTriggerType.PointerEnter, delegate { OnEnter(slot); });
            AddEvent(slot, EventTriggerType.PointerExit, delegate { OnExit(slot); });
            AddEvent(slot, EventTriggerType.BeginDrag, delegate { OnDragStart(slot); });
            AddEvent(slot, EventTriggerType.EndDrag, delegate { OnDragEnd(slot); });
            AddEvent(slot, EventTriggerType.Drag, delegate { OnDrag(slot); });

            inventory.GetSlots[i].slotDisplay = slot;
            slotsOnInterface.Add(slot, inventory.GetSlots[i]);
        }
    }
    #endregion
}