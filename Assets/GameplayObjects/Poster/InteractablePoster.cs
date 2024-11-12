using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablePoster : InteractableObject
{
    private bool isPulled = false;

    private Animation posterAnim;
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();

        if (!isPulled)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull Poster?"));
        }
        return interactionOptionInstances;
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);

        if (option.option == InteractionOption.PULL && !isPulled)
        {
            posterAnim.Play();
            isPulled = true;
        }
    }
    public override bool CanBeSelected()
    {
        return !isPulled;
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        posterAnim = GetComponent<Animation>();
    }


    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
}
