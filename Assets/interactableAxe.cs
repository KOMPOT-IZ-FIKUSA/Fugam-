using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactableAxe : InteractableObject
{
    private PlayerInventory inventory;
    [SerializeField] public SlotItem axeItemSource;
    private bool pickedUpAxe = false;

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> options = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem())
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up Axe?"));
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
        if (option.option == InteractionOption.PICK_UP && !pickedUpAxe)
        {
            if (inventory.CanAddItem())
            {
                inventory.AddItem(axeItemSource.Copy());
                Destroy(gameObject);
                pickedUpAxe = true;
            }
        }
    }
    
    protected override void Start()
    {
        base.Start();
        inventory = FindObjectOfType<PlayerInventory>();
    }
    protected override void Update()
    {
        base.Update();
    }
}
