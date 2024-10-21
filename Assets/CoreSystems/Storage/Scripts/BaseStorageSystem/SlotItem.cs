using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public abstract class SlotItem : ScriptableObject
{
    /// <summary>
    /// For the item, creates the UI component.
    /// </summary>
    /// <param name="container">The container that has the item</param>
    /// <returns>A component instance</returns>
    public abstract MonoBehaviour CreateUI(SlotContainerUI container);

    public virtual SlotItem Copy() {
        return Instantiate(this);
    }


    public override bool Equals(object obj)
    {
        // Since there are no other properties just require type equality
        return this.GetType() == obj.GetType();
    }
    public override int GetHashCode()
    {
        return base.GetHashCode();
    }




}

/// <summary>
/// A base container class for SlotItem s
/// </summary>
public abstract class SlotContainer : ScriptableObject
{
    [SerializeField] private SlotItem[] items;



    /// <summary>
    /// Searches for an item in the container. If not found, returns -1.
    /// </summary>
    /// <param name="item"></param>
    /// <returns>An index of the item or -1 if not found</returns>
    public int FindItem(SlotItem item)
    {
        for (int i = 0; i < GetCapability(); i++)
        {
            if (GetItem(i) == item)
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

    public int GetCapability()
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

    public SlotItem GetItem(int index)
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



