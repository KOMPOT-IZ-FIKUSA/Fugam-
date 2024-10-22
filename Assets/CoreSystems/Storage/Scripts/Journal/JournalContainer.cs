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
    public JournalContainer Copy()
    {
        JournalContainer instance = ScriptableObject.CreateInstance<JournalContainer>();
        for (int i = 0; i < MAX_JOURNAL_ITEMS; i++)
        {
            instance.SetItem(i, GetItem(i));
        }
        return instance;
    }
}
