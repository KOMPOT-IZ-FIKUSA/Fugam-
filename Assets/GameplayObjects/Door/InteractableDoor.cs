using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// A class to handle a door.
/// TODO: Animation-based state change
/// The class is not done
/// </summary>
public class InteractableDoor : InteractableObject
{

    [SerializeField] private Transform door;
    private bool doorOpened = false;
    [SerializeField] private SlotItem key;

    PlayerInventory inventory; //ACCESS INVENTORY
    InteractionUIController interactionUIController; //INTERACTIONUI for our hint message
    private Animation doorAnim;

    protected override void Start()
    {
        base.Start();

        //Assign the inventory and interaction
        inventory = FindObjectOfType<PlayerInventory>();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        doorAnim = GetComponent<Animation>();
    }
    protected override void Update()
    {
        base.Update();


    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.OPEN && !doorOpened)
        {
            //Same idea as the vent, get selected slot item
            SlotItem selectedItem = inventory.GetSelectedItem();

            //Checks if it is equal to a key
            if (selectedItem != null && selectedItem.Equals(key))
            {
                doorAnim.Play("DoorAnim");
                doorOpened = true;
            }
            else
            {
                //It will give a message saying we need a key
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "You need a key to open this door!";
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }

        }
        else if (option.option == InteractionOption.CLOSE)
        {
            doorAnim.Play("DoorCloseAnim");
            doorOpened = false;
        }
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> result = new List<InteractionOptionInstance>();


        if (doorOpened)
        {
            result.Add(new InteractionOptionInstance(InteractionOption.CLOSE, "Close door"));
        }
        if (!doorOpened)
        {
            result.Add(new InteractionOptionInstance(InteractionOption.OPEN, "Open door"));
        }
        return result;
    }
}
