using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder.Shapes;


public class SpriteItemUI : MonoBehaviour
{


    [SerializeField] protected SpriteItem item;
    [SerializeField] protected SlotContainerUI containerUI;
    protected SlotContainer container => containerUI.GetContainer();

    // non-serializable
    private SpriteRenderer spriteRenderer;

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

        if (spriteRenderer == null)
        {
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = item.GetSprite();
            int index = container.FindItem(item);
            if (index == -1)
            {
                Debug.LogError("Handling slot item UI: missing item in the container");
            }
            else
            {
                Rect rect = containerUI.GetRectForIndex(index);
                setSpriteToRect(rect);
            }
        }
    }

    private void setSpriteToRect(Rect rect)
    {
        Vector2 position = rect.center;
        spriteRenderer.transform.localPosition = new Vector3(position.x, position.y, spriteRenderer.transform.position.z);

        // Set the sprite's scale based on the size of the Rect
        Vector2 size = rect.size;
        spriteRenderer.transform.localScale = new Vector3(size.x, size.y, spriteRenderer.transform.localScale.z);
    }

    public void Init(SpriteItem item, SlotContainerUI containerUI)
    {
        this.containerUI = containerUI;
        this.item = item;
    }
}