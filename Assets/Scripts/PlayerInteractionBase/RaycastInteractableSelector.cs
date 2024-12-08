using UnityEngine;

[RequireComponent(typeof(PlayerInteractController))]
public class RaycastInteractableSelector : MonoBehaviour
{



    private PlayerInteractController controller;

    [SerializeField, HideInInspector] private InteractableObject selectedObject;

    [SerializeField, HideInInspector] private float distanceToSelected;

    [SerializeField] private RectTransform centerScreenReticle;

    private const float maximumPlayerReach = 10;

    public InteractableObject GetSelectedObject() { return selectedObject; }
    public float GetDistanceToSelected() { return selectedObject == null ? float.PositiveInfinity : distanceToSelected; }

    void Start()
    {
        controller = GetComponent<PlayerInteractController>();
        
    }


    void Update()
    {

        if (centerScreenReticle == null)
        {
            Debug.LogError("Cannot find the center of the screen");
            return;
        }
        if (controller == null)
        {
            Debug.LogError("Cannot find interaction controller");
            return;
        }

        selectedObject = null;
        distanceToSelected = float.PositiveInfinity;

        Vector3 reticlePositionOnScreen = centerScreenReticle.TransformPoint(Vector3.zero);
        Camera camera = controller.GetPlayerCamera();
        Ray ray = camera.ScreenPointToRay(reticlePositionOnScreen);
        Debug.DrawLine(ray.origin, ray.origin + ray.direction * maximumPlayerReach, Color.yellow, Time.deltaTime * 2);
        RaycastHit hitinfo;
        bool success = Physics.Raycast(ray, out hitinfo, maximumPlayerReach);
        if (!success) return;

        InteractableObject interactableObject = hitinfo.collider.GetComponentInParent<InteractableObject>();

        // Skip the collision if the object is not interractable
        if (interactableObject == null) return;

        // Skip the collision if it's farther than the last one found
        if (hitinfo.distance > distanceToSelected) return;

        // Skip the collision if it is further than the object can be interacted with
        if (hitinfo.distance > interactableObject.GetRaycastSelectionRange()) return;

        if (!interactableObject.CanBeSelected()) return;

        selectedObject = interactableObject;
        distanceToSelected = hitinfo.distance;
    }

    private static int compareDistances(RaycastHit hit1, RaycastHit hit2)
    {
        if (hit1.distance > hit2.distance)
        {
            return 1;
        }
        else if (hit1.distance < hit2.distance)
        {
            return -1;
        } else
        {
            return 0;
        }
    }

}
