using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableScrewdriver : InteractableObject
{
    
    private PlayerInventory inventory;
    [SerializeField] public SlotItem screwdriverItemSource;

    private void Awake()
    {
         
    }
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> options = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem())
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up"));
        } else
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
                inventory.AddItem(screwdriverItemSource.Copy());
                GameObject.Destroy(this.gameObject);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("Cannot find player inventory");
        }
        screwdriverItemSource.itemName = "Screwdriver";
    }
    protected override void Update()
    {
        base.Update();
    }
}
