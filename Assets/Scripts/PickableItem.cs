using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableItem : InteractableItem
{

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PICK_UP)
        {
            // TODO: Go to player inventory, add and destroy this item
            print("Picked up an item!");
            Destroy(gameObject);
        }
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> result = new List<InteractionOptionInstance>();
        result.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up"));
        return result;
    }

}
