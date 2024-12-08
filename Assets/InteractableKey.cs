using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : InteractableObject
{
    private PlayerInventory inventory;
    [SerializeField] public SlotItem KeyItemSource;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> options = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem())
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up Key?"));
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
        if (option.option == InteractionOption.PICK_UP)
        {
            if (inventory.CanAddItem())
            {
                inventory.AddItem(KeyItemSource.Copy());
                Destroy(gameObject);
            }
        }
    }
    protected override void Start()
    {
        base.Start();
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("No PlayerInventory found in scene");
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}

