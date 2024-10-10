using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVent : InteractableObject
{
    private Rigidbody ventRb;
    private bool isOpened = false;
    private string ventHint = "You are missing a tool to open this!";


    private InteractionUIController interactionUIController;
    [SerializeField] private Vector3 pullVector;
    private InteractableScrewdriver interactableScrewdriver;
    private PlayerInventory inventory;
    private SlotItem screwdriver;

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();

        if (!isOpened)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Open Vent?"));
        }
        return interactionOptionInstances;

    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);

        SlotItem selectedItem = inventory.GetHotbarContainer().GetItem(inventory.SelectedSlot);

        if (option.option == InteractionOption.PULL)
        {

            if (selectedItem != null && selectedItem.Equals(screwdriver))  // Check if selected item is screwdriver
            {
                print("Opened Vent");
                ventRb.AddForce(pullVector);
                isOpened = true;
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = ventHint;
                interactionUIController.Invoke("HintMessage", interactionUIController.hintDelay);
                
                Debug.Log("You need a screwdriver to open this vent.");
            }
        }
    }
    public override void SetSelected(bool selected)
    {
        if (isOpened)
        {
            base.SetSelected(false);
        }
        else
        {
            base.SetSelected(selected);
        }

    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ventRb = GetComponent<Rigidbody>();
        interactionUIController = FindObjectOfType<InteractionUIController>();

        //Finds screwdriver script
        interactableScrewdriver = FindObjectOfType<InteractableScrewdriver>();
        if (interactableScrewdriver != null)
        {
            // Reference the screwdriver item
            screwdriver = interactableScrewdriver.screwdriverItemSource;
            Debug.Log("Screwdriver item source: " + interactableScrewdriver);
        }
        else
        {
            
            Debug.LogError("Cannot find screwdriver item");
        }

        //Find player inventory
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("Cannot find player inventory");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    
    

}
