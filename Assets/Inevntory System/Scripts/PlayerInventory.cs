using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

public class PlayerInventory : MonoBehaviour
{
    [Header("Items")]
    public HotbarContainer Hotbar;
    [SerializeField] private HotbarContainerUI hotbarContainerUI;

    public int selectedSlot = -1;


    [Space(20)]
    [Header("Keys")]
    [SerializeField] private KeyCode throwitemKey;


    [SerializeField] Camera cam;//#2
    [SerializeField] GameObject pickUpItem_gameObject;

    private void Awake()
    {
        if (Hotbar == null)
        {
            Hotbar = ScriptableObject.CreateInstance<HotbarContainer>();
            Hotbar.Init();
        }
    }

    private void Start()
    {
        print(hotbarContainerUI.GetContainer() == Hotbar);

        for (int i = 0; i < HotbarContainer.MAX_HOTBAR_ITEMS; i++)
        {
            SlotItem item = Hotbar.GetItem(i);
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
            SlotItem item = Hotbar.GetItem(selectedSlot);
            if (item is ThrowableItem)
            {
                Vector3 lookDirection = cam.transform.rotation * Vector3.forward;
                (item as ThrowableItem).CreateGameObject(cam.transform.position, lookDirection);
                Hotbar.DeleteItem(selectedSlot);
            }
        }
        /*
         * 
         * 
        //UI
        for (int i = 0; i < 9; i++)
        {
            if (i < Items.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[Items[i]].GetComponent<Item>().itemScriptableObject.item_sprite;
                Debug.Log($"This is {i}");
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }
         * 
         */


        for (int i = 0; i < keyCodes.Length; i++)
        {
            if (Input.GetKeyDown(keyCodes[i]))
            {
                selectedSlot = i;
            }
        }
    }
}

public interface IPickable
{
    void PickItem();
}
