using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableBrick : InteractableItem
{
    private Rigidbody brickRb;
    private bool isPulled = false;

    [SerializeField] private Vector3 pullVector;

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        
        if (!isPulled)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull Brick"));
        }
        return interactionOptionInstances;
        
    }


    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PULL)
        {
            
            // TODO: Go to player inventory, add and destroy this item
            print("Pulled Brick");
            brickRb.AddForce(pullVector);
            isPulled = true;
            
        }
    }
    public override void SetSelected(bool selected)
    {
        if (isPulled)
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
        brickRb = GetComponent<Rigidbody>();
        
    }

    // Update is called once per frame
   protected override void Update()
    {
        base.Update();
    }
}
