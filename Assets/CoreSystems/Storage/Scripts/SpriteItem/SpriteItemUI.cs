using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
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

    protected void Update()
    {
        if (item == null)
        {
            Debug.LogError("Handling item UI with null item");
            return;
        }
        if (containerUI == null || container == null)
        {
            Debug.LogError("Handling item UI with null container");
            return;
        }

        if (imageComponent == null)
        {
            imageComponent = gameObject.AddComponent<Image>();
            imageTransform = gameObject.GetComponent<RectTransform>();
            imageComponent.sprite = item.GetSprite();
        }

    }

    protected void LateUpdate()
    {
        if (imageComponent != null)
        {
            // Find index of the item in the container
            int index = container.FindItem(item);

            // If is not in the container, destroy self. Item will be handled by some other container if still exists.
            if (index == -1)
            {
                Destroy(gameObject);
            }

            // Find proper location
            Rect worldRect = containerUI.GetWorldPositionRectForIndex(index);
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
        imageTransform.sizeDelta = 0.9f * (localRightUp - localLeftDown);
        imageTransform.localScale = new Vector3(1, 1, 1);
    }

    public void Init(SpriteItem item, SlotContainerUI containerUI)
    {
        this.containerUI = containerUI;
        this.item = item;
    }
}