using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class InteractableItem : MonoBehaviour
{

    private Rigidbody rb;
    private Outline outline;
    private PlayerInteractController playerController;

    [SerializeField] private float interactionRange = 3;

    public float GetInteractionRange() {  return interactionRange; }

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb is null)
        {
            throw new MissingComponentException("Error: Cannot find rigidbody for interactable object");
        }

        outline = GetComponent<Outline>();
        if (outline is null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        playerController = FindObjectOfType<PlayerInteractController>();
        if (playerController == null)
        {
            throw new System.Exception("Cannot find player controller for interactable object");
        }
        playerController.StartTrackingItem(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDestroy()
    {

        playerController.StopTrackingItem(this);
    }

    public void SetSelected(bool selected)
    {
        outline.enabled = selected;
    }
}
