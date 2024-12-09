using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushableDrawers : InteractableObject
{
    private Animation pushDrawersAnim;

    private bool isItPushed = false;
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        if (!isItPushed)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Push The Drawers?"));
        }
        return interactionOptionInstances;
    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PULL && !isItPushed)
        {
            pushDrawersAnim.Play("DrawerPushAnim");
            isItPushed = true;
        }
    }

    public override bool CanBeSelected()
    {
        return !isItPushed;
    }

    protected override void Start()
    {
        base.Start();

        pushDrawersAnim = GetComponent<Animation>();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("PlayerInventory not found");
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}



