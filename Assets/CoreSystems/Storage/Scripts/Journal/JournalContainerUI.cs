using UnityEngine;
using UnityEngine.UI;

public class JournalContainerUI : SlotContainerUI
{
   [SerializeField] public Image[] slots = new Image[JournalContainer.MAX_JOURNAL_ITEMS];

   protected override Image[] Slots => slots;
   
   protected void OnEnable()
   {
      // makes sure that the container is there
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
      createItemsUI();
   }

   private PlayerInventory inventory;
   
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

      ValidateSlots();
   }

   private void ValidateSlots()
   {
      for (int i = 0; i < slots.Length; i++)
      {
         if (slots[i] == null)
         {
            Debug.LogError($"Journal slot {i} has no slot");
         }
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
}
