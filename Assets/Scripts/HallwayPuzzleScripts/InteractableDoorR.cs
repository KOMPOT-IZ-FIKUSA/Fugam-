using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoorR : InteractableObject
{
    private Animation doorAnim;
    
    private bool doorOpened = false;
    private string doorHint = "The door is locked!";
    private string doorKeyBroke = "The key broke!";
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;

    [SerializeField] private SlotItem keyItem;

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        if (!doorOpened)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Unlock Door?"));
        }
        return interactionOptionInstances;
    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);

        SlotItem selectedItem = inventory.GetSelectedItem();

        if (selectedItem != null && selectedItem.Equals(keyItem)) 
        {
            doorAnim.Play("DoorRightSideAnim");
            doorOpened = true;

            inventory.GetHotbarContainer().DeleteItem(inventory.SelectedSlot);
            inventory.SelectedSlot = 0;
            interactionUIController.hints.enabled = true;
            interactionUIController.hints.text = doorKeyBroke;
            interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
        }
        else
        {
            interactionUIController.hints.enabled = true;
            interactionUIController.hints.text = doorHint;
            interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
        }
    }
    public override bool CanBeSelected()
    {
        return !doorOpened;
    }

    protected override void Start()
    {
        base.Start();
        doorAnim = GetComponent<Animation>();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        inventory = FindObjectOfType<PlayerInventory>();
    }

    protected override void Update()
    {
        base.Update();
        
    }
}
