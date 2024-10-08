using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableVent : InteractableObject
{
    private Rigidbody ventRb;
    private bool isOpened = false;

    [SerializeField] private Vector3 pullVector;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();

        if (!isOpened)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull Brick"));
        }
        return interactionOptionInstances;

    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        //TODO: If selected item is screwdriver open vent / else unable to open vent!
        if (option.option == InteractionOption.PULL)
        {

            // TODO: Go to player inventory, add and destroy this item
            print("Opened Vent");
            ventRb.AddForce(pullVector);
            isOpened = true;

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

    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
