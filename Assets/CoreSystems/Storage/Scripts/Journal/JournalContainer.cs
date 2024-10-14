using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu]
public class JournalContainer : SlotContainer
{
    public const int MAX_JOURNAL_ITEMS = 15;

    private void Awake()
    {
        setCapability(MAX_JOURNAL_ITEMS);
    }

    private void DeleteItem(int index)
    {
        setItem(index, null);
    }

    public void SetItem(int index, SlotItem item)
    {
        setItem(index, item);
    }
}
