using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("General")]//Header
    public List<itemType> inventoryList;//
    public int selectedItem;
    public float playerReach;
    [SerializeField] GameObject throwItem_gameobject;

    [Space(20)]
    [Header("Keys")]
    [SerializeField] KeyCode throwitemKey;
    [SerializeField] KeyCode pickItemKey;

    [Space(20)]
    [Header("Item gameobjects")]
    [SerializeField] GameObject hammer_item;
    [SerializeField] GameObject gun_item;
    [SerializeField] GameObject axe_item;

    [Space(20)]
    [Header("Item prefabs")]
    [SerializeField] GameObject hammer_prefab;
    [SerializeField] GameObject gun_prefab;
    [SerializeField] GameObject axe_prefab;

    [Space(20)]
    [Header("UI")]
    [SerializeField] Image[] inventorySlotImage = new Image[9];
    [SerializeField] Image[] inventoryBackgroundImage = new Image[9];
    [SerializeField] Sprite emptySlotSprite;



    [SerializeField] Camera cam;//#2
    [SerializeField] GameObject pickUpItem_gameObject;

    private Dictionary<itemType, GameObject> itemSetActive = new Dictionary<itemType, GameObject>() { };
    private Dictionary<itemType, GameObject> itemInstantiate = new Dictionary<itemType, GameObject>() { };



    private void Start()
    {
        itemSetActive.Add(itemType.Hammer, hammer_item);
        itemSetActive.Add(itemType.Gun, gun_item);
        itemSetActive.Add(itemType.Axe, axe_item);

        itemInstantiate.Add(itemType.Hammer, hammer_prefab);
        itemInstantiate.Add(itemType.Gun, gun_prefab);
        itemInstantiate.Add(itemType.Axe, axe_prefab);

        NewItemSelected();
    }

    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        Debug.DrawLine(ray.origin, ray.direction);
        if (Physics.Raycast(ray, out hitInfo, playerReach))
        {
            IPickable item = hitInfo.collider.GetComponent<IPickable>();
            if (item != null)
            {
                pickUpItem_gameObject.SetActive(true);
                if (Input.GetKey(pickItemKey))
                {
                    inventoryList.Add(hitInfo.collider.GetComponent<ItemPickable>().itemScriptableObjects.item_type);
                    item.PickItem();
                }

            }
            else
            {
                pickUpItem_gameObject.SetActive(false);
            }
        }
        else
        {
            pickUpItem_gameObject.SetActive(false);

        }
        // Item throw
        if (Input.GetKeyDown(throwitemKey) && inventoryList.Count > 0)
        {
            Instantiate(itemInstantiate[inventoryList[selectedItem]], position: throwItem_gameobject.transform.position, new Quaternion());
            inventoryList.RemoveAt(selectedItem);
            if (selectedItem != 0)
            {
                selectedItem -= 1;
            }
            NewItemSelected();
        }

        //UI
        for (int i = 0; i < 9; i++)
        {
            if (i < inventoryList.Count)
            {
                inventorySlotImage[i].sprite = itemSetActive[inventoryList[i]].GetComponent<Item>().itemScriptableObject.item_sprite;
                Debug.Log($"This is {i}");
            }
            else
            {
                inventorySlotImage[i].sprite = emptySlotSprite;
            }
        }

        int a = 0;
        foreach(Image image in inventoryBackgroundImage)
        {
            if (a == selectedItem)
            {
                image.color = new Color32(145,255,126,255);
            }
            else
            {
                image.color = new Color32(219,219,219,255);
            }
            a++;
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && inventoryList.Count > 0)
        {
            selectedItem = 0;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && inventoryList.Count > 1)
        {
            selectedItem = 1;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3) && inventoryList.Count > 2)
        {
            selectedItem = 2;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4) && inventoryList.Count > 3)
        {
            selectedItem = 3;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5) && inventoryList.Count > 4)
        {
            selectedItem = 4;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6) && inventoryList.Count > 5)
        {
            selectedItem = 5;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && inventoryList.Count > 6)
        {
            selectedItem = 6;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8) && inventoryList.Count > 7)
        {
            selectedItem = 7;
            NewItemSelected();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9) && inventoryList.Count > 8)
        {
            selectedItem = 8;
            NewItemSelected();
        }

    }

    private void NewItemSelected()
    {
        hammer_item.SetActive(false);
        gun_item.SetActive(false);
        axe_item.SetActive(false);
        if (inventoryList.Count > 0)
        {
            GameObject selectedItemGameObject = itemSetActive[inventoryList[selectedItem]];
            selectedItemGameObject.SetActive(true);
        }

    }
}

public interface IPickable
{
    void PickItem();
}
