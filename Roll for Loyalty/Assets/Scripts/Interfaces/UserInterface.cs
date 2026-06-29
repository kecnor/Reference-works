using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

public abstract class UserInterface : MonoBehaviour
{
    #region Variables
    public InventoryObject inventory;
    public Dictionary<GameObject, InventorySlot> slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
    public Action<ItemBuff[]> OnUseItem;
    #endregion
    #region Constructor
    void Awake()
    {
        foreach (InventorySlot slot in inventory.GetSlots)
        {
            slot.parentInventory = inventory;
            slot.OnAfterUpdate += OnSlotUpdate;
            slot.OnBeforeUpdate += OnSlotUpdate;
        }
        CreateSlots();
        AddEvent(gameObject, EventTriggerType.PointerEnter, delegate { OnEnterInterface(gameObject); });
        AddEvent(gameObject, EventTriggerType.PointerExit, delegate { OnExitInterface(gameObject); });
    }

    public void BindInventory(InventoryObject newInventory)
    {
        inventory = newInventory;
        slotsOnInterface.Clear();
        foreach (InventorySlot slot in inventory.GetSlots)
        {
            slot.parentInventory = inventory;
            slot.OnAfterUpdate -= OnSlotUpdate;
            slot.OnBeforeUpdate -= OnSlotUpdate;

            slot.OnAfterUpdate += OnSlotUpdate;
            slot.OnBeforeUpdate += OnSlotUpdate;
        }

        CreateSlots();
        foreach (InventorySlot slot in inventory.GetSlots)
        {
            OnSlotUpdate(slot);
        }
    }

    public abstract void CreateSlots();
    #endregion
    #region Functions
    //Visual update for the given slot
    private void OnSlotUpdate(InventorySlot slot)
    {
        if (slot.Item.ID >= 0)
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = slot.ItemObject.display;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 1);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = slot.Amount == 1 ? "" : slot.Amount.ToString();
        }
        else
        {
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().sprite = null;
            slot.slotDisplay.transform.GetChild(0).GetComponentInChildren<Image>().color = new Color(1, 1, 1, 0);
            slot.slotDisplay.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }
    //Creating events that triggers when interacting with the given slot
    protected void AddEvent(GameObject obj, EventTriggerType type, UnityAction<BaseEventData> action)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        var eventTrigger = new EventTrigger.Entry();
        eventTrigger.eventID = type;
        eventTrigger.callback.AddListener(action);
        trigger.triggers.Add(eventTrigger);
    }

    public void OnRightClick(GameObject obj)
    {
        InventorySlot slot = slotsOnInterface[obj];
        if (inventory.UseItem(slot, out ItemBuff[] buffs))
        {
            OnUseItem?.Invoke(buffs);
        }
    }

    public void OnEnter(GameObject obj)
    {
        MouseData.slotHoverredOver = obj;
    }

    public void OnExit(GameObject obj)
    {
        MouseData.slotHoverredOver = null;
    }

    public void OnEnterInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = obj.GetComponent<UserInterface>();
    }

    public void OnExitInterface(GameObject obj)
    {
        MouseData.interfaceMouseIsOver = null;
    }

    public void OnDragStart(GameObject obj)
    {
        MouseData.tempItemBeingDragged = CreateTempItem(obj);
        moveDraggedItemToMouse();
    }

    public void OnDrag(GameObject obj)
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            moveDraggedItemToMouse();
        }
    }

    public void OnDragEnd(GameObject obj)
    {
        if (obj != null)
        {
            Destroy(MouseData.tempItemBeingDragged);
            if (MouseData.interfaceMouseIsOver != null)
            {
                if (MouseData.slotHoverredOver.CompareTag("Bin"))
                {
                    slotsOnInterface[obj].RemoveItem();
                }
                else if (MouseData.slotHoverredOver)
                {
                    InventorySlot mousHoverSlotData = MouseData.interfaceMouseIsOver.slotsOnInterface[MouseData.slotHoverredOver];
                    inventory.SwapItems(slotsOnInterface[obj], mousHoverSlotData);
                }
            }
        }
    }

    //Creating a visual Object for the dragged item
    private GameObject CreateTempItem(GameObject obj)
    {
        GameObject tempItem = null;
        if (slotsOnInterface[obj].Item.ID >= 0)
        {
            tempItem = new GameObject();
            var rt = tempItem.AddComponent<RectTransform>();
            rt.sizeDelta = new Vector2(0.5f, 0.5f);
            tempItem.transform.SetParent(transform.parent);
            var img = tempItem.AddComponent<Image>();
            img.sprite = slotsOnInterface[obj].ItemObject.display;
            img.raycastTarget = false;
        }
        return tempItem;
    }

    private void moveDraggedItemToMouse()
    {
        if (MouseData.tempItemBeingDragged != null)
        {
            Vector3 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            worldPoint.z = transform.parent.position.z;

            MouseData.tempItemBeingDragged.GetComponent<RectTransform>().position = worldPoint;
        }
    }
    #endregion
}