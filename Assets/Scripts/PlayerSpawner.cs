using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerSpawner : MonoBehaviour
{

    [SerializeField] private GameObject playerPrefab;
    // Start is called before the first frame update

    private void Awake()
    {

        PlayerSpawner[] spawners = FindObjectsOfType<PlayerSpawner>();
        if (spawners.Length == 1)
        {

            Camera camera = GetComponent<Camera>();
            if (camera != null)
            {
                Destroy(camera);
            }
            GameObject player = Instantiate(playerPrefab);
            player.transform.position = transform.position;
            player.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
        else
        {
            throw new System.Exception($"{spawners.Length} PlayerSpawner's found (expected 1)");
        }
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
