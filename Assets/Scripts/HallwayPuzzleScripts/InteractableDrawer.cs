using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDrawer : InteractableObject
{
    private InteractablePlanks planksbool;
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;
    private Animation drawerAnim;
    private Rigidbody drawerRb;

    private string crowbarBroke = "Damn, this crowbar is busted!";

    [SerializeField] private SlotItem Crowbar;
    private bool isItOpened = false;
    
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();

        if (!isItOpened)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull The Drawer?"));
        }
        return interactionOptionInstances;

    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);

        SlotItem selectedItem = inventory.GetSelectedItem();

        if (option.option == InteractionOption.PULL && !isItOpened)
        {
            if (selectedItem != null && selectedItem.Equals(Crowbar) && (planksbool.topPlankPulled || planksbool.bottomPlankPulled))
            {
                drawerAnim.Play("DrawerAnim");
                drawerRb.isKinematic = false;
                isItOpened = true;
                Destroy(this.gameObject, 2f);
                inventory.GetHotbarContainer().DeleteItem(inventory.SelectedSlot);
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = crowbarBroke;
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
            else if (selectedItem != null && selectedItem.Equals(Crowbar) && (!planksbool.topPlankPulled || !planksbool.bottomPlankPulled))
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "I Don't want to touch that if I don't have to!";
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
            else if ((selectedItem == null || !selectedItem.Equals(Crowbar)) && (planksbool.topPlankPulled || planksbool.bottomPlankPulled))
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "Maybe with the crowbar?!";
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "I Don't want to touch that if I don't have to!";
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        drawerRb = GetComponent<Rigidbody>();
        drawerAnim = GetComponent<Animation>();
        planksbool = FindObjectOfType<InteractablePlanks>();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        inventory = FindObjectOfType<PlayerInventory>();
    }
    protected override void Update()
    {
        base.Update();
        if (isItOpened)
        {
            drawerRb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX;
            drawerRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }

    }
}


