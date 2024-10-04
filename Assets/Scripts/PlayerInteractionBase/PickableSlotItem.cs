using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A base class for any pickable item that is stored in inventory / any slot container.
/// </summary>
public class PickableSlotItem : InteractableObject
{

    [SerializeField] private SlotItem item;

    private PlayerInventory inventory;

    protected override void Start()
    {
        base.Start();
        inventory = GetComponent<PlayerInventory>();
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PICK_UP)
        {
            if (inventory.CanAddItem())
            {
                inventory.AddItem(item);
                Destroy(gameObject);
            }
        }
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> result = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem())
        {
            result.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up"));
        }
        return result;
    }

}
