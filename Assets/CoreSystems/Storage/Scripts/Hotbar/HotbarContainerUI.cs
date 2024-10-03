using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarContainerUI : SlotContainerUI
{
    [SerializeField] protected Image[] slots = new Image[HotbarContainer.MAX_HOTBAR_ITEMS];
    protected PlayerInventory inventory { get; private set; }
    protected int lastSelectedSlotIndex = -1;


    private void Start()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }
        if (container == null)
        {
            container = inventory.GetHotbarContainer();
        }
        validateSlots();
        setupSlotsSelection();
    }

    private void validateSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
            {
                Debug.LogError($"Hotbar slot {i} is null");
            }
        }
    }

    /// <summary>
    /// Deselects every empty slot. Selects slot lastSelectedSlotIndex
    /// </summary>
    private void setupSlotsSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            deselectSlot(i);
        }
        if (lastSelectedSlotIndex != -1)
        {
            selectSlot(lastSelectedSlotIndex);
        }
    }

    /// <summary>
    /// Uses lastSelectedSlotIndex and inventory.SelectedSlot to decide whick slots to select/deselect.
    /// Sets lastSelectedSlotIndex to inventory.SelectedSlot
    /// </summary>
    private void updateSlotsSelection()
    {
        if (inventory == null)
        {
            Debug.LogError("inventory = null while updating hotbar slots selection");
        }
        if (inventory.SelectedSlot != lastSelectedSlotIndex)
        {
            if (lastSelectedSlotIndex != -1)
            {
                deselectSlot(lastSelectedSlotIndex);
            }
            if (inventory.SelectedSlot != -1)
            {
                selectSlot(inventory.SelectedSlot);
            }
            lastSelectedSlotIndex = inventory.SelectedSlot;
        }
    }

    public override Rect GetWorldPositionRectForIndex(int index)
    {
        RectTransform rectTransform = slots[index].GetComponent<RectTransform>();
        Vector2 leftDown = rectTransform.TransformPoint(rectTransform.rect.position);
        Vector2 rightUp = rectTransform.TransformPoint(rectTransform.rect.position + rectTransform.rect.size);
        return new Rect(leftDown, rightUp - leftDown);
    }

    

    protected void deselectSlot(int index)
    {
        slots[index].color = new Color32(219, 219, 219, 255);
    }

    protected void selectSlot(int index)
    {
        slots[index].color = new Color32(145, 255, 126, 255);
    }

    protected override void Update()
    {
        base.Update();
        updateSlotsSelection();
    }


    
}