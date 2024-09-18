using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractController : MonoBehaviour
{
    /*
     * List of interactable items that are in the scene
     * Every time an item is spawned it registers in StartTrackingItem
     * Every time an item leaves the scene it stops tracking
     */
    private List<InteractableItem> trackedItems = new List<InteractableItem>();
    [SerializeField] private Camera camera;

    [SerializeField] private float selectedItemMaxAngle = 15;

    private InteractableItem selectedItem;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelectedItem(FindSelectedItem());
    }

    private InteractableItem FindSelectedItem()
    {
        float maxAngleCos = Mathf.Cos(selectedItemMaxAngle / 180 * Mathf.PI);
        InteractableItem foundItem = null;

        float bestMatch = -2;

        Vector3 cameraPos = camera.transform.position;
        Vector3 cameraDirection = camera.transform.rotation * Vector3.forward;
        foreach (InteractableItem item in trackedItems)
        {

            // Check if the object is close enough
            float distance = Vector3.Distance(item.transform.position, cameraPos);
            if (distance > item.GetInteractionRange())
            {
                continue;
            }

            // Calculate dot product between camera direction vector and object direction vector
            Vector3 directionToItem = (item.transform.position - cameraPos).normalized;
            float dotProduct = Vector3.Dot(cameraDirection, directionToItem);

            // Check if the object is within the range
            if (dotProduct < maxAngleCos)
            {
                continue;
            }

            // If this is the best match, update the bestMatch and foundItem
            if (dotProduct > bestMatch)
            {
                bestMatch = dotProduct;
                foundItem = item;
            }
        }
        return foundItem;
    }

    private void UpdateSelectedItem(InteractableItem item)
    {
        if (item == selectedItem)
        {
            return;
        }

        if (selectedItem != null)
        {
            selectedItem.SetSelected(false);
        }

        if (item != null)
        {
            item.SetSelected(true);
        }

        selectedItem = item;
    }

    public void StartTrackingItem(InteractableItem item)
    {
        if (trackedItems.Contains(item))
        {
            throw new System.Exception("Strange behavior: trying to start tracking an item that is already registered");
        }
        trackedItems.Add(item);
        item.SetSelected(false);
    }

    public void StopTrackingItem(InteractableItem item)
    {
        bool success = trackedItems.Remove(item);
        if (!success)
        {
            throw new System.Exception("Strange behavior: trying to stop tracking an unknown item");
        }
        if (selectedItem == item)
        {
            selectedItem = null;
        }
        item.SetSelected(false);
    }
}
