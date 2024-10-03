using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class to handle a door.
/// TODO: Animation-based state change
/// The class is not done
/// </summary>
public class InteractableDoor : InteractableObject
{

    [SerializeField] private Transform door;
    [SerializeField] private Vector3 closedRotation;
    [SerializeField] private Vector3 openedRotationY;
    [SerializeField] private bool isOpen;

    [SerializeField] private AnimationCurve animationCurve;
    [SerializeField, HideInInspector] private float animationTime;
    [SerializeField, HideInInspector] private bool animating;



    protected override void Start()
    {
        throw new System.NotImplementedException("Door code has to be finished to apply the component");
        base.Start();
        SetOpen(false);
    }


    public void SetOpen(bool isOpen)
    {
        if (isOpen == this.isOpen)
        {
            return;
        }
        animationTime = 0;
        animating = true;
        this.isOpen = isOpen;
    }

    protected override void Update()
    {
        base.Update();

        if (animating)
        {
            UpdateAnimationTime();
            SetAnimatedDoorAngle();
        }
    }

    private void UpdateAnimationTime()
    {
        animationTime += Time.deltaTime;
        if (animationTime > 1)
        {
            animationTime = 1;
            animating = false;
        }
    }


    private void SetAnimatedDoorAngle()
    {
        float alpha = animationCurve.Evaluate(animationTime);
        if (isOpen)
        {
            alpha = 1 - alpha;
        }
        Vector3 interpolatedAngle = Vector3.Lerp(openedRotationY, closedRotation, alpha);
        door.rotation = Quaternion.Euler(interpolatedAngle);
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.OPEN)
        {
            SetOpen(true);
        }
        else if (option.option == InteractionOption.CLOSE)
        {
            SetOpen(false);
        }
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> result = new List<InteractionOptionInstance>();
        if (animating)
        {
            return result;
        }

        if (isOpen)
        {
            result.Add(new InteractionOptionInstance(InteractionOption.CLOSE, "Close door"));
        }
        else
        {
            result.Add(new InteractionOptionInstance(InteractionOption.OPEN, "Open door"));
        }
        return result;
    }
}
