using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HotbarContainer : SlotContainer
{


    public const int MAX_HOTBAR_ITEMS = 9;
    private void Awake()
    {
        setCapability(MAX_HOTBAR_ITEMS);
    }

    public void Init()
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

