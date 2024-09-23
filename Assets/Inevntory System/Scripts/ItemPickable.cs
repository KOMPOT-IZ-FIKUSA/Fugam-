using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickable : MonoBehaviour, IPickable
{
    public ItemSO itemScriptableObjects;

    public void PickItem()
    {
        Destroy(gameObject);
    }
}
