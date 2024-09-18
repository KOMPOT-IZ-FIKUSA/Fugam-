using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickableItem : MonoBehaviour
{

    private Rigidbody rb;
    private Outline outline;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb is null)
        {
            print("Error: Cannot find rigidbody for pickable object");
        }

        outline = GetComponent<Outline>();
        if (outline is null)
        {
            print("Error: Cannot find outline for pickable object");
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
