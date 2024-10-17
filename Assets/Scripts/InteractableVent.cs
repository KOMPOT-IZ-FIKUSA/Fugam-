using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVent : InteractableObject
{
    private Rigidbody ventRb;
    private bool isOpened = false;
    private string ventHint = "You are missing a tool to open this!";
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;

    [SerializeField] private Vector3 pullVector;
    [SerializeField] private SlotItem screwdriver;

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

        SlotItem selectedItem = inventory.GetSelectedItem();

        if (option.option == InteractionOption.PULL && !isOpened)
        {
            if (selectedItem != null && selectedItem.Equals(screwdriver))  // Check if selected item is screwdriver
            {
                ventRb.AddForce(pullVector);
                isOpened = true;
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = ventHint;
                // Invoke Clear message after the delay
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
        }
    }
    public override bool CanBeSelected()
    {
        return !isOpened;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        ventRb = GetComponent<Rigidbody>();
        interactionUIController = FindObjectOfType<InteractionUIController>();

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
