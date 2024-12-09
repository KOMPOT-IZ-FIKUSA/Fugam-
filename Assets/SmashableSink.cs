using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashableSink : InteractableObject
{
    private Animation sinkAnim;

    public bool isSmashed = false;
    private string sinkHint = "I wonder if I could smash this with something?!";

    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;

    [SerializeField] private SlotItem axe;

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        if (!isSmashed)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Smash The Sink?"));
        }

        return interactionOptionInstances;
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        SlotItem selectedItem = inventory.GetSelectedItem();
        if (option.option == InteractionOption.PULL && !isSmashed)
        {
            if (selectedItem != null && selectedItem.Equals(axe))  // Check if selected item is axe
            {
                sinkAnim.Play("SinkAnim");
                isSmashed = true;
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = sinkHint;
                // Invoke Clear message after the delay
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
        }
    }
    public override bool CanBeSelected()
    {
        return !isSmashed;
    }
    protected override void Start()
    {
        base.Start();
        sinkAnim = GetComponent<Animation>();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        inventory = FindObjectOfType<PlayerInventory>();
    }

    protected override void Update()
    {
        base.Update();
    }
}
