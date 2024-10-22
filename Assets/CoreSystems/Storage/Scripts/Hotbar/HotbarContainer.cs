using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A SlotContainer that stores items for the player's hotbar.
/// </summary>
[CreateAssetMenu]
public class HotbarContainer : SlotContainer
{

    /// <summary>
    /// Number of slots
    /// </summary>
    public const int MAX_HOTBAR_ITEMS = 9;

    private void Awake()
    {
        setCapability(MAX_HOTBAR_ITEMS);
    }


    public void DeleteItem(int index)
    {
        base.SetItem(index, null);
    }

    public new void SetItem(int index, SlotItem item)
    {
        base.SetItem(index, item);
    }

    public HotbarContainer Copy()
    {
        HotbarContainer instance = ScriptableObject.CreateInstance<HotbarContainer>();
        for (int i = 0; i < MAX_HOTBAR_ITEMS; i++)
        {
            instance.SetItem(i, GetItem(i));
        }
        return instance;
    }
}


