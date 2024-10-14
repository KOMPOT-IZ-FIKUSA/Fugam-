using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class JournalContainerUI : SlotContainerUI
{
   [SerializeField] protected Image[] slots = new Image[JournalContainer.MAX_JOURNAL_ITEMS];
   
   protected PlayerInventory inventory { get; private set; }
   protected int lastSelectedSlotIndex = -1;

   private void Start()
   {
      if (inventory == null)
      {
         inventory = FindObjectOfType<PlayerInventory>();
      }

      if (container == null)
      {
         container = inventory.GetHotbarContainer(); // this needs to be changed
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
      throw new System.NotImplementedException();
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
}
