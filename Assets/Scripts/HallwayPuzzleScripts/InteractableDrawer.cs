using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDrawer : InteractableObject
{
    private InteractablePlanks planksbool;
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
               
        }
    }
    protected override void Start()
    {
        base.Start();
        drawerRb = GetComponent<Rigidbody>();
        drawerAnim = GetComponent<Animation>();
        planksbool = FindObjectOfType<InteractablePlanks>();

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


