using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SlotContainerUI : MonoBehaviour
{
    [SerializeField, HideInInspector] protected SlotContainer container;
    public abstract Rect GetWorldPositionRectForIndex(int index);

    public SlotContainer GetContainer() { return container; }
}
