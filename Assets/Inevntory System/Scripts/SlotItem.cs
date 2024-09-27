using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public abstract class SlotItem : ScriptableObject
{
    public abstract MonoBehaviour CreateUI(GameObject owner, SlotContainerUI container);
}

public abstract class SlotContainer : ScriptableObject
{
    [SerializeField] private SlotItem[] items;

    public int FindItem(SlotItem item)
    {
        for (int i = 0; i < getCapability(); i++)
        {
            if (getItem(i) == item)
            {

                return i;
            }
        }
        return -1;
    }

    protected void setCapability(int num)
    {
        if (num < 0)
        {
            Debug.LogError($"Invalid container capability: {num} < 0");
            return;
        }
        if (items == null)
        {
            items = new SlotItem[num];
        }
        else if (num != items.Length)
        {
            SlotItem[] newArray = new SlotItem[num];
            Array.Copy(items, newArray, Math.Min(items.Length, num));
            items = newArray;
        }
    }

    protected int getCapability()
    {
        if (items == null)
        {
            Debug.LogError("Trying to get capability of a container that is not initialized properly");
            return 0;
        }
        return items.Length;
    }

    protected void setItem(int index, SlotItem item)
    {

        if (items == null)
        {
            Debug.LogError("Trying to set item to a container that is not initialized properly");
            return;
        }
        if (index < 0 && index >= items.Length)
        {
            Debug.LogError($"Cannot set item to [{index}] of the container with capability {items.Length}");
            return;
        }
        items[index] = item;
    }

    protected SlotItem getItem(int index)
    {
        if (items == null)
        {
            Debug.LogError("Trying to get item from a container that is not initialized properly");
            return null;
        }
        if (index < 0 || index >= items.Length)
        {
            Debug.LogError($"Cannot get item from [{index}] of the container with capability {items.Length}");
            return null;
        }
        return items[index];
    }
}



