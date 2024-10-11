using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The class is responsible for positioning and serring text of the UI label when a player can interact with the InteractableObject
/// </summary>
public class InteractionUIController : MonoBehaviour
{
    // UI label object that shows signs line "[E] Pull object"
    [SerializeField] public Text text;
    [SerializeField] public TextMeshProUGUI hints;

    public float hintDelay = 2.25f;
    public float messageDelay = 2.25f;

    private PlayerInteractController interactController;

    void Start()
    {
        interactController = FindObjectOfType<PlayerInteractController>();
    }

    /// <summary>
    /// Sets enabled property of the UI label depending on the selected object.
    /// If an interactable object is selected, sets its screen position.
    /// The screen position is calculated as a projection of the point an object returns.
    /// The projection is done from 3D world space to camera space.
    /// </summary>
    void LateUpdate()
    {
        InteractableObject obj = interactController.GetSelectedObject();

        // validate
        if (interactController == null)
        {
            Debug.LogError("Cannot find " + typeof(PlayerInteractController).Name);
        }

        // If nothing is selected
        if ( obj == null)
        {
            text.enabled = false;            
        } else
        {
            text.enabled = true;
            // Get interactions as string
            string interactions = GetInteractionsFromItemAsString(obj);
            // Update label content if necessary
            if (text.text != interactions)
            {
                text.text = interactions;
            }

            Camera camera = interactController.GetPlayerCamera();
            // Project 3D point from obj.GetUILabelPosition() to camera space and set to the UI label's position
            text.transform.position = camera.WorldToScreenPoint(obj.GetUILabelPosition());
        }
    }

    /// <summary>
    /// Creates a string that contains all interactions that can be currently done for an item.
    /// The string contains pairs of keys and names of interactions.
    /// String example: "[E] Open\n[Y] Destroy\n"
    /// </summary>
    /// <param name="item"></param>
    /// <returns>A string containing lines like: [Key] IntractionName</returns>
    private string GetInteractionsFromItemAsString(InteractableObject item)
    {
        string labelContent = "";
        foreach (InteractionOptionInstance option in item.GetAvailabeleOptions())
        {
            string button = OptionsKeyMap.map[option.option].name;
            labelContent += $"[{button}] {option.gameplayName}\n";
        }
        return labelContent;
    }
    public void HintMessage()
    {
        hints.text = "";
        hints.enabled = false;
    }
    public void EndMessage()
    {
        hints.text = "";
        hints.enabled = false;
    }
}
