using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDrawer : InteractableObject
{
    private InteractablePlanks planksbool;
    private InteractionUIController interactionUIController;
    private Animation drawerAnim;
    private Rigidbody drawerRb;


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

        if (option.option == InteractionOption.PULL && !isItOpened)
        {
            if (planksbool.topPlankPulled || planksbool.bottomPlankPulled)
            {
                drawerAnim.Play("DrawerAnim");
                drawerRb.isKinematic = false;
                isItOpened = true;
                Destroy(this.gameObject, 2f);
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "I Don't want to touch that if I don't have too!";
                // Invoke Clear message after the delay
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


