using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableObject : MonoBehaviour
{
    /// <summary>
    /// The outline component used to handle selection. Optional.
    /// </summary>
    protected Outline outline;
    public PlayerInteractController playerController { get; private set; }

    public Camera playerCamera => playerController.GetPlayerCamera();

    /// <summary>
    /// The transform which is used to calculate an angle for angular selection. If the player looks at this point, then the angle = 0 and the object most ikely will be selected.
    /// </summary>
    [Header("Transform of the point used for angular selection")]
    public Transform AngularSelectionCenter;

    /// <summary>
    /// The anchor for the UI label. The label like "[E] Interact" will be bound to this point after its projection to screen space.
    /// </summary>
    [Header("Transform of the point used for UI interaction label")]
    public Transform UILabelTransform;

    /// <summary>
    /// The maximum distance to the point of raycast collision when the object is raycasted from camera.
    /// </summary>
    [Header("Maximum distance for raycasting selection")]
    [SerializeField, Range(0, 5)] protected float raycastSelectionRange = 3;

    /// <summary>
    /// The maximum distance to the AngularSelectionCenter when the object is selected by angular selector.
    /// </summary>
    [Header("Maximum distance for angular selection")]
    [SerializeField, Range(0, 5)] protected float angularSelectionRange = 3;

    /// <summary>
    /// The maximum deviation between a centralray from the camera and 
    /// </summary>
    [Header("Maximum angle difference for angular selection")]
    [SerializeField, Range(0, 180)] protected float angularSelectionAngle = 15;

    public float GetRaycastSelectionRange() {  return raycastSelectionRange; }
    public float GetAngularSelectionRange() {  return angularSelectionRange; }
    public float GetAngularSelectionAngle() {  return angularSelectionAngle; }

    public virtual bool CanBeSelected()
    {
        return true;
    }

    protected virtual void Awake()
    {
        outline = GetComponent<Outline>();
        if (outline is null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5;
        }
        playerController = FindObjectOfType<PlayerInteractController>();
        if (playerController == null)
        {
            throw new MissingComponentException(typeof(PlayerInteractController).Name);
        }
    }

    /// <summary>
    /// Sets up an outline and finds PlayerInteractController
    /// </summary>
    protected virtual void Start()
    {


    }

    protected virtual void OnEnable()
    {

        playerController.StartTrackingObject(this);
    }

    protected virtual void OnDisable()
    {
        playerController.StopTrackingObject(this);
    }
    protected virtual void Update()
    {

    }

    public virtual void SetSelected(bool selected)
    {
        if (selected && !CanBeSelected())
        {
            Debug.LogError("Strange behavior: Attempt to select an unselectable object");
        }
        outline.enabled = selected;
    }


    /// <summary>
    /// The method to handle interactions applied to an object.
    /// </summary>
    /// <param name="option">One of the return values of GetAvailableOptions()</param>
    public virtual void Interact(InteractionOptionInstance option)
    {

    }

    /// <summary>
    /// The method to define which interactions can be applied to this object.
    /// </summary>
    /// <returns>A list of interactions.</returns>
    public abstract List<InteractionOptionInstance> GetAvailabeleOptions();

    /// <returns>The world position of anchoring point for interaction UI label</returns>
    public Vector3 GetUILabelPosition()
    {
        if (UILabelTransform == null)
        {
            return transform.position;
        }
        else
        {
            return UILabelTransform.position;
        }
    }


    /// <returns>The world position of point for angular selection (Look for AngularInteractionSelector)</returns>
    public Vector3 GetAngularSelectionCenter()
    {
        if (AngularSelectionCenter == null)
        {
            return transform.position;
        } else
        {
            return AngularSelectionCenter.position;
        }
    }
}
