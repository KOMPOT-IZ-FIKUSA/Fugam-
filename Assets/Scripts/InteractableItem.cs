using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public abstract class InteractableItem : MonoBehaviour
{

    protected Outline outline;
    public PlayerInteractController playerController { get; private set; }

    public Camera playerCamera => playerController.GetPlayerCamera();

    [SerializeField] protected float interactionRange = 3;

    public float GetInteractionRange() {  return interactionRange; }

    protected virtual void Awake()
    {
    }

    // Start is called before the first frame update
    protected virtual void Start()
    {

        outline = GetComponent<Outline>();
        if (outline is null)
        {
            outline = gameObject.AddComponent<Outline>();
            outline.OutlineColor = Color.yellow;
            outline.OutlineWidth = 5;
        }

        playerController = FindObjectOfType<PlayerInteractController>();
        if (playerController == null)
        {
            throw new System.Exception("Cannot find player controller for interactable object");
        }
        playerController.StartTrackingItem(this);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        
    }

    protected virtual void OnDestroy()
    {

        playerController.StopTrackingItem(this);
    }

    public virtual void SetSelected(bool selected)
    {
        outline.enabled = selected;
    }

    public virtual void Interact(InteractionOptionInstance option)
    {

    }

    public abstract List<InteractionOptionInstance> GetAvailabeleOptions();

    public virtual Vector3 GetUILabelPosition()
    {
        return transform.position;
    }
}
