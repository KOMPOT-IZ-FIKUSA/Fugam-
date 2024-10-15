using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalContainerUI : SlotContainerUI
{
   [SerializeField] public Image[] slots = new Image[JournalContainer.MAX_JOURNAL_ITEMS];
   
   protected override Image[] GetSlots()
   {
      return slots;
   }
   
   protected void OnEnable()
   {

      // Ensure container is initialized
      if (container == null)
      {
         if (inventory == null)
         {
            inventory = FindObjectOfType<PlayerInventory>();
         }

         if (inventory != null)
         {
            container = inventory.GetJournalContainer();
         }
         else
         {
            Debug.LogError("PlayerInventory not found when initializing JournalContainerUI.");
            return;
         }
      }

      // Ensure lastFrameItems is initialized
      if (lastFrameItems == null)
      {
         lastFrameItems = new HashSet<SlotItem>();
      }
      else
      {
         lastFrameItems.Clear();
      }

      createItemsUI();
   }
   
   protected PlayerInventory inventory { get; private set; }
   protected int lastSelectedSlotIndex = -1;

   private void Awake()
   {
      if (inventory == null)
      {
         inventory = FindObjectOfType<PlayerInventory>();
      }

      if (container == null)
      {
         container = inventory.GetJournalContainer();
      }

      validateSlots();
      setupSlotsSelection();
   }

   private void validateSlots()
   {
      for (int i = 0; i < slots.Length; i++)
      {
         if (slots[i] == null)
         {
            Debug.LogError($"Journal slot {i} has no slot");
         }
      }
   }

   private void setupSlotsSelection()
   {
      for (int i = 0; i < slots.Length; i++)
      {
         deselectSlot(i);
      }

      if (lastSelectedSlotIndex != -1)
      {
         selectSlot(lastSelectedSlotIndex);
      }
   }

   private void updateSlotSelection()
   {
      if (inventory == null)
      {
         Debug.LogError($"Inventory = null while updating hotbar slots selection");
      }

      if (inventory.SelectedSlot != lastSelectedSlotIndex)
      {
         if (lastSelectedSlotIndex != -1)
         {
            deselectSlot(lastSelectedSlotIndex);
         }

         if (inventory.SelectedSlot != -1)
         {
            selectSlot(inventory.SelectedSlot);
         }
         lastSelectedSlotIndex = inventory.SelectedSlot;
      }
   }
   public override Rect GetWorldPositionRectForIndex(int index)
   {
      if (index >= 0 && index < slots.Length)
      {
         RectTransform rectTransform = slots[index].GetComponent<RectTransform>();
         Vector2 leftDown = rectTransform.TransformPoint(rectTransform.rect.position);
         Vector2 rightUp = rectTransform.TransformPoint(rectTransform.rect.position + rectTransform.rect.size);
         return new Rect(leftDown, rightUp - leftDown);
      }
      return new Rect();
   }

   protected void deselectSlot(int index)
   {
      slots[index].color = Color.white;
   }

   protected void selectSlot(int index)
   {
      slots[index].color = Color.green;
   }

   protected override void Update()
   {
      base.Update();
      updateSlotSelection();
   }
   
   public bool IsMouseOverSlot(int index)
   {
      if (index < 0 || index >= slots.Length)
      {
         Debug.LogError($"Invalid slot index: {index}");
         return false;
      }

      RectTransform rectTransform = slots[index].GetComponent<RectTransform>();
      Vector2 localMousePos;
      bool isOver = RectTransformUtility.ScreenPointToLocalPointInRectangle(
         rectTransform,
         Input.mousePosition,
         null, // For Screen Space - Overlay canvases
         out localMousePos
      );

      // Log mouse position and rect information for debugging
      Debug.Log($"Slot {index} local mouse position: {localMousePos}");
      Debug.Log($"Slot {index} rect: {rectTransform.rect}");

      isOver = rectTransform.rect.Contains(localMousePos);
      Debug.Log($"Mouse over slot {index}: {isOver}");
      return isOver;
   }
}
