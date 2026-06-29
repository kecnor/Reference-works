using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DynamicInterface : UserInterface
{
    #region Variables
    public float xStart;
    public float yStart;
    public float xSpaceBetweenSlots;
    public float ySpaceBetweenSlots;
    public int numberOfColumns;

    [SerializeField] private GameObject InventoryPrefab;
    #endregion
    #region Functions
    //Creating the inventory's slots with the given variables, then equiping them with the nessesary events and visual
    public override void CreateSlots()
    {
        ClearOldSlots();

        slotsOnInterface = new Dictionary<GameObject, InventorySlot>();
        for (int i = 0; i < inventory.GetSlots.Length; i++)
        {
            GameObject obj = Instantiate(InventoryPrefab, Vector3.zero, Quaternion.identity, transform);
            obj.GetComponent<RectTransform>().localPosition = GetPosition(i);

            AddEvent(obj, EventTriggerType.PointerClick, delegate { OnRightClick(obj); });
            AddEvent(obj, EventTriggerType.PointerEnter, delegate { OnEnter(obj); });
            AddEvent(obj, EventTriggerType.PointerExit, delegate { OnExit(obj); });
            AddEvent(obj, EventTriggerType.BeginDrag, delegate { OnDragStart(obj); });
            AddEvent(obj, EventTriggerType.EndDrag, delegate { OnDragEnd(obj); });
            AddEvent(obj, EventTriggerType.Drag, delegate { OnDrag(obj); });

            inventory.GetSlots[i].parentInventory = inventory;
            inventory.GetSlots[i].slotDisplay = obj;

            slotsOnInterface.Add(obj, inventory.GetSlots[i]);
        }
    }

    private void ClearOldSlots()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(transform.GetChild(i).gameObject);
        }
    }

    private Vector3 GetPosition(int i)
    {
        return new Vector3(xStart + xSpaceBetweenSlots * (i % numberOfColumns), yStart - ySpaceBetweenSlots * (i / numberOfColumns), 0);
    }
    #endregion
}