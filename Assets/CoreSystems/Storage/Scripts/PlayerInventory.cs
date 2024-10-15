using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using static UnityEditor.Progress;

/// <summary>
/// The main class that sets up player inventory.
/// 1) Manages HotbarContainer
/// 2) It handles user input to select a slot, throw an item, etc...
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    [Header("Items")]
    [SerializeField] private HotbarContainer _hotbar;
    [SerializeField] private JournalContainer _journal;

    public int SelectedSlot = -1;
    
    [Space(20)]
    [Header("Keys")]
    [SerializeField] private KeyCode throwitemKey;

    [SerializeField] private KeyCode openJournalKey = KeyCode.J;
    

    // Non-serializable camera and hotbarUI
    private Camera cam;
    private HotbarContainerUI hotbarContainerUI;
    private JournalContainerUI journalContainerUI;
    
    private bool isJournalOpen = false;
    
    private SlotItem draggedItem = null;
    private int draggedItemIndex = -1;
    private SlotContainerUI sourceContainerUI = null;
    
    public HotbarContainer GetHotbarContainer()
    {
        return _hotbar;
    }

    public JournalContainer GetJournalContainer()
    {
        return _journal;
    }

    private void Awake()
    {
        if (_hotbar == null)
        {
            _hotbar = ScriptableObject.CreateInstance<HotbarContainer>();
        }

        if (_journal == null)
        {
            _journal = ScriptableObject.CreateInstance<JournalContainer>();
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

        if (journalContainerUI == null)
        {
            journalContainerUI = FindObjectOfType<JournalContainerUI>();
            if (journalContainerUI == null)
            {
                Debug.LogError("Cannot find player journalContainerUI");
            }
        }
        journalContainerUI.gameObject.SetActive(false);
    }

    private readonly KeyCode[] slotsKeyCodes = new KeyCode[]
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
        if (Input.GetKeyDown(openJournalKey))
        {
            ToggleJournal();
        }
        // if journal is not open then hotbar input will work
        if (!isJournalOpen)
        {
            HandleHotbarInput();
        }

        if (isJournalOpen)
        {
            HandleHotbarJournalInteraction();
        }
    }

    private void HandleHotbarJournalInteraction()
    {
        // If mouse is clicked, start dragging the item from either container
        if (Input.GetMouseButtonDown(0)) // Left-click to start dragging
        {
            for (int i = 0; i < hotbarContainerUI.slots.Length; i++)
            {
                if (hotbarContainerUI.IsMouseOverSlot(i)) // Check if mouse is over a hotbar slot
                {
                    StartDraggingItem(hotbarContainerUI, i); // Start dragging from the hotbar
                    return;
                }
            }

            for (int i = 0; i < journalContainerUI.slots.Length; i++)
            {
                if (journalContainerUI.IsMouseOverSlot(i)) // Check if mouse is over a journal slot
                {
                    StartDraggingItem(journalContainerUI, i); // Start dragging from the journal
                    return;
                }
            }
        }

        // If mouse is released, try to drop the item into the other container
        if (Input.GetMouseButtonUp(0)) // Release left-click to drop
        {
            for (int i = 0; i < hotbarContainerUI.slots.Length; i++)
            {
                if (hotbarContainerUI.IsMouseOverSlot(i))
                {
                    StopDraggingItem(hotbarContainerUI, i); // Drop the item into the hotbar
                    return;
                }
            }

            for (int i = 0; i < journalContainerUI.slots.Length; i++)
            {
                if (journalContainerUI.IsMouseOverSlot(i))
                {
                    StopDraggingItem(journalContainerUI, i); // Drop the item into the journal
                    return;
                }
            }

            // If the mouse was released outside any valid slot, cancel the drag
            CancelDragging();
        }
    }

    private void ToggleJournal()
    {
        isJournalOpen = !isJournalOpen;
        journalContainerUI.gameObject.SetActive(isJournalOpen);

        if (isJournalOpen)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void HandleHotbarInput()
    {
        // Throw item if player pressed the button and throwable item is in selected slot
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

        // Iterate over keys and if one is pressed, select that slot
        for (int i = 0; i < slotsKeyCodes.Length; i++)
        {
            if (Input.GetKeyDown(slotsKeyCodes[i]))
            {
                // Here it doesn't do any special slot selection procedures.
                // This class does not handle the UI.
                // It's the responsibility of HotbarContainerUI to change the selection in UI by accessing this variable
                SelectedSlot = i;
            }
        }
    }

    // Is any slot free
    public bool CanAddItem()
    {
        for (int i = 0; i < _hotbar.GetCapability(); i++)
        {
            // If a slot is epmty, return true
            if (_hotbar.GetItem(i) == null)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Adds item to an empty slot
    /// </summary>
    /// <param name="item"></param>
    /// <param name="selectSlot">Whether a slot will be selected after item is added</param>
    public void AddItem(SlotItem item, bool selectSlot = true)
    {
        if (item == null)
        {
            Debug.Log("Strange behavior: tried to add null item");
            return;
        }
        bool success = false;
        for (int i = 0; i < _hotbar.GetCapability(); i++)
        {
            // If a slot is epmty, return true
            if (_hotbar.GetItem(i) == null)
            {
                _hotbar.SetItem(i, item);
                if (selectSlot)
                {
                    SelectedSlot = i;
                }
                success = true;
                return;
            }
        }
        if (!success)
        {
            Debug.LogError("Tried to add an object to a full inventory");
        }
    }
    
    private void StartDraggingItem(SlotContainerUI containerUI, int slotIndex)
    {
        SlotItem item = containerUI.GetContainer().GetItem(slotIndex);
        if (item != null)
        {
            draggedItem = item;
            draggedItemIndex = slotIndex;
            sourceContainerUI = containerUI;
            containerUI.GetContainer().SetItem(slotIndex, null);  // Remove the item from the original slot
            containerUI.ForceUpdateUI(); // Update UI to reflect item removal
        }
    }

    private void StopDraggingItem(SlotContainerUI targetContainerUI, int slotIndex)
    {
        if (draggedItem != null)
        {
            if (targetContainerUI.GetContainer().GetItem(slotIndex) == null)
            {
                targetContainerUI.GetContainer().SetItem(slotIndex, draggedItem);
                targetContainerUI.ForceUpdateUI(); // Update UI to reflect item addition
                draggedItem = null;
                draggedItemIndex = -1;
                sourceContainerUI = null;
            }
            else
            {
                // Slot is occupied, return the item to the original container
                sourceContainerUI.GetContainer().SetItem(draggedItemIndex, draggedItem);
                sourceContainerUI.ForceUpdateUI(); // Update UI to reflect item return
                draggedItem = null;
                draggedItemIndex = -1;
                sourceContainerUI = null;
            }
        }

    }
    private void CancelDragging()
    {
        if (draggedItem != null)
        {
            sourceContainerUI.GetContainer().SetItem(draggedItemIndex, draggedItem);
            sourceContainerUI.ForceUpdateUI();
            draggedItem = null;
            draggedItemIndex = -1;
            sourceContainerUI = null;
        }
    }
}

