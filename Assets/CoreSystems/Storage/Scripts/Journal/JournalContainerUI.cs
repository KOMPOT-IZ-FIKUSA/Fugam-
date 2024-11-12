using UnityEngine;
using UnityEngine.UI;

public class JournalContainerUI : SlotContainerUI
{
    protected override void OnEnable()
    {
        base.OnEnable();
        // makes sure that the container is there
        if (container == null)
        {
            if (inventory == null)
            {
                inventory = FindObjectOfType<PlayerInventory>();
            }

            if (inventory != null)
            {
                container = inventory.GetJournalContainer();
            }
            else
            {
                Debug.LogError("PlayerInventory not found when initializing JournalContainerUI.");
                return;
            }
        }
        createItemsUI();
    }

    private PlayerInventory inventory;

    private void Awake()
    {
        if (inventory == null)
        {
            inventory = FindObjectOfType<PlayerInventory>();
        }

        if (container == null)
        {
            container = inventory.GetJournalContainer();
        }

    }


}
