using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUIController : MonoBehaviour
{
    // UI label object
    [SerializeField] public Text text;

    private PlayerInteractController interactController;
    private Camera playerCamera;



    void Start()
    {
        interactController = FindObjectOfType<PlayerInteractController>();
        playerCamera = interactController.GetPlayerCamera();
    }


    void LateUpdate()
    {

        InteractableItem item = interactController.GetSelectedItem();

        if ( item == null)
        {
            text.enabled = false;            
        } else
        {
            text.enabled = true;
            string interactions = GetInteractionsFromItemAsString(item);
            if (text.text != interactions)
            {
                text.text = interactions;
            }
            text.transform.position = playerCamera.WorldToScreenPoint(item.GetUILabelPosition());
        }
    }


    private string GetInteractionsFromItemAsString(InteractableItem item)
    {
        string labelContent = "";
        foreach (InteractionOptionInstance option in item.GetAvailabeleOptions())
        {
            string button = OptionsKeyMap.map[option.option].name;
            labelContent += $"[{button}] {option.gameplayName}\n";
        }
        return labelContent;
    }
}
