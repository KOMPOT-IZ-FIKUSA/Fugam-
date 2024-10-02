using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu]
public class SpriteItem : SlotItem
{
    [SerializeField] private Sprite sprite;
    public string itemName;

    public Sprite GetSprite() { return sprite; }

    public override MonoBehaviour CreateUI(SlotContainerUI containerUI)
    {
        GameObject owner = new GameObject(itemName);
        owner.transform.parent = containerUI.transform;
        SpriteItemUI ui = owner.AddComponent<SpriteItemUI>();
        ui.Init(this, containerUI);
        return ui;
    }
}