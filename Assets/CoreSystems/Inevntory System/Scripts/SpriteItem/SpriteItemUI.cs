using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;


public class SpriteItemUI : MonoBehaviour
{


    [SerializeField] protected SpriteItem item;
    [SerializeField] protected SlotContainerUI containerUI;
    protected SlotContainer container => containerUI.GetContainer();

    // non-serializable
    private Image imageComponent;
    private RectTransform spriteRendererTransform;
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
            spriteRendererTransform = gameObject.GetComponent<RectTransform>();
            imageComponent.sprite = item.GetSprite();
        }

    }

    protected void LateUpdate()
    {
        if (imageComponent != null)
        {
            int index = container.FindItem(item);
            if (index == -1)
            {
                Debug.LogError("Handling slot item UI: missing item in the container");
            }
            Rect worldRect = containerUI.GetWorldPositionRectForIndex(index);
            if (worldRect != previousFrameWorldRectSet)
            {
                setSpriteToRect(worldRect);
                previousFrameWorldRectSet = worldRect;
            }
        }
    }

    private void setSpriteToRect(Rect worldRect)
    {
        Vector2 worldLeftDown = worldRect.min;
        Vector2 worldRightUp = worldRect.max;   
        Vector2 localLeftDown = spriteRendererTransform.parent.InverseTransformPoint(worldLeftDown);
        Vector2 localRightUp = spriteRendererTransform.parent.InverseTransformPoint(worldRightUp);
        Vector2 localCenter = (localLeftDown + localRightUp) / 2;
        spriteRendererTransform.localPosition = localCenter;
        spriteRendererTransform.sizeDelta = 0.9f * (localRightUp - localLeftDown);
        spriteRendererTransform.localScale = new Vector3(1, 1, 1);
    }

    public void Init(SpriteItem item, SlotContainerUI containerUI)
    {
        this.containerUI = containerUI;
        this.item = item;
    }
}