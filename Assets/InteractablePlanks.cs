using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePlanks : InteractableObject
{
    private Animation myPlankAnimations;
    private Rigidbody plankRb;
    public bool topPlankPulled = false;
    public bool bottomPlankPulled = false;
    private string planksHint = "You need a tool to remove the planks!";
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;

    [SerializeField] private SlotItem Crowbar;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();

        if (!topPlankPulled || !bottomPlankPulled)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull Planks?"));
        }
        return interactionOptionInstances;
    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);

        SlotItem selectedItem = inventory.GetSelectedItem();

        if (option.option == InteractionOption.PULL)
        {
            if (selectedItem != null && selectedItem.Equals(Crowbar))  // Check if selected item is screwdriver
            {
                if (this.gameObject.tag == "TopPlank" && !topPlankPulled)
                {
                    topPlankPulled = true;
                    myPlankAnimations.Play("PlankTopAnim");
                    plankRb.isKinematic = false;
                }
                else if (this.gameObject.tag == "BottomPlank" && !bottomPlankPulled)
                {
                    bottomPlankPulled = true;
                    myPlankAnimations.Play("PlankBottomAnim");
                    plankRb.isKinematic = false;
                }
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = planksHint;
                // Invoke Clear message after the delay
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
        }
    }
    public override bool CanBeSelected()
    {
        return !bottomPlankPulled && !topPlankPulled;

    }

    protected override void Start()
    {
        base.Start();

        myPlankAnimations = GetComponent<Animation>();
        plankRb = GetComponent<Rigidbody>();
        inventory = FindObjectOfType<PlayerInventory>();
        interactionUIController = FindObjectOfType<InteractionUIController>();

        if (inventory == null)
        {
            Debug.LogError("PlayerInventory not found in the scene!");
        }

        if (interactionUIController == null)
        {
            Debug.LogError("InteractionUIController not found in the scene!");
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
