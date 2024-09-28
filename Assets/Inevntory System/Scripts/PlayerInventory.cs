using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private HotbarContainer _hotbar;

    public int SelectedSlot = -1;
    
    [Space(20)]
    [Header("Keys")]
    [SerializeField] private KeyCode throwitemKey;

    // Non-serializable camera and hotbarUI
    private Camera cam;
    private HotbarContainerUI hotbarContainerUI;

    public HotbarContainer GetHotbarContainer()
    {
        return _hotbar;
    }

    private void Awake()
    {
        if (_hotbar == null)
        {
            _hotbar = ScriptableObject.CreateInstance<HotbarContainer>();
            _hotbar.Init();
        }
    }

    private void Start()
    {
        if (cam == null)
        {
            cam = FindObjectOfType<Camera>();
            if (cam == null )
            {
                Debug.LogError("Cannot find player camera");
            }
        }
        if (hotbarContainerUI == null)
        {
            hotbarContainerUI = FindObjectOfType<HotbarContainerUI>();
            if (hotbarContainerUI == null )
            {
                Debug.LogError("Cannot find player camera");
            }    
        }

        for (int i = 0; i < HotbarContainer.MAX_HOTBAR_ITEMS; i++)
        {
            SlotItem item = _hotbar.GetItem(i);
            if (item == null) { continue; }
            GameObject itemUIGameObject = new GameObject();
            itemUIGameObject.name = "HotbarItem" + (i + 1);
            itemUIGameObject.transform.parent = hotbarContainerUI.transform;
            item.CreateUI(itemUIGameObject, hotbarContainerUI);
        }
    }

    private readonly KeyCode[] keyCodes = new KeyCode[]
   {
        KeyCode.Alpha1,
        KeyCode.Alpha2,
        KeyCode.Alpha3,
        KeyCode.Alpha4,
        KeyCode.Alpha5,
        KeyCode.Alpha6,
        KeyCode.Alpha7,
        KeyCode.Alpha8,
        KeyCode.Alpha9
   };
    void Update()
    {
        // Item throw
        if (Input.GetKeyDown(throwitemKey))
        {
            SlotItem item = _hotbar.GetItem(SelectedSlot);
            if (item is IThrowableItem)
            {
                Vector3 lookDirection = cam.transform.rotation * Vector3.forward;
                (item as IThrowableItem).CreateGameObject(cam.transform.position, lookDirection);
                _hotbar.DeleteItem(SelectedSlot);
            }
        }


        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                SelectedSlot = i;
            }
        }
    }
}

