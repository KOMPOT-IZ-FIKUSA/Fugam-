using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : InteractableObject
{
    private bool isOpened = false;
    private string exitHint = "The Power is not on!";
    private InteractionUIController interactionUIController;
    private PlayerInventory inventory;
    private GeneratorControl generatorControlScript;


    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        if (!isOpened)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Open Exit Door?"));
        }
        return interactionOptionInstances;
    }
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PULL && !isOpened)
        {
            if (generatorControlScript.lightsOn)
            {
                isOpened = true;
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = "YOU ESCAPED!";
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
            else
            {
                interactionUIController.hints.enabled = true;
                interactionUIController.hints.text = exitHint;
                interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
            }
        }
    }

    protected override void Start()
    {
        base.Start();
        interactionUIController = FindObjectOfType<InteractionUIController>();
        generatorControlScript = FindObjectOfType<GeneratorControl>();
    }
    protected override void Update()
    {
        base.Update();
    }
}
