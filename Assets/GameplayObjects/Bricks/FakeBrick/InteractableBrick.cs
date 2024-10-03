using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// A brick that can be pulled one time.
/// </summary>
public class InteractableBrick : InteractableObject
{
    private Rigidbody brickRb;

    [SerializeField, HideInInspector] private bool isPulled = false;

    [Header("The force in global coordinates that is applied on pulling.")]
    [SerializeField] private Vector3 pullVector;

    /// <summary>
    /// Returns a pulling option if the object hasn't been pulled yet
    /// </summary>
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {

        List<InteractionOptionInstance> interactionOptionInstances = new List<InteractionOptionInstance>();
        if (!isPulled)
        {
            interactionOptionInstances.Add(new InteractionOptionInstance(InteractionOption.PULL, "Pull Brick"));
        }
        return interactionOptionInstances;
    }

    /// <summary>
    /// Handled the interaction of pulling
    /// </summary>
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PULL)
        {
            brickRb.AddForce(pullVector, ForceMode.Acceleration);
            isPulled = true;
            GetComponent<Animation>().Play();
        }
    }

    protected override void Start()
    {
        base.Start();
        brickRb = GetComponent<Rigidbody>();
        
    }

   protected override void Update()
    {
        base.Update();
    }

    /// <summary>
    /// The brick can be selected if it hasn't been pulled yet
    /// </summary>
    public override bool CanBeSelected()
    {
        return !isPulled;
    }
}
