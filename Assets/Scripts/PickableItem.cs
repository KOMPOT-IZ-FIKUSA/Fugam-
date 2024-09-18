using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour
{

    private Rigidbody rb;
    private Outline outline;
    private PlayerPickupController playerController;

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
            throw new MissingComponentException("Error: Cannot find rigidbody for pickable object");
        }

        outline = GetComponent<Outline>();
        if (outline is null)
        {
            outline = gameObject.AddComponent<Outline>();
        }

        playerController = FindObjectOfType<PlayerPickupController>();
        if (playerController == null)
        {
            throw new System.Exception("Cannot find player controller for pickable object");
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
