
using System;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
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

    [SerializeField] protected RectTransform[] SlotsTransforms;

    /// <returns>The position on the screen in which the object should be displayed. The geometry of the slot.</returns>
    public virtual Rect GetWorldPositionRectForIndex(int index) {

        if (SlotsTransforms.Length != container.GetCapability())
        {
            // Error example: 'Found 0 slot position objects for 9 slots in the container.'
            Debug.LogError($"Error: Found {SlotsTransforms.Length} slot position objects for {container.GetCapability()} slots in the container.");
        }

        if (index < 0 || index >= SlotsTransforms.Length)
        {
            Debug.LogError($"Invalid index {index}. Must be between 0 and {SlotsTransforms.Length - 1}");
            return Rect.zero; // It returns a default value or handle this case appropriately.
        }
        RectTransform rectTransform = SlotsTransforms[index].GetComponent<RectTransform>();
        return RectTransformToWorldPositionRect(rectTransform);
    }


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
        validateSlots();
    }
    
    public void ForceUpdateUI()
    {
        createItemsUI();
    }

    public static Rect RectTransformToWorldPositionRect(RectTransform rectTransform)
    {
        Vector2 leftDown = rectTransform.TransformPoint(rectTransform.rect.position);
        Vector2 rightUp = rectTransform.TransformPoint(rectTransform.rect.position + rectTransform.rect.size);
        return new Rect(leftDown, rightUp - leftDown);
    }

    public static Rect ShrinkRect(Rect rect, float alpha)
    {
        float shrinkAlpha = 0.25f;
        Vector2 newSize = rect.size * (1 - shrinkAlpha);
        Vector2 newLeftDown = rect.center - newSize / 2;
        return new Rect(newLeftDown, newSize);
    }
    
    public bool IsMouseOverSlot(int index)
    {
        Rect slotWorldPosition = GetWorldPositionRectForIndex(index);
        return slotWorldPosition.Contains(Input.mousePosition);
    
    }

    protected virtual void validateSlots()
    {
        for (int i = 0; i < SlotsTransforms.Length; i++)
        {
            if (SlotsTransforms[i] == null)
            {
                Debug.LogError($"Hotbar slot {i} is null");
            }
        }
    }

    public int GetSlotsCount()
    {
        if (SlotsTransforms.Length != container.GetCapability())
        {
            // Error example: 'Found 0 slot position objects for 9 slots in the container.'
            Debug.LogError($"Error: Found {SlotsTransforms.Length} slot position objects for {container.GetCapability()} slots in the container.");
        }
        return SlotsTransforms.Length;
    }
   
}
