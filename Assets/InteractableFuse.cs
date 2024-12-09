using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableFuse : InteractableObject
{
    private PlayerInventory inventory;
    private SmashableSink smashableSink;
    [SerializeField] public SlotItem fuseItemSource;
    private bool pickedUpFuse = false;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> options = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem() && smashableSink.isSmashed)
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up Fuse?"));
        }
        else
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Inventory full"));
        }
        return options;
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PICK_UP && !pickedUpFuse)
        {
            if (inventory.CanAddItem())
            {
                inventory.AddItem(fuseItemSource.Copy());
                pickedUpFuse = true;
                Destroy(gameObject);
            }
        }
    }
    public override bool CanBeSelected()
    {
        return !pickedUpFuse && smashableSink.isSmashed;
    }

    protected override void Start()
    {
        base.Start();
        inventory = FindObjectOfType<PlayerInventory>();
        smashableSink = FindObjectOfType<SmashableSink>();
    }

    protected override void Update()
    {
        base.Update();
    }   


}
