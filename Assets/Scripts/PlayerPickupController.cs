using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickupController : MonoBehaviour
{
    /*
     * List of pickable items that are in the scene
     * Every time an item is spawned it registers in StartTrackingItem
     * Every time an item leaves the scene it stops tracking
     */
    private List<PickableItem> trackedItems = new List<PickableItem>();
    [SerializeField] private Camera camera;

    [SerializeField] private float selectedItemMaxAngle = 15;

    private PickableItem selectedItem;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        UpdateSelectedItem(FindSelectedItem());
    }

    private PickableItem FindSelectedItem()
    {
        float maxAngleCos = Mathf.Cos(selectedItemMaxAngle / 180 * Mathf.PI);
        PickableItem foundItem = null;

        float bestMatch = -2;

        Vector3 cameraPos = camera.transform.position;
        Vector3 cameraDirection = camera.transform.rotation * Vector3.forward;
        foreach (PickableItem pickableItem in trackedItems)
        {

            // Check if the object is close enough
            float distance = Vector3.Distance(pickableItem.transform.position, cameraPos);
            if (distance > pickableItem.GetInteractionRange())
            {
                continue;
            }

            // Calculate dot product between camera direction vector and object direction vector
            Vector3 directionToItem = (pickableItem.transform.position - cameraPos).normalized;
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
                foundItem = pickableItem;
            }
        }
        return foundItem;
    }

    private void UpdateSelectedItem(PickableItem item)
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

    public void StartTrackingItem(PickableItem item)
    {
        if (trackedItems.Contains(item))
        {
            throw new System.Exception("Strange behavior: trying to start tracking an item that is already registered");
        }
        trackedItems.Add(item);
        item.SetSelected(false);
    }

    public void StopTrackingItem(PickableItem item)
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
