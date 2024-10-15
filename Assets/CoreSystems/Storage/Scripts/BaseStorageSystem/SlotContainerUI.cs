
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Base class to handle user interface for a SlotItem.
/// This class is responsible for drawing the container and creating UI instances for slots (look void createItemsUI).
/// </summary>
public abstract class SlotContainerUI : MonoBehaviour
{
    /// <summary>
    /// The container that is drawn by this UI handler
    /// </summary>
    protected SlotContainer container;
    protected abstract Image[] GetSlots();

    
    private SlotItem draggedItem = null;
    private int draggedItemIndex = -1;

    /// <returns>The position on the screen in which the object should be displayed. The geometry of the slot.</returns>
    public abstract Rect GetWorldPositionRectForIndex(int index);

    public SlotContainer GetContainer() { return container; }

    /// <summary>
    /// A set of items that were in the container last frame
    /// </summary>
    protected HashSet<SlotItem> lastFrameItems = new HashSet<SlotItem>();

    /// <summary>
    /// For every item that didn't exist in the container the last frame, but appeared now creates a UI object to handle the item.
    /// </summary>
    protected void createItemsUI()
    {
        HashSet<SlotItem> items = new HashSet<SlotItem>();
        for (int i = 0; i < container.GetCapability(); i++)
        {
            SlotItem item = container.GetItem(i);
            if (item != null)
            {
                items.Add(item);
                if (!lastFrameItems.Contains(item))
                {
                    item.CreateUI(this);
                }
            }
        }
        lastFrameItems = items;
    }

    protected virtual void Update()
    {
        createItemsUI();
    }

    public void StartDraggingItem(int slotIndex)
    {
        Image[] slots = GetSlots();  // Get the slots array from the derived class

        // Check if the slot index is valid
        if (slotIndex < 0 || slotIndex >= slots.Length)
        {
            Debug.LogError($"Invalid slot index: {slotIndex}. Slots array length: {slots.Length}");
            return;
        }

        SlotItem item = container.GetItem(slotIndex);
        if (item != null)
        {
            draggedItem = item;
            draggedItemIndex = slotIndex;
            container.SetItem(slotIndex, null);  // Remove the item from the original slot
        }
        else
        {
            Debug.Log("No item found in this slot");
        }
    }

    public void StopDraggingItem(int slotIndex)
    {
        if (draggedItem != null)
        {
            if (container.GetItem(slotIndex) == null)  // Check if the target slot is empty
            {
                Debug.Log($"Dropping {draggedItem.name} into slot {slotIndex}");
                container.SetItem(slotIndex, draggedItem);  // Assign the dragged item to the new slot
                draggedItem = null;
                draggedItemIndex = -1;
            }
            else
            {
                Debug.Log($"Slot {slotIndex} is already occupied, returning item to original slot {draggedItemIndex}");
                container.SetItem(draggedItemIndex, draggedItem);  // Return the item to its original slot
                draggedItem = null;
                draggedItemIndex = -1;
            }

            // Optionally, refresh the UI to reflect the new state
            //updateSlotUI(slotIndex);  // Example method to force the UI to update
        }
    }

    public void CancelDragging()
    {
        if (draggedItem != null)
        {
            container.SetItem(draggedItemIndex, draggedItem); // it will send back the item to the original slot
            draggedItem = null;
            draggedItemIndex = -1;
        }
    }
    
    public void ForceUpdateUI()
    {
        createItemsUI();
    }
   
}
