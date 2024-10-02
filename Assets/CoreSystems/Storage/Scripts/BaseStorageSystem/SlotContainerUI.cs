using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotContainerUI : MonoBehaviour
{
    [SerializeField, HideInInspector] protected SlotContainer container;
    public abstract Rect GetWorldPositionRectForIndex(int index);

    public SlotContainer GetContainer() { return container; }

    private HashSet<SlotItem> lastFrameItems = new HashSet<SlotItem>();
    private void createItemsUI()
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
}
