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
        setItem(index, null);
    }

    public void SetItem(int index, SlotItem item)
    {
        setItem(index, item);
    }
}


