using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInteractController : MonoBehaviour
{
    /*
     * List of interactable objects that are in the scene
     * Every time an object is spawned it registers in StartTrackingObject
     * Every time an object leaves the scene it stops tracking
     */
    [SerializeField] private List<InteractableObject> trackedObjects = new List<InteractableObject>();

    /// <returns>A readonly collection of InteractableObject's that are tracked</returns>
    public ReadOnlyCollection<InteractableObject> GetAllObjects()
    {
        return trackedObjects.AsReadOnly();
    }

    [SerializeField] private Camera playerCamera;


    private InteractionUIController interactionUIController;

    // Two selectors to find an object to select
    private RaycastInteractableSelector raycastSelector;
    private AngularInteractionSelector angularSelector;

    // An object that is currently selected. (null if nothing selected)
    private InteractableObject selectedObject;

    public Camera GetPlayerCamera() { return playerCamera; }
    public InteractableObject GetSelectedObject() { return selectedObject; }
    
    private void Start()
    {
        interactionUIController = FindObjectOfType<InteractionUIController>();
        if (interactionUIController == null)
        {
            throw new MissingComponentException(typeof(InteractionUIController).Name);
        }
        raycastSelector = GetComponent<RaycastInteractableSelector>();
        if (raycastSelector == null)
        {
            throw new MissingComponentException(typeof(RaycastInteractableSelector).Name);
        }
        angularSelector = GetComponent<AngularInteractionSelector>();
        if (angularSelector == null)
        {
            throw new MissingComponentException(typeof(AngularInteractionSelector).Name);
        }
    }

    void Update()
    {
        SetSelectedObject(FindSelectedObject());

        if (selectedObject != null)
        {
            foreach (InteractionOptionInstance option in selectedObject.GetAvailabeleOptions())
            {
                if (Input.GetKeyDown(OptionsKeyMap.map[option.option].keyCode))
                {
                    selectedObject.Interact(option);
                }
            }

        }
    }

    /// <summary>
    /// Find a selected object from two selectors.
    /// </summary>
    /// <returns>Returns the closest selected object.</returns>
    private InteractableObject FindSelectedObject()
    {
        // If one of the objects is null, the distance is infinity.
        // If both objects are null, both distances are infinity => return null
        // If one of the objects is null, return another one
        // If both objects != null, select the closest object
        if (raycastSelector.GetDistanceToSelected() < angularSelector.GetDistanceToSelected())
        {
            return raycastSelector.GetSelectedObject();
        } else
        {
            return angularSelector.GetSelectedObject();
        }
    }

    /// <summary>
    /// Sets the current selected object to the argument and handles the selection/deselection cases.
    /// </summary>
    private void SetSelectedObject(InteractableObject obj)
    {
        if (obj == selectedObject)
        {
            return;
        }

        if (selectedObject != null)
        {
            selectedObject.SetSelected(false);

        }

        if (obj != null)
        {
            obj.SetSelected(true);
        }

        selectedObject = obj;
    }

    /// <summary>
    /// Should be called when a new InteractableObject appears in the world. Adds an object to the list of tracked InteractableObject's
    /// </summary>
    public void StartTrackingObject(InteractableObject obj)
    {
        if (trackedObjects.Contains(obj))
        {
            Debug.LogError("Strange behavior: trying to start tracking an obj that is already registered");
        }
        trackedObjects.Add(obj);
        obj.SetSelected(false);
    }

    /// <summary>
    /// Should be called when an InteractableObject is destroyed.
    /// </summary>
    public void StopTrackingObject(InteractableObject obj)
    {
        bool success = trackedObjects.Remove(obj);
        if (!success)
        {
            Debug.LogError("Strange behavior: trying to stop tracking an unknown obj");
        }
        // Handle a case if object was selected before, but now it is being destroyed
        if (selectedObject == obj)
        {
            SetSelectedObject(null);
        }
        obj.SetSelected(false);
    }

}
