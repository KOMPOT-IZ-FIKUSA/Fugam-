using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Clue : InteractableObject
{
    [SerializeField] private ClueData clueData;
    [SerializeField] private GameObject clueUIPanel;
    [SerializeField] private GameObject enlargedCluePanel;
    [SerializeField] private Animation enlargeAnimation; //Inspection Zoom, Not working/decided yet
    [SerializeField] private GameObject clueInspectionPanel;

    protected override void Start()
    {
        base.Start();
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        CollectClue();
    }

    private void CollectClue() //it will destroy the 3D item and set the sprite data to the assigned CLueUIPanel
    {
        if (clueUIPanel != null)
        {
            clueUIPanel.GetComponent<Image>().sprite = clueData.sprite; 
            clueUIPanel.SetActive(true); 
        }
        
        gameObject.SetActive(false);
        
    }

    public void InspectClue() //it will assign the sprite to the deactived big panel on the ui and activate the panel
    { //zooming
        if (enlargedCluePanel != null)
        {
            enlargedCluePanel.GetComponent<Image>().sprite = clueData.sprite;
            clueInspectionPanel.SetActive(true);
            if (enlargeAnimation != null)
            {
                enlargeAnimation.Play("ClueEnlargeAnimation"); 
            }
        }
    }

    public void CloseInspection()
    {
        clueInspectionPanel.SetActive(false);
    }

    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        return new List<InteractionOptionInstance>()
        {
            new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick a clue")
        };
    }
}
