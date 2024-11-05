using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clue : InteractableObject
{
    [SerializeField] private ClueData clueData;
    [SerializeField] private GameObject clueUIPanel;
    private InteractionUIController interactionUIController;

    
    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        CollectClue();
    }

    private void CollectClue() //it will destroy the 3D item and set the sprite data to the assigned CLueUIPanel
    {
        ClueManager.Instance.SetCurrentClue(clueData);
        
        if (clueUIPanel != null)
        {
            clueUIPanel.GetComponent<Image>().sprite = clueData.sprite; 
            clueUIPanel.SetActive(true); 
            
            interactionUIController.hints.enabled = true;
            interactionUIController.hints.text = clueData.clueMessage;
            // Invoke Clear message after the delay
            interactionUIController.Invoke("ClearMessage", interactionUIController.hintDelay);
        }
        
        Destroy(gameObject);
    }
    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        return new List<InteractionOptionInstance>()
        {
            new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick a clue")
        };
    }

    protected override void Start()
    {
        base.Start();
        interactionUIController = FindObjectOfType<InteractionUIController>();

    }
}
