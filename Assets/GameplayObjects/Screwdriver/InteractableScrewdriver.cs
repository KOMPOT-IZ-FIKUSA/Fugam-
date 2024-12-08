using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InteractableScrewdriver : InteractableObject
{
    private AudioSource CobwebDestroy;
    private PlayerInventory inventory;
    [SerializeField] public SlotItem screwdriverItemSource;
    public GameObject Cobweb;
    public GameObject EmptyObject; //when screwdriver destroys so cobweb sound can still play
    private float destroyDelay = 0.2f; //delay for destroying objects
    private float destroyDelay2 = 0.5f;
    private bool picked = false;



    public override List<InteractionOptionInstance> GetAvailabeleOptions()
    {
        List<InteractionOptionInstance> options = new List<InteractionOptionInstance>();
        if (inventory.CanAddItem())
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Pick up"));
        } else
        {
            options.Add(new InteractionOptionInstance(InteractionOption.PICK_UP, "Inventory full"));
        }
        return options;
    }

    public override void Interact(InteractionOptionInstance option)
    {
        base.Interact(option);
        if (option.option == InteractionOption.PICK_UP && !picked)
        {
            if (inventory.CanAddItem())
            {

                inventory.AddItem(screwdriverItemSource.Copy());
                // Play destroy sound from the separate GameObject
                AudioSource CobwebDestroy = EmptyObject.GetComponent<AudioSource>();
                if (CobwebDestroy != null)
                {
                    CobwebDestroy.Play();
                }
                // Destroy cobweb and screwdriver objects
                Destroy(Cobweb, destroyDelay);
                Destroy(gameObject, destroyDelay2);
                picked = true;
            }
        }
    }


    protected override void Start()
    {
        CobwebDestroy = GetComponent<AudioSource>();
        base.Start();
        inventory = FindObjectOfType<PlayerInventory>();
        if (inventory == null)
        {
            Debug.LogError("Cannot find player inventory");
        }
    }
    protected override void Update()
    {
        base.Update();
    }
}
