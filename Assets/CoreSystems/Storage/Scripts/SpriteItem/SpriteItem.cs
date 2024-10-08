using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

/// <summary>
/// A SlotItem with a Sprite. Can be easily rendered in SlotContainer s
/// </summary>
[CreateAssetMenu]
public class SpriteItem : SlotItem
{
    /// <summary>
    /// A sprite that is associated with this item. This is set as an image of the slot.
    /// </summary>
    [SerializeField] private Sprite sprite;
    public string itemName;

    public Sprite GetSprite() { return sprite; }

    /// <summary>
    /// Creates an empty child of containerUI. Adds a SpriteItemUI component to it. Initializes the UI for this item.
    /// </summary>
    /// <param name="containerUI"></param>
    /// <returns>A created component</returns>
    public override MonoBehaviour CreateUI(SlotContainerUI containerUI)
    {
        GameObject owner = new GameObject(itemName);
        owner.transform.parent = containerUI.transform;
        SpriteItemUI ui = owner.AddComponent<SpriteItemUI>();
        ui.Init(this, containerUI);
        return ui;
    }

}