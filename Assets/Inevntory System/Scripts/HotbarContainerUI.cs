using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarContainerUI : SlotContainerUI
{
    [SerializeField] protected Image[] slots = new Image[HotbarContainer.MAX_HOTBAR_ITEMS];
    [SerializeField, HideInInspector] protected PlayerInventory inventory;
    [SerializeField, HideInInspector] protected int lastSelectedSlotIndex = -1;

    private void Start()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }
        if (container == null)
        {
            container = inventory.Hotbar;
        }
    }

    public override Rect GetRectForIndex(int index)
    {
        return (slots[index].transform as RectTransform).rect;
    }

    protected void deselectSlot(int index)
    {
        slots[index].color = new Color32(219, 219, 219, 255);
    }

    protected void selectSlot(int index)
    {
        slots[index].color = new Color32(145, 255, 126, 255);
    }

    protected void Update()
    {
        if (inventory.selectedSlot != lastSelectedSlotIndex)
        {
            if (lastSelectedSlotIndex != -1)
            {
                deselectSlot(lastSelectedSlotIndex);
            }
            if (inventory.selectedSlot != -1)
            {
                selectSlot(inventory.selectedSlot);
            }
            lastSelectedSlotIndex = inventory.selectedSlot;
        }

    }
}