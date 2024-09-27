using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO: implement raycasting
/* example:
 * 
 * 
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawLine(ray.origin, ray.direction);
        if (Physics.Raycast(ray, out hitInfo, playerReach))
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                pickUpItem_gameObject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    Items.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObjects.item_type);
                    item.PickItem();
                }

            }
            else
            {
                pickUpItem_gameObject.SetActive(false);
            }
        }
        else
        {
            pickUpItem_gameObject.SetActive(false);

        }
 * 
 * 
 */



public class PlayerInteractController : MonoBehaviour
{
    /*
     * List of interactable items that are in the scene
     * Every time an item is spawned it registers in StartTrackingItem
     * Every time an item leaves the scene it stops tracking
     */
    [SerializeField] private List<InteractableItem> trackedItems = new List<InteractableItem>();

    [SerializeField] private Camera playerCamera;

    [SerializeField] private float selectedItemMaxAngle = 15;

    private InteractionUIController interactionUIController;

    private InteractableItem selectedItem;


    public Camera GetPlayerCamera() { return playerCamera; }
    public InteractableItem GetSelectedItem() { return selectedItem; }
    
    private void Start()
    {
        interactionUIController = FindObjectOfType<InteractionUIController>();
        if (interactionUIController == null)
        {
            throw new MissingComponentException("InteractionUIController");
        }
    }

    void Update()
    {
        SetSelectedItem(FindSelectedItem());

        if (selectedItem != null)
        {
            foreach (InteractionOptionInstance option in selectedItem.GetAvailabeleOptions())
            {
                if (Input.GetKeyDown(OptionsKeyMap.map[option.option].keyCode))
                {
                    selectedItem.Interact(option);
                }
            }

        }
    }

    private InteractableItem FindSelectedItem()
    {
        float maxAngleCos = Mathf.Cos(selectedItemMaxAngle / 180 * Mathf.PI);
        InteractableItem foundItem = null;

        float bestMatch = -2;

        Vector3 cameraPos = playerCamera.transform.position;
        Vector3 cameraDirection = playerCamera.transform.rotation * Vector3.forward;
        foreach (InteractableItem item in trackedItems)
        {

            // Check if the object is close enough
            float distance = Vector3.Distance(item.transform.position, cameraPos);
            if (distance > item.GetInteractionRange())
            {
                continue;
            }

            // Calculate dot product between camera direction vector and object direction vector
            Vector3 directionToItem = (item.GetUILabelPosition() - cameraPos).normalized;
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

    private void SetSelectedItem(InteractableItem item)
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
            SetSelectedItem(null);
        }
        item.SetSelected(false);
    }

}
