using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A component that selects the object by calculating deviation between camera look direction and direction vector between camera and object.
/// </summary>
[RequireComponent(typeof(PlayerInteractController))]
public class AngularInteractionSelector : MonoBehaviour
{
    private PlayerInteractController controller;

    /// <summary>
    /// The object that is selected in Update()
    /// </summary>
    [SerializeField, HideInInspector] private InteractableObject selectedObject;

    /// <summary>
    /// Distance to the selected object. If seectedObject = null, this distance = float.positiveInvfinity
    /// </summary>
    [SerializeField, HideInInspector] private float distanceToSelected;

    public InteractableObject GetSelectedObject() { return selectedObject; }


    /// <summary>
    /// Distance to the selected object. If seectedObject = null, this distance = float.positiveInvfinity
    /// </summary>
    public float GetDistanceToSelected() { return selectedObject == null ? float.PositiveInfinity : distanceToSelected; }

    void Start()
    {
        controller = GetComponent<PlayerInteractController>();
    }

    void Update()
    {
        
        if (controller == null)
        {
            Debug.LogError("Cannot find interaction controller");
            return;
        }
        
        selectedObject = null;
        distanceToSelected = float.PositiveInfinity;

        Camera camera = controller.GetPlayerCamera();
        Vector3 cameraPos = camera.transform.position;
        Vector3 cameraDirection = camera.transform.rotation * Vector3.forward;
        // Iterate over all objects to calculate deviation and distance to each
        foreach (InteractableObject obj in controller.GetAllObjects())
        {
            // Check if the object is close enough
            float distance = Vector3.Distance(obj.transform.position, cameraPos);
            if (distance > obj.GetAngularSelectionRange())
            {
                continue;
            }

            // Calculate dot product between camera direction vector and object direction vector
            Vector3 directionToItem = obj.GetAngularSelectionCenter() - cameraPos;
            
            // If position of the obj equals to the position of the camera
            if (directionToItem == Vector3.zero)
            {
                selectedObject = obj;
                distance = 0;
            }

            // Otherwise normalize and calculate the angle
            directionToItem = directionToItem.normalized;
            float cosine = Vector3.Dot(cameraDirection, directionToItem);
            float angle = Mathf.Acos(cosine); // always positive by the definition of arccos
            angle = angle / Mathf.PI * 180;

            // If angle is too big for the obj, skip it
            if (angle > obj.GetAngularSelectionAngle())
            {
                continue;
            }

            // If before a better match was found, skip this
            if (angle > distanceToSelected)
            {
                continue;
            }

            // If obj can't be selected, skip it
            if (!obj.CanBeSelected())
            {
                continue;
            }

            if (checkObjectsBetweenCameraAndInteractable(camera, obj))
            {
                continue;
            }

            selectedObject = obj;
            distanceToSelected = distance;
        }
    }

    private static bool checkObjectsBetweenCameraAndInteractable(Camera camera, InteractableObject obj)
    {
        
        Vector3 start = camera.transform.position;
        Vector3 end = obj.transform.position;
        Vector3 delta = end - start;
        Ray ray = new Ray(start, delta);
        RaycastHit result;
        bool success = Physics.Raycast(ray, out result, delta.magnitude);
        if (!success)
        {
            return false;
        }
        bool componentFound = result.collider.GetComponentInParent<InteractableObject>() == obj;
        
        return !componentFound;
    }
}
