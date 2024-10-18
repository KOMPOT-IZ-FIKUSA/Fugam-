
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

    protected abstract Image[] Slots { get; }
 
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
    
    public void ForceUpdateUI()
    {
        createItemsUI();
    }
    
    public bool IsMouseOverSlot(int index)
    {
        Image[] slots = Slots;
        if (index < 0 || index >= slots.Length)
        {
            Debug.LogError($"Invalid slot index: {index}");
            return false;
        }

        RectTransform rectTransform = slots[index].GetComponent<RectTransform>();
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            Input.mousePosition,
            null, // For Screen Space - Overlay canvases
            out var localMousePos
        );

        return rectTransform.rect.Contains(localMousePos);
    
    }
   
}
