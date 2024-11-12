using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;

/// <summary>
/// A UI component to draw SpriteItem s
/// </summary>
public class SpriteItemUI : MonoBehaviour
{
    protected SpriteItem item;
    protected SlotContainerUI containerUI;
    protected SlotContainer container => containerUI.GetContainer();
    private Image imageComponent;
    private RectTransform imageTransform;
    private Rect previousFrameWorldRectSet;

    private bool _isDragged = false;

    protected void Update()
    {
        if (item == null)
        {
            Debug.LogError("Handling item UI with null item");
            return;
        }
        if (containerUI == null || container == null)
        {
            Debug.LogError("Handling item UI with null containerUI");
            return;
        }

        int index = container.FindItem(item);
        if (index == -1)
        {
            Destroy(gameObject);
            return;
        }

        if (imageComponent == null)
        {
            imageComponent = gameObject.AddComponent<Image>();
            imageTransform = gameObject.GetComponent<RectTransform>();
            imageComponent.sprite = item.GetSprite();
        }

        _handleInteraction();
    }


    private void _handleInteraction()
    {

        if (Input.GetMouseButtonDown(0))
        {
            int index = container.FindItem(item);
            Rect worldRect = containerUI.GetWorldPositionRectForIndex(index);
            // Start dragging if the mouse is above
            if (worldRect.Contains(Input.mousePosition))
            {
                _startDragging();
            }
        }
        if (_isDragged)
        {
            if (Input.GetMouseButtonUp(0))
            {
                _stopDragging();
            }
        }
    }

    private void _stopDragging()
    {
        if (!_isDragged)
        {
            Debug.Log("Strange behavior: _stopDragging called without active dragging");
        }
        _isDragged = false;

        SlotContainerUI[] allContainers = FindObjectsOfType<SlotContainerUI>();
        foreach (SlotContainerUI containerUI in allContainers)
        {
            SlotContainer container = containerUI.GetContainer();
            if (containerUI.isActiveAndEnabled)
            {
                for (int index = 0; index < container.GetCapability(); index += 1)
                {
                    Rect worldRect = containerUI.GetWorldPositionRectForIndex(index);
                    if (worldRect.Contains(Input.mousePosition))
                    {
                        if (container.GetItem(index) == null)
                        {
                            this.container.RemoveItem(item);
                            container.SetItem(index, item);
                            if (container != this.container)
                            {
                                //Destroy(gameObject);
                            }
                            return;
                        }
                    }
                }
            }
        }
    }


    private void OnDestroy()
    {
        item = null;
        containerUI = null;
    }


    private void _startDragging()
    {
        _isDragged = true;
    }

    protected void LateUpdate()
    {
        if (imageComponent != null)
        {
            // Find index of the item in the containerUI
            int index = container.FindItem(item);

            // If is not in the containerUI, destroy self. Item will be handled by some other containerUI if still exists.
            if (index == -1)
            {
                //Debug.Log($"Item {item.itemName} not found in containerUI, destroying UI.");
                Destroy(gameObject);
                return;  // Exit the method to avoid executing further code.
            }

            // Find proper location
            Rect worldRect = containerUI.GetWorldPositionRectForIndex(index);
            if (_isDragged)
            {
                worldRect.center = Input.mousePosition;
            }
            // If changed, set it
            if (worldRect != previousFrameWorldRectSet)
            {
                setSpriteToRect(worldRect);
                previousFrameWorldRectSet = worldRect;
            }

        }
    }

    /// <summary>
    /// Sets the position of the image with item's sprite to the argument
    /// </summary>
    /// <param name="worldRect">World position of the UI image slot</param>
    private void setSpriteToRect(Rect worldRect)
    {
        Vector2 worldLeftDown = worldRect.min;
        Vector2 worldRightUp = worldRect.max;
        Vector2 localLeftDown = imageTransform.parent.InverseTransformPoint(worldLeftDown);
        Vector2 localRightUp = imageTransform.parent.InverseTransformPoint(worldRightUp);
        Vector2 localCenter = (localLeftDown + localRightUp) / 2;
        imageTransform.localPosition = localCenter;
        imageTransform.sizeDelta = localRightUp - localLeftDown;
        imageTransform.localScale = new Vector3(1, 1, 1);
    }

    public void Init(SpriteItem item, SlotContainerUI containerUI)
    {
        this.containerUI = containerUI;
        this.item = item;
        //transform.SetParent(containerUI.transform, false);
    }


}