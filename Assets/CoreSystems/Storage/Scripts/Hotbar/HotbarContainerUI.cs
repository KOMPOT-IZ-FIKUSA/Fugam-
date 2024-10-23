using UnityEngine;
using UnityEngine.UI;

public class HotbarContainerUI : SlotContainerUI
{
    private Image[] images = new Image[HotbarContainer.MAX_HOTBAR_ITEMS]; 

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

        images = new Image[SlotsTransforms.Length];
        for (int i = 0; i < SlotsTransforms.Length; i++)
        {
            images[i] = SlotsTransforms[i].GetComponent<Image>();
        }

        setupSlotsSelection();
    }



    /// <summary>
    /// Deselects every empty slot. Selects slot lastSelectedSlotIndex
    /// </summary>
    private void setupSlotsSelection()
    {
        for (int i = 0; i < images.Length; i++)
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
        Rect rect = base.GetWorldPositionRectForIndex(index);
        return ShrinkRect(rect, 0.25f);
    }
    

    protected void deselectSlot(int index)
    {
        images[index].color = new Color32(219, 219, 219, 255);
    }

    protected void selectSlot(int index)
    {
        images[index].color = new Color32(145, 255, 126, 255);
    }

    protected override void Update()
    {
        base.Update();
        updateSlotsSelection();
    }
}